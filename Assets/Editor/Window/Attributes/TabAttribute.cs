using System;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    //Attribute that hooks onto the window
    [AttributeUsage(AttributeTargets.Class)]
    public class TabAttribute : Attribute
    {
        public int index = -1;
        public TabAttribute(int index=-1)
        {
            this.index = index;
        }
    }

}