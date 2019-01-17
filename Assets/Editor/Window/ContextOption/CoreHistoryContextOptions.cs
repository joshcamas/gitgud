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