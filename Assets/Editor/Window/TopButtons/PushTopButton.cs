using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{
    [TopButton(index = 30)]
    public class PushTopButton : TopButton
    {
        int pushCount = -1;

        //Scan for pushes
        public override void OnCreate()
        {
            Refresh();
        }

        public override void Refresh()
        {
            GitCore.Log("@{push}..", (output, commits) =>
            {
                if (output.errorData != null)
                    return;

                pushCount = commits.Count;
            });
        }

        public override string GetName()
        {
            if(pushCount < 1)
                return "Push";

            return "Push (" + pushCount + ")";
        }

        public override bool IsDisabled()
        {
            if (pushCount < 1)
                return true;

            return false;
        }

        //Run when button is clicked
        public override void OnClick(GitGudWindow window)
        {
            GitGudWindow.DisableInput();

            GitCore.Push(true, (output) =>
            {
                GitGudWindow.EnableInput();
                GitGudWindow.PlanRefresh();
            });
        }

    }

}