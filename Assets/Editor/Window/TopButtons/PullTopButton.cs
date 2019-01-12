using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{
    [TopButton(index = 20,flexibleSpaceAfter = true)]
    public class PullTopButton : TopButton
    {
        public override string GetName()
        {
            return "Pull";
        }

        public override bool IsDisabled()
        {
            //Always disabled for now
            return true;
        }

        //Run when button is clicked
        public override void OnClick(GitGudWindow window)
        {

        }

    }

}