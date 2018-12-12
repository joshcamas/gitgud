using System;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    //Attribute that hooks onto the top buttons in the git gud window
    [AttributeUsage(AttributeTargets.Class)]
    public class TopButtonAttribute : Attribute, IOrderedAttribute
    {
        public int index;
        public bool flexibleSpaceAfter = false;

        public int GetIndex()
        {
            return index;
        }
    }

}