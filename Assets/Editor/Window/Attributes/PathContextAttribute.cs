using System;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    //Attribute that hooks onto path context menus
    [AttributeUsage(AttributeTargets.Class)]
    public class PathContextAttribute : Attribute, IOrderedAttribute
    {
        public FilePathMode mode;
        public int index;

        public int GetIndex()
        {
            return index;
        }
    }

}