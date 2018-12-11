using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{
    [Tab(0)]
    public class ChangesTab : Tab
    {
        private FileViewer unstagedFileViewer;
        private FileViewer stagedFileViewer;

        private GitFile[] unstagedFiles;
        private GitFile[] stagedFiles;

        private List<PathContextOption> stagedContextOptions;
        private List<PathContextOption> unstagedContextOptions;

        private string commitText;
        private bool autoPush = true;

        private bool error = false;
        private string errorString;


        public override string GetName()
        {
            return "Changes";
        }

        public override void OnEnable()
        {
            BuildContextLists();

            stagedFileViewer = new FileViewer();
            stagedFileViewer.Init("Staged", stagedContextOptions);

            unstagedFileViewer = new FileViewer();
            unstagedFileViewer.Init("Unstaged", unstagedContextOptions);

            Scan();

        }

        public override void Render(EditorWindow window)
        {
            //Error render
            if(error)
            {
                EditorGUILayout.LabelField("Something went wrong, and we have no smart error catching :^)");
                EditorGUILayout.LabelField(errorString);
                return;
            }

            EditorGUILayout.LabelField("Local Changes", EditorStyles.largeLabel);

            RenderStageButtons();
            unstagedFileViewer.Render(unstagedFiles);

            RenderUnstageButtons();
            stagedFileViewer.Render(stagedFiles);

            /*
            if (GUILayout.Button("Scan", GUILayout.ExpandWidth(false)))
                Scan();
            */

            RenderCommitSection();
        }


        private void RenderUnstageButtons()
        {
            bool noneSelected = (stagedFileViewer.GetSelectedPaths().Length == 0);
            bool noFiles = (stagedFiles.Length == 0);

            EditorGUILayout.BeginHorizontal();

            //UNSTAGE SELECTED
            EditorGUI.BeginDisabledGroup(noneSelected);

            if (GUILayout.Button("Unstage Selected", GUILayout.ExpandWidth(false)))
            {
                GitUtility.UnstagePaths(stagedFileViewer.GetSelectedPaths(), null);
                Scan();
            }

            EditorGUI.EndDisabledGroup();

            //UNSTAGE ALL
            EditorGUI.BeginDisabledGroup(noFiles);

            if (GUILayout.Button("Unstage All", GUILayout.ExpandWidth(false)))
            {
                GitUtility.UnstagePaths(GitFile.GetPaths(stagedFiles), null);
                Scan();
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
        }

        private void RenderStageButtons()
        {
            bool noneSelected = (unstagedFileViewer.GetSelectedPaths().Length == 0);
            bool noFiles = (unstagedFiles.Length == 0);

            EditorGUILayout.BeginHorizontal();

            //STAGE SELECTED
            EditorGUI.BeginDisabledGroup(noneSelected);

            if (GUILayout.Button("Stage Selected", GUILayout.ExpandWidth(false)))
            {
                GitUtility.StagePaths(unstagedFileViewer.GetSelectedPaths(), null);
                Scan();
            }

            EditorGUI.EndDisabledGroup();

            //STAGE ALL
            EditorGUI.BeginDisabledGroup(noFiles);

            if (GUILayout.Button("Stage All", GUILayout.ExpandWidth(false)))
            {
                GitUtility.StagePaths(GitFile.GetPaths(unstagedFiles), null);
                Scan();
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
        }

        private void RenderCommitSection()
        {
            

            commitText = EditorGUILayout.TextArea(commitText,GUILayout.MinHeight(50));

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Commit", GUILayout.ExpandWidth(false)))
            {

            }

            autoPush = EditorGUILayout.Toggle("Push changes automatically", autoPush);

            EditorGUILayout.EndHorizontal();

        }

        //Scan attributes for context lists
        private void BuildContextLists()
        {
            stagedContextOptions = new List<PathContextOption>();
            unstagedContextOptions = new List<PathContextOption>();

            GitUtility.ForEachTypeWith<PathContextAttribute>(true, (type, attribute) =>
            {
                if ((attribute.mode & FilePathModes.Staged) == FilePathModes.Staged)
                    stagedContextOptions.Add((PathContextOption)Activator.CreateInstance(type));

                if ((attribute.mode & FilePathModes.Unstaged) == FilePathModes.Unstaged)
                    unstagedContextOptions.Add((PathContextOption)Activator.CreateInstance(type));

            });
        }

        /// <summary>
        /// Scans the status of the local repo, and renders it
        /// </summary>
        private void Scan()
        {
            GitUtility.Status(true,
                (commandOutput) =>
                {
                    //Error catch
                    if (commandOutput.outputData == null)
                    {
                        if (commandOutput.errorData != null)
                        {
                            Debug.LogError(commandOutput.errorData);
                            error = true;
                            errorString = commandOutput.errorData;
                        } else
                        {
                            error = true;
                            errorString = "Scan failed";
                        }
                    }
                    else
                    {
                        error = false;
                        BuildStageLists(commandOutput.outputData);
                    }

                });
        }

        /// <summary>
        /// Converts status string into two file lists - staged and unstaged
        /// TODO: Make this function generic, instead of only working for this specific use
        /// </summary>
        private void BuildStageLists(string status)
        {
            //Convert string status into readable file names and statuses
            List<GitFile> unstagedFileList = new List<GitFile>();
            List<GitFile> stagedFileList = new List<GitFile>();

            //Read status code, more info here:
            //https://www.git-scm.com/docs/git-status/1.8.5

            foreach (string s in status.Split('\n'))
            {
                //Skip any non important lines
                if (s.Length < 3)
                    continue;

                string filestatus = s.Substring(0, 2);
                string filepathA = s.Substring(3, s.Length - 3);
                string filepathB = filepathA;

                //Detect separate paths
                if(filepathA.Contains("->"))
                {
                    string[] split = filepathA.Split(new string[] { " -> " }, System.StringSplitOptions.None);
                    filepathA = split[0];
                    filepathB = split[1];
                }

                FileStatus stagedStatus = GitFile.StatusCodeToFileStatus(filestatus[0]);
                FileStatus unstagedStatus = GitFile.StatusCodeToFileStatus(filestatus[1]);

                //Stage
                if (stagedStatus != FileStatus.unmodified &&
                    stagedStatus != FileStatus.ignored &&
                    stagedStatus != FileStatus.untracked)
                {
                    stagedFileList.Add(new GitFile(filepathA, stagedStatus));
                }

                //Unstage
                if (unstagedStatus != FileStatus.unmodified &&
                    unstagedStatus != FileStatus.ignored)
                {
                    unstagedFileList.Add(new GitFile(filepathB, unstagedStatus));
                }


            }

            unstagedFiles = unstagedFileList.ToArray();
            stagedFiles = stagedFileList.ToArray();
        }


        /// <summary>
        /// Window error hook
        /// </summary>
        /// <param name="error"></param>
        private void OnError(string error)
        {
            if (error == null)
                return;

            Debug.LogError(error);
        }
    }

}