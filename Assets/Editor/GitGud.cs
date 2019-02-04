using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace GitGud
{
    public class GitGud
    {
        /// <summary>
        /// Run an array of commands, one after the other, then finally run OnComplete when process completes
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="onComplete"></param>
        public static void RunCommands(string[] commands, Action<CommandOutput[]> onComplete)
        {
            CommandOutput[] outputs = new CommandOutput[commands.Length];

            //TODO: This super hacky loop relies on the fact that RunCommand is NOT async!!!
            //BAD
            for (int i =0;i<commands.Length;i++)
            {
                RunCommand(commands[i], (output) => { outputs[i] = output; });
            }

            onComplete(outputs);
        }

        /// <summary>
        /// Run a git command, running onComplete when process completes.
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

            RunCommandInternal(command, 
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
                    if (!string.IsNullOrEmpty(error))
                    {
                        //Special case: Stop line endings error
                        if (error.Contains("LF will be replaced by CRLF"))
                            return;

                        if (error.Contains("The file will have its original line endings in your working directory"))
                            return;
                        

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
        /// Asynchronously run a git command, with streaming onOutput and onError events as they happen, 
        /// and then running onComplete when the process completes
        /// </summary>
        /// <param name="command"></param>
        /// <param name="onComplete"></param>
        /// <param name="onError"></param>
        public static void RunCommandInternal(string command, Action<string> onOutput, Action<string> onError, Action onComplete)
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
            gitProcess.EnableRaisingEvents = true;
            
            //Hook events
            gitProcess.OutputDataReceived += (sender, args) => onOutput(args.Data);
            gitProcess.ErrorDataReceived += (sender, args) => onError(args.Data);

            //Run process
            bool started = gitProcess.Start();

            if (!started)
            {
                onError("Process for command " + command + " failed");
                GitEvents.TriggerOnGitCommandComplete();
                return;
            }

            GitEvents.TriggerOnGitCommandStart();
            gitProcess.BeginErrorReadLine();
            gitProcess.BeginOutputReadLine();
            
            gitProcess.WaitForExit();
            gitProcess.Close();
            GitEvents.TriggerOnGitCommandComplete();
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