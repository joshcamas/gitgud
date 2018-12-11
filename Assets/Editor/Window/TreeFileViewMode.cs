using System;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    //A simple fileviewmode that displays a tree of files and directories, 
    //does nothing right now
    public class TreeFileViewMode : FileViewMode
    {
        private Vector2 scrollPosition;

        public override void Render(GitFile[] files)
        {
            EditorGUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.BeginVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
        }
    }

}