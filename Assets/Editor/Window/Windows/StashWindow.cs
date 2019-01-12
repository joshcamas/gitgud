using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{

    public class StashWindow : EditorWindow
    {
        string stashComment;
        Action<string> onClose;

        //Window creation
        public static StashWindow ShowWindow(Action<string> onClose)
        {
            //Show existing window instance. If one doesn't exist, make one.
            StashWindow window = (StashWindow)StashWindow.GetWindow(typeof(StashWindow), true, "Stash");
            window.onClose = onClose;

            return window;
        }

        public void OnGUI()
        {
            //Auto close window of deserialization cleared action
            if (onClose == null)
            {
                Close();
                return;
            }

            bool isClosing = false;
            EditorGUILayout.LabelField("This will save your current changes and return your working copy to a clean state. If you wish, you may enter a message to describe this stash:");

            stashComment = EditorGUILayout.TextField(stashComment);

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Ok",GUILayout.ExpandWidth(false)))
            {
                onClose(stashComment);
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
    }
}