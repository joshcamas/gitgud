using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GitGud
{
    //Core functions
    public class GitCore
    {
        public static void DiscardFiles(List<string> paths, Action<CommandOutput> onComplete)
        {
            string combinedPaths = GitUtility.QuoteAndCombinePaths(paths.ToArray());

            GitGud.RunCommand("checkout " + combinedPaths, onComplete);

        }

        public static void DiscardFile(string path,Action<CommandOutput> onComplete)
        {
            GitGud.RunCommand("checkout -- " + path, onComplete);
        }
        
        public static void DiscardAll(Action<CommandOutput> onComplete)
        {
            GitGud.RunCommand("checkout .", onComplete);
        }

        public static void AddFile(string filename, Action<CommandOutput> onComplete)
        {
            GitGud.RunCommand("add " + filename, onComplete);
        }

        public static void CheckoutCommit(Commit commit, Action<CommandOutput> onComplete)
        {
            GitGud.RunCommand("checkout " + commit.hash + " .", onComplete);

        }

        public static void Status(bool shortMode, Action<CommandOutput> onComplete)
        {
            if (shortMode)
                GitGud.RunCommand("status -s --untracked-files=all", onComplete);
            else
                GitGud.RunCommand("status", onComplete);
        }

        //Log with a internal format, which returns a list of commit objects
        public static void Log(string filter, Action<CommandOutput,List<Commit>> onComplete)
        {
            Action<CommandOutput> onLogComplete = (output) =>
            {
                //Error catching
                if (output.errorData != null)
                {
                    onComplete(output, null);
                    return;
                }

                List<Commit> commits = new List<Commit>();

                if (output.outputData == null)
                {
                    onComplete(output, commits);
                    return;
                }

                //Convert log string into commit format
                string[] commitStrings = output.outputData.Split('\n');

                foreach(string commitStr in commitStrings)
                {
                    string[] components = commitStr.Split(',');
                    Commit commit = new Commit();
                    commit.hash = components[0];
                    commit.tree = components[1];
                    commit.parent = components[2];
                    commit.author_name = components[3];
                    commit.author_email = components[4];
                    commit.date = components[5];
                    commit.subject = components[6];

                    commits.Add(commit);
                }

                onComplete(output, commits);
            };


            string format = "%H,%T,%P,%an,%ae,%ad,'%s";

            if (filter == "" || filter == null)
                GitGud.RunCommand("log --pretty=\"" + format + "\"", onLogComplete);
            else
                GitGud.RunCommand("log --pretty=\"" + format + "\" " + filter, onLogComplete);
        }

        //Log with a custom format, returns raw output string
        public static void LogFormat(string format,string filter, Action<CommandOutput, string> onComplete)
        {
            Action<CommandOutput> onLogComplete = (output) =>
            {
                onComplete(output, output.outputData);
            };

            if (filter == "" || filter == null)
                GitGud.RunCommand("log --pretty=\"" + format + "\"", onLogComplete);
            else
                GitGud.RunCommand("log --pretty=\"" + format + "\" " + filter, onLogComplete);
        }

        //Returns list of stashes, in order by index
        //Returned stash is in form "branch:message"
        public static void ListStash(Action<CommandOutput, List<string>> onComplete)
        {
            GitGud.RunCommand("stash list", (output) =>
            {
                //Error catching
                if (output.errorData != null)
                {
                    onComplete(output, null);
                    return;
                }

                //Split lines
                List<string> stashes = new List<string>();
                foreach (string line in output.outputData.Split('\n'))
                {
                    //Each line is in form "@stash{index}: on branch: message"
                    string[] parts = line.Split(':');
                    string branch = parts[1].Replace("On ", "");
                    string message = "";
                    if (parts.Length > 2)
                        message = parts[2];

                    stashes.Add(branch + ":" + message);
                }

                onComplete(output, stashes);
            });
        }

        //Create a new stash
        public static void PushStash(string message, Action<CommandOutput> onComplete)
        {
            if (message == "" || message == null)
                GitGud.RunCommand("stash push", onComplete);
            else
                GitGud.RunCommand("stash push --message=\"" + message + "\"", onComplete);
        }

        //Simple push command
        public static void Push(bool all, Action<CommandOutput> onComplete)
        {
            if (all)
                GitGud.RunCommand("push --all", onComplete);
            else
                GitGud.RunCommand("push", onComplete);
        }

        //Simple pull command
        public static void Pull(Action<CommandOutput> onComplete)
        {
            GitGud.RunCommand("pull", onComplete);
        }

        //Simple fetch command
        public static void Fetch(Action<CommandOutput> onComplete)
        {
            GitGud.RunCommand("fetch", onComplete);
        }

        public static void Commit(string message, Action<CommandOutput> onComplete)
        {
            if (message == "")
                GitGud.RunCommand("commit", onComplete);
            else
                GitGud.RunCommand("commit --message=\"" + message + "\"", onComplete);
        }
        
        //Stage a list of paths
        public static void StagePaths(List<string> paths, Action<CommandOutput> onComplete)
        {
            string combinedPaths = GitUtility.QuoteAndCombinePaths(paths.ToArray());

            GitGud.RunCommand("add " + combinedPaths, onComplete);
        }

        //Unstage a list of paths
        public static void UnstagePaths(List<string> paths, Action<CommandOutput> onComplete)
        {
            string combinedPaths = GitUtility.QuoteAndCombinePaths(paths.ToArray());

            GitGud.RunCommand("reset -- " + combinedPaths, onComplete);
        }

        //Stage a single path
        public static void StagePath(string path, Action<CommandOutput> onComplete)
        {
            GitGud.RunCommand("add \"" + path + "\"", onComplete);
        }

        //Unstage a single path
        public static void UnstagePath(string path, Action<CommandOutput> onComplete)
        {
            GitGud.RunCommand("reset -- \"" + path + "\"", onComplete);
        }


    }
}