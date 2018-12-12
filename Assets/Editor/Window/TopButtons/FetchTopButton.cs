using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{
    [TopButton(index = 40, flexibleSpaceAfter = true)]
    public class FetchTopButton : TopButton
    {
        public override string GetName()
        {
            return "Fetch";
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