using System;
using System.Linq;
using System.Diagnostics;
using UnityEngine;

namespace GitGud
{
    public class GitUtility
    {
        /// <summary>
        /// Returns 0 for success, 1 for incorrect / missing git executable, and 2 for incorrect / missing git repo
        /// </summary>
        /// <returns></returns>
        public static int GitState()
        {
            return 0;
        }

        public static void AddFile(string filename, Action<CommandOutput> onComplete)
        {
            GitGudtCore.RunCommand("add " + filename, onComplete);
        }

        public static void Status(bool shortMode,Action<CommandOutput> onComplete)
        {
            if(shortMode)
                GitGudtCore.RunCommand("status -s --untracked-files=all", onComplete);
            else
                GitGudtCore.RunCommand("status", onComplete);
        }

        //Stage a list of paths
        public static void StagePaths(string[] paths, Action<CommandOutput> onComplete)
        {
            if (paths.Length == 0)
            {
                //TODO: commandoutput is never outputted correctly here
                if(onComplete != null)
                    onComplete(new CommandOutput());
                return;
            }
                
            StagePath(paths[0], (output) =>
            {
                StagePaths(paths.Skip(1).ToArray(), onComplete);
            });
        }

        //Unstage a list of paths
        public static void UnstagePaths(string[] paths, Action<CommandOutput> onComplete)
        {
            if (paths.Length == 0)
            {
                //TODO: commandoutput is never outputted correctly here
                if (onComplete != null)
                    onComplete(new CommandOutput());
                return;
            }

            UnstagePath(paths[0], (output) =>
            {
                UnstagePaths(paths.Skip(1).ToArray(), onComplete);
            });
        }

        //Stage a single path
        public static void StagePath(string path, Action<CommandOutput> onComplete)
        {
            GitGudtCore.RunCommand("add " + path, onComplete);
        }

        //Unstage a single path
        public static void UnstagePath(string path, Action<CommandOutput> onComplete)
        {
            GitGudtCore.RunCommand("reset -- " + path, onComplete);
        }

        //Converts a path relative to the repo to an absolute path
        public static string RepoPathToAbsolutePath(string repoPath)
        {
            return System.IO.Path.Combine(GitGudSettings.GetString("repo_path"), repoPath);
        }

        //Converts a path relative to the repo to a path relative to the unity assets folder
        public static string RepoPathToRelativeAssetPath(string repoPath)
        {
            string absolutePath = RepoPathToAbsolutePath(repoPath);

            //Make sure repopath is actually inside the asset path
            if (Application.dataPath.Length > absolutePath.Length)
                return null;

            return "Assets" + absolutePath.Substring(Application.dataPath.Length);
        }

    }
}