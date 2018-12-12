using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{
    //Renders a list of commits, and manages context menus and such
    public class CommitListGUI
    {
        private Vector2 scrollPosition;
        private List<Commit> selectedCommits;

        public void OnEnable()
        {

        }

        public void Render(List<Commit> commits)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);

            //Render list of files
            selectedCommits = SelectableListGUI.RenderList<Commit>(commits, selectedCommits, RenderCommit,
            (selectedFiles, mouse) =>
            {
                //Context click
                if (mouse == 1)
                {
                   // ShowContextMenu(GitFile.GetPaths(selectedFiles));
                }
            });

            EditorGUILayout.EndScrollView();
        }

        private bool RenderCommit(Commit commit,bool selected)
        {
            EditorGUILayout.BeginHorizontal();

            GUIStyle style = new GUIStyle("Label");

            if (selected)
                style.normal.textColor = Color.blue;

            //Buttons
            bool pressedA = GUILayout.Button(commit.subject, style);
            bool pressedB = GUILayout.Button(commit.date, style);
            bool pressedC = GUILayout.Button(commit.author_name, style);

            EditorGUILayout.EndHorizontal();

            //If at least one button is pressed, return true
            if (pressedA)
                return true;

            if (pressedB)
                return true;

            if (pressedC)
                return true;

            return false;
        }
    }

}