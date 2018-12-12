using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{
    public class TopButton
    {
        public bool flexibleSpaceAfter = false;

        public virtual void OnCreate()
        {

        }

        public virtual void Refresh()
        {

        }

        public virtual string GetName()
        {
            return "Top Button";
        }

        public virtual bool IsDisabled()
        {
            return false;
        }

        //Run when button is clicked
        public virtual void OnClick(GitGudWindow window)
        {

        }

        //Default button rendering
        public virtual void Render(GitGudWindow window)
        {
            EditorGUI.BeginDisabledGroup(IsDisabled());

            bool clicked = GUILayout.Button(GetName(), "ToolbarButton", GUILayout.ExpandWidth(false));

            EditorGUI.EndDisabledGroup();

            if (clicked)
                OnClick(window);

            if (flexibleSpaceAfter)
                GUILayout.FlexibleSpace();
        }
    }

}