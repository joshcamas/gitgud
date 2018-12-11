using System;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    public class PathContextOption
    {
        //Path of context menu
        public virtual string GetContextPath()
        {
            return null;
        }

        //Function run when file context option is clicked
        public virtual void OnSelect(string[] paths)
        {

        }

        //Function to check if this context option is disabled for string path
        public virtual bool IsDisabled(string[] path)
        {
            return false;
        }
    }

}