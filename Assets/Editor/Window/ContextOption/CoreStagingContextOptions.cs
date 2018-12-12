using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace GitGud.UI
{
    [PathContext(index=10,mode = FilePathMode.Staged | FilePathMode.Unstaged)]
    public class ShowInExplorerContextOption : ContextOption<string>
    {
        //Path of context menu
        public override string GetContextPath()
        {
            return "Show In Explorer";
        }

        //Function run when file context option is clicked
        public override void OnSelect(List<string> paths)
        {
            string absolutePath = GitUtility.RepoPathToAbsolutePath(paths[0]);

            EditorUtility.RevealInFinder(absolutePath);
        }

        //Function to check if this context option is disabled for string path
        public override bool IsDisabled(List<string> paths)
        {
            //Only allow one file to be selected
            if (paths.Count != 1)
                return true;

            string absolutePath = GitUtility.RepoPathToAbsolutePath(paths[0]);
            Debug.Log(absolutePath);

            //Disable if file does not exist (ie moved or deleted)
            if (!File.Exists(absolutePath))
                return true;

            return false;
        }
    }

    [PathContext(index = 20, mode = FilePathMode.Staged | FilePathMode.Unstaged)]
    public class SelectInProjectContextOption : ContextOption<string>
    {
        //Path of context menu
        public override string GetContextPath()
        {
            return "Select in Project Window";
        }

        //Function run when file context option is clicked
        public override void OnSelect(List<string> paths)
        {
            string assetPath = GitUtility.RepoPathToRelativeAssetPath(paths[0]);

            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(assetPath);
        }

        //Function to check if this context option is disabled for string path
        public override bool IsDisabled(List<string> paths)
        {
            //Only allow one file to be selected
            if (paths.Count != 1)
                return true;

            //If file is not asset, disable
            if (GitUtility.RepoPathToRelativeAssetPath(paths[0]) == null)
                return true;

            return false;
        }
    }

}