using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{
    [Tab(index=10)]
    public class ChangesTab : Tab
    {
        private FileViewer unstagedFileViewer;
        private FileViewer stagedFileViewer;

        private List<GitFile> unstagedFiles;
        private List<GitFile> stagedFiles;

        private List<ContextOption<string>> stagedContextOptions;
        private List<ContextOption<string>> unstagedContextOptions;

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

        public override void Render(GitGudWindow window)
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

        public override void Refresh()
        {
            Scan();
        }

        private void RenderUnstageButtons()
        {
            bool noneSelected = (stagedFileViewer.GetSelectedPaths().Count == 0);
            bool noFiles = (stagedFiles.Count == 0);

            EditorGUILayout.BeginHorizontal();

            //UNSTAGE SELECTED
            EditorGUI.BeginDisabledGroup(noneSelected);

            if (GUILayout.Button("Unstage Selected", GUILayout.ExpandWidth(false)))
            {
                GitCore.UnstagePaths(stagedFileViewer.GetSelectedPaths(), null);
                Scan();
            }

            EditorGUI.EndDisabledGroup();

            //UNSTAGE ALL
            EditorGUI.BeginDisabledGroup(noFiles);

            if (GUILayout.Button("Unstage All", GUILayout.ExpandWidth(false)))
            {
                GitCore.UnstagePaths(GitFile.GetPaths(stagedFiles), null);
                Scan();
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
        }

        private void RenderStageButtons()
        {
            bool noneSelected = (unstagedFileViewer.GetSelectedPaths().Count == 0);
            bool noFiles = (unstagedFiles.Count == 0);

            EditorGUILayout.BeginHorizontal();

            //STAGE SELECTED
            EditorGUI.BeginDisabledGroup(noneSelected);

            if (GUILayout.Button("Stage Selected", GUILayout.ExpandWidth(false)))
            {
                GitCore.StagePaths(unstagedFileViewer.GetSelectedPaths(), null);
                Scan();
            }

            EditorGUI.EndDisabledGroup();

            //STAGE ALL
            EditorGUI.BeginDisabledGroup(noFiles);

            if (GUILayout.Button("Stage All", GUILayout.ExpandWidth(false)))
            {
                GitCore.StagePaths(GitFile.GetPaths(unstagedFiles), null);
                Scan();
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
        }

        private void RenderCommitSection()
        {
            commitText = EditorGUILayout.TextArea(commitText,GUILayout.MinHeight(50));

            EditorGUILayout.BeginHorizontal();

            bool stageEmpty = (stagedFiles.Count == 0);

            //Do not allow committing if there is nothing to stage
            EditorGUI.BeginDisabledGroup(stageEmpty);

            if (GUILayout.Button("Commit", GUILayout.ExpandWidth(false)))
                Commit();

            autoPush = EditorGUILayout.Toggle("Push changes automatically", autoPush);

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();

        }

        //Scan attributes for context lists
        private void BuildContextLists()
        {
            stagedContextOptions = new List<ContextOption<string>>();
            unstagedContextOptions = new List<ContextOption<string>>();

            GitUtility.ForEachTypeWithOrdered<PathContextAttribute>(true, (type, attribute) =>
            {
                if ((attribute.mode & FilePathMode.Staged) == FilePathMode.Staged)
                    stagedContextOptions.Add((ContextOption<string>)Activator.CreateInstance(type));

                if ((attribute.mode & FilePathMode.Unstaged) == FilePathMode.Unstaged)
                    unstagedContextOptions.Add((ContextOption<string>)Activator.CreateInstance(type));
            });

        }

        private void Commit()
        {
            //Commit!
            GitCore.Commit(commitText, (output) =>
            {
                //Error
                if (output.errorData != null)
                {
                    Debug.LogError(output.errorData);
                    return;
                }

                //Auto push
                if (autoPush)
                    AutoPush();
                else
                    //Trigger a refresh
                    GitGudWindow.PlanRefresh();
            });
        }

        private void AutoPush()
        {
            GitCore.Push(true, (output) =>
            {
                //Trigger a refresh
                GitGudWindow.PlanRefresh();

                //Error
                if (output.errorData != null)
                {
                    Debug.LogError(output.errorData);
                    return;
                }

            });
        }

        /// <summary>
        /// Scans the status of the local repo, and renders it
        /// </summary>
        private void Scan()
        {
            GitCore.Status(true,
                (commandOutput) =>
                {
                    //Error catch
                    if (commandOutput.errorData != null)
                    {
                        Debug.LogError(commandOutput.errorData);
                        error = true;
                        errorString = commandOutput.errorData;
                        return;
                    }

                    error = false;
                    BuildStageLists(commandOutput.outputData);

                });
        }

        /// <summary>
        /// Converts status string into two file lists - staged and unstaged
        /// TODO: Make this function generic, instead of only working for this specific use
        /// </summary>
        private void BuildStageLists(string status)
        {
            stagedFiles = new List<GitFile>();
            unstagedFiles = new List<GitFile>();

            if (status == null)
                return;

            //Remove any pesky quotes
            status = status.Replace("\"", "");

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
                    stagedFiles.Add(new GitFile(filepathA, stagedStatus));
                }

                //Unstage
                if (unstagedStatus != FileStatus.unmodified &&
                    unstagedStatus != FileStatus.ignored)
                {
                    unstagedFiles.Add(new GitFile(filepathB, unstagedStatus));
                }
            }
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