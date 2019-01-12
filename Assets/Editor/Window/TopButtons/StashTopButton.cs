using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{
    [TopButton(index = 70)]
    public class StashTopButton : TopButton
    {
        private StashWindow stashWindow;

        public override string GetName()
        {
            return "Stash";
        }

        public override bool IsDisabled()
        {
            if (stashWindow != null)
                return true;

            return false;
        }

        //Run when button is clicked
        public override void OnClick(GitGudWindow window)
        {
            stashWindow = StashWindow.ShowWindow(OnFinishStashWindow);
        }

        private void OnFinishStashWindow(string message)
        {
            GitGudWindow.DisableInput();

            GitCore.PushStash(message, (output) =>
            {
                GitGudWindow.EnableInput();
                GitGudWindow.PlanRefresh();
            });
        }
    }

}