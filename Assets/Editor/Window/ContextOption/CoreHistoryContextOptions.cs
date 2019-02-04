using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace GitGud.UI
{
    [CommitContext(index = 10)]
    public class CheckoutContextOption : ContextOption<Commit>
    {
        //Path of context menu
        public override string GetContextPath()
        {
            return "Checkout...";
        }

        //Function to check if this context option is disabled for string path
        public override bool IsDisabled(List<Commit> commits)
        {
            //Only allow one file to be selected
            if (commits.Count != 1)
                return true;

            return false;
        }

        public override void OnSelect(List<Commit> commits)
        {
            GitCore.CheckoutCommit(commits[0], (output) =>
            {
                GitEvents.TriggerOnLocalChange();
                AssetDatabase.Refresh(ImportAssetOptions.Default);
            });
        }

    }

    [CommitContext(index = 20)]
    public class CheckoutResetContextOption : ContextOption<Commit>
    {
        //Path of context menu
        public override string GetContextPath()
        {
            return "Checkout and Reset...";
        }

        //Function to check if this context option is disabled for string path
        public override bool IsDisabled(List<Commit> commits)
        {
            //Only allow one file to be selected
            if (commits.Count != 1)
                return true;

            return false;
        }

        public override void OnSelect(List<Commit> commits)
        {
            GitCore.CheckoutCommit(commits[0], (output) =>
            {
                GitGud.RunCommands(new string[] { "clean -f -d", "reset --hard" }, (outputs) =>
                {
                    GitEvents.TriggerOnLocalChange();
                    AssetDatabase.Refresh(ImportAssetOptions.Default);
                });
            });
        }

    }

    [CommitContext(index = 15)]
    public class GetInfoContextOption : ContextOption<Commit>
    {
        //Path of context menu
        public override string GetContextPath()
        {
            return "Get Info";
        }

        //Function to check if this context option is disabled for string path
        public override bool IsDisabled(List<Commit> commits)
        {
            //Only allow one file to be selected
            if (commits.Count != 1)
                return true;

            return false;
        }

        public override void OnSelect(List<Commit> commits)
        {
            Debug.Log(commits[0].parent);
            Debug.Log(commits[0].tree);
        }

    }

    [CommitContext(index = 30)]
    public class MergeContextOption : ContextOption<Commit>
    {
        //Path of context menu
        public override string GetContextPath()
        {
            return "Merge...";
        }

        //Function to check if this context option is disabled for string path
        public override bool IsDisabled(List<Commit> commits)
        {
            //Only allow one file to be selected
            if (commits.Count != 1)
                return true;

            return false;
        }
    }

}