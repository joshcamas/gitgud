using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace GitGud.UI
{
    [CommitContext(index = 0)]
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
    }


}