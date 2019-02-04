using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{
    [TopButton(index = 60,flexibleSpaceAfter = true)]
    public class BranchTopButton : TopButton
    {
        public override string GetName()
        {
            return "Branch";
        }

        public override bool IsDisabled()
        {
            //Always disabled for now
            return false;
        }

        //Run when button is clicked
        public override void OnClick(GitGudWindow window)
        {
            BranchWindow.ShowWindow(GitEvents.TriggerOnLocalChange);
        }

    }

}