using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace GitGud.UI
{
    [StashContext(index = 10)]
    public class ApplyStashContextOption : ContextOption<string>
    {
        //Path of context menu
        public override string GetContextPath()
        {
            return "Apply Stash on Local Copy";
        }

        //Function to check if this context option is disabled for string path
        public override bool IsDisabled(List<string> commits)
        {
            return false;
        }

        public override void OnSelect(List<string> commits)
        {

        }
    }

    [StashContext(index = 20)]
    public class DeleteStashContextOption : ContextOption<string>
    {
        //Path of context menu
        public override string GetContextPath()
        {
            return "Delete Stash";
        }

        //Function to check if this context option is disabled for string path
        public override bool IsDisabled(List<string> commits)
        {
            return false;
        }

        public override void OnSelect(List<string> commits)
        {

        }
    }

}