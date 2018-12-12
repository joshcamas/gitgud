using System;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    //Attribute that hooks onto commit context menus
    [AttributeUsage(AttributeTargets.Class)]
    public class CommitContextAttribute : Attribute, IOrderedAttribute
    {
        public int index = -1;

        public int GetIndex()
        {
            return index;
        }
    }

}