using System;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    //Attribute that hooks onto the window
    [AttributeUsage(AttributeTargets.Class)]
    public class TabAttribute : Attribute, IOrderedAttribute
    {
        public int index = -1;

        public int GetIndex()
        {
            return index;
        }
    }

}