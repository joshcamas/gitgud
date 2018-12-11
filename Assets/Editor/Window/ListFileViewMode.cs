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

        public override void Init(List<PathContextOption> contextOptions)
        {
            base.Init(contextOptions);
            selectedFiles = new List<GitFile>();
        }

        public override string[] GetSelectedPaths()
        {
            if (selectedFiles == null)
                return new string[0];

            return GitFile.GetPaths(selectedFiles);
        }

        public override void Render(GitFile[] files)
        {
            if (files == null)
                return;

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.BeginVertical();

            //Remove any missing files from selected files
            List<GitFile> filesToDeselect = new List<GitFile>();
            List<GitFile> allFiles = new List<GitFile>(files);
            foreach(GitFile f in selectedFiles)
            {
                if (!allFiles.Contains(f))
                    filesToDeselect.Add(f);
            }

            foreach(GitFile f in filesToDeselect)
            {
                selectedFiles.Remove(f);
            }

            //Render files
            foreach (GitFile f in files)
            {
                EditorGUILayout.BeginHorizontal();

                GUIStyle style = new GUIStyle("Label");

                if(selectedFiles.Contains(f))
                    style.normal.textColor = Color.blue;

                float statusSize = 20;

                //Render label, or just the status text
                if (fileStatusIcons.ContainsKey(f.status))
                    GUILayout.Label(fileStatusIcons[f.status], GUILayout.Width(statusSize), GUILayout.Height(statusSize));
                else
                    GUILayout.Label(f.status.ToString(), GUILayout.Width(statusSize), GUILayout.Height(statusSize));

                bool pressed = GUILayout.Button(f.path, style);

                if (pressed)
                {
                    //Selection
                    if (Event.current.button == 0)
                    {
                        //Select multiple - in between (shift) mode
                        if(Event.current.shift && selectedFiles.Count != 0)
                        {
                            int startIndex = allFiles.IndexOf(selectedFiles[selectedFiles.Count - 1]);
                            int stopIndex = allFiles.IndexOf(f);

                            //TODO: Replace double for loop with a multidirectional one somehow
                            if(startIndex < stopIndex)
                            {
                                for(int i= startIndex + 1; i <= stopIndex; i++)
                                {
                                    //Toggle file
                                    if (selectedFiles.Contains(allFiles[i]))
                                        selectedFiles.Remove(allFiles[i]);
                                    else
                                        selectedFiles.Add(allFiles[i]);
                                }
                            }
                            else if (startIndex > stopIndex)
                            {
                                for (int i = stopIndex; i < startIndex; i++)
                                {
                                    //Toggle file
                                    if (selectedFiles.Contains(allFiles[i]))
                                        selectedFiles.Remove(allFiles[i]);
                                    else
                                        selectedFiles.Add(allFiles[i]);
                                }
                            }
                            
                        } else if(Event.current.control)
                        {
                            //Toggle Selected
                            if (selectedFiles.Contains(f))
                                selectedFiles.Remove(f);
                            else
                                selectedFiles.Add(f);
                        } else 
                        {
                            //Select file
                            selectedFiles = new List<GitFile>() { f };
                        }

                    }
                    //Context
                    else if (Event.current.button == 1)
                    {
                        ShowContextMenu(GitFile.GetPaths(selectedFiles));
                    }

                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
        }
    }

}