using System;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StashContextAttribute : Attribute, IOrderedAttribute
    {
        public int index;

        public int GetIndex()
        {
            return index;
        }
    }

}