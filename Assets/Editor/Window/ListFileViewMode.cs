using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    //A simple fileviewmode that displays a list of files
    public class ListFileViewMode : FileViewMode
    {
        private Vector2 scrollPosition;
        private List<GitFile> selectedFiles;

        public override void Init(List<ContextOption<string>> contextOptions)
        {
            base.Init(contextOptions);
            selectedFiles = new List<GitFile>();
        }

        public override List<string> GetSelectedPaths()
        {
            if (selectedFiles == null)
                return new List<string>();

            return GitFile.GetPaths(selectedFiles);
        }

        public override void Render(List<GitFile> files)
        {
            if (files == null)
                return;

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);

            float statusSize = 20;
            
            //Render list of files
            selectedFiles = SelectableListGUI.RenderList<GitFile>(files, selectedFiles, (file, selected) =>
            {
                 //Render
                 EditorGUILayout.BeginHorizontal();

                 GUIStyle style = new GUIStyle("Label");

                 if (selected)
                     style.normal.textColor = Color.blue;

                 //Status Icon
                 if (fileStatusIcons.ContainsKey(file.status))
                     GUILayout.Label(fileStatusIcons[file.status], GUILayout.Width(statusSize), GUILayout.Height(statusSize));
                 else
                     GUILayout.Label(file.status.ToString(), GUILayout.Width(statusSize), GUILayout.Height(statusSize));

                 //Button
                 bool pressed = GUILayout.Button(file.path, style, GUILayout.Height(statusSize));

                 EditorGUILayout.EndHorizontal();
                 return pressed;
             },
            (selectedFiles, mouse) =>
            {
                //Context click
                if(mouse == 1)
                {
                    ContextOption<string>.ShowContextMenu(GitFile.GetPaths(selectedFiles),contextOptions);
                }
            });

            EditorGUILayout.EndScrollView();
        }
    }

}