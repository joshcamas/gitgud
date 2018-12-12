using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{
    [TopButton(index = 30)]
    public class PushTopButton : TopButton
    {
        public override string GetName()
        {
            return "Push";
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