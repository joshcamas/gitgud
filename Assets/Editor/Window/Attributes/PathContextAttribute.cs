using System;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    //Attribute that hooks onto path context menus
    [AttributeUsage(AttributeTargets.Class)]
    public class PathContextAttribute : Attribute
    {
        public FilePathModes mode;

        public PathContextAttribute(FilePathModes mode)
        {
            this.mode = mode;
        }
    }

}