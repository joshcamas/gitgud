using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{

    public class BranchWindow : EditorWindow
    {
        string currentBranchName = "...";
        string newBranchName;
        Action onClose;

        //Window creation
        public static BranchWindow ShowWindow(Action onClose)
        {
            //Show existing window instance. If one doesn't exist, make one.
            BranchWindow window = (BranchWindow)BranchWindow.GetWindow(typeof(BranchWindow), true, "Stash");
            window.onClose = onClose;

            return window;
        }

        private void OnEnable()
        {
            GitCore.CurrentBranch((branch) =>
            {
                currentBranchName = branch;
                Repaint();
            });
        }

        private void OnGUI()
        {
            //Auto close window of deserialization cleared action
            if (onClose == null)
            {
                Close();
                return;
            }

            bool isClosing = false;
            EditorGUILayout.LabelField("Create a new branch",EditorStyles.boldLabel);

            EditorGUILayout.LabelField("Current Branch: " + currentBranchName);

            newBranchName = EditorGUILayout.TextField("New Branch: ",newBranchName);

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Create Branch", GUILayout.ExpandWidth(false)))
            {
                CreateBranch();
                isClosing = true;
            }

            if (GUILayout.Button("Cancel", GUILayout.ExpandWidth(false)))
            {
                isClosing = true;
            }

            EditorGUILayout.EndHorizontal();

            if (isClosing)
                Close();
        }

        private void CreateBranch()
        {
            GitCore.CreateBranch(newBranchName,(output) =>
            {
                if(output.errorData != null)
                {
                    Debug.LogError("Could not create branch: " + output.errorData);
                } else
                {
                    Debug.Log("Successfully create branch " + newBranchName);
                }
            });
        }
    }
}