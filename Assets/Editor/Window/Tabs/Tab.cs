using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    public class Tab
    {
        public virtual string GetName()
        {
            return "Tab";
        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnDisable()
        {

        }

        public virtual void Render(GitGudWindow window)
        {

        }

        //Used to reset the page, such as rescanning data
        public virtual void Refresh()
        {

        }

    }

}