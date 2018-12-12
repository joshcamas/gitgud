using System;
using System.Diagnostics;
using UnityEngine;

namespace GitGud
{
    public class GitGud
    {
        /// <summary>
        /// Run a git command, streaming onError events as they happen, and running onComplete 
        /// when process completes.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="onComplete"></param>
        /// <param name="onError"></param>
        public static void RunCommand(string command,Action<CommandOutput> onComplete)
        {
            string outputData = null;
            string errorData = null;

            if(GitGudSettings.GetBool("debug"))
                UnityEngine.Debug.Log("Running command " + command);

            RunCommandAsync(command, 
                (output) => {

                    //Output stream
                    if (output != null)
                    {
                        if (outputData == null)
                            outputData = output;
                        else
                            outputData += "\n" + output;

                    }
                }, 
                
                (error) => {
                    //Error stream
                    if (error != null)
                    {
                        if (errorData == null)
                            errorData = error;
                        else
                            errorData += "\n" + error;
                    }
                },
                () =>
                {
                    //Debugging
                    if (GitGudSettings.GetBool("debug") && outputData != null)
                        UnityEngine.Debug.Log("<color=blue>Output:</color>" + outputData);

                    if (GitGudSettings.GetBool("debug") && errorData != null)
                        UnityEngine.Debug.Log("<color=red>Error:</color>" + errorData);

                    CommandOutput output = new CommandOutput(outputData, errorData);
                    if(onComplete != null)
                        onComplete(output);
                }
                );
        }

        /// <summary>
        /// Run a git command, with streaming onOutput and onError events as they happen, 
        /// and then running onComplete when the process completes
        /// </summary>
        /// <param name="command"></param>
        /// <param name="onComplete"></param>
        /// <param name="onError"></param>
        public static void RunCommandAsync(string command, Action<string> onOutput, Action<string> onError, Action onComplete)
        {
            //Start info
            ProcessStartInfo gitInfo = new ProcessStartInfo();
            gitInfo.CreateNoWindow = true;
            gitInfo.RedirectStandardError = true;
            gitInfo.RedirectStandardOutput = true;
            gitInfo.FileName = GitGudSettings.GetString("git_path");
            gitInfo.Arguments = command;
            gitInfo.WorkingDirectory = GitGudSettings.GetString("repo_path");
            gitInfo.UseShellExecute = false;

            //Process
            Process gitProcess = new Process();
            gitProcess.StartInfo = gitInfo;

            //Hook events
            gitProcess.OutputDataReceived += (sender, args) => onOutput(args.Data);
            gitProcess.ErrorDataReceived += (sender, args) => onError(args.Data);

            try
            {
                //Run process
                gitProcess.Start();

                gitProcess.BeginOutputReadLine();
                gitProcess.BeginErrorReadLine();

                gitProcess.WaitForExit();
                gitProcess.Close();

            } catch
            {
                onError("Process for command " + command + " failed");
            }

            onComplete();
        }
    }

    /// <summary>
    /// Container data class that RunCommand() returns
    /// </summary>
    public struct CommandOutput
    {
        public CommandOutput(string outputData, string errorData)
        {
            this.outputData = outputData;
            this.errorData = errorData;
        }

        public string outputData;
        public string errorData;
    }
}