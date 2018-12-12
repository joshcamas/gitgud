using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    //A simple fileviewmode that displays a tree of files and directories, 
    //does nothing right now
    public class TreeFileViewMode : FileViewMode
    {
        private Vector2 scrollPosition;

        public override void Render(List<GitFile> files)
        {
            EditorGUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.BeginVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
        }
    }

}