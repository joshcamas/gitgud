using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GitGud
{
    public class DebugWindow : EditorWindow
    {
        private string commandString = "";

        private Vector2 statusScroll;
        private string outputString = "";

        //Window
        [MenuItem("GitGud/Debugger")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            DebugWindow.GetWindow(typeof(DebugWindow), false, "Git Debugger");
        }

        private void OnEnable()
        {
            ReadStatus(false);    
        }

        void OnGUI()
        {
            RenderConsole();
            RenderCommandPrompt();
        }

        private void ReadStatus(bool shortMode)
        {
            GitCore.Status(shortMode,(output) =>
            {
                outputString += output.outputData;

                if (output.errorData != null)
                    outputString += "[ERROR] " + output.errorData;
            });
        }

        private void RenderConsole()
        {

            statusScroll = EditorGUILayout.BeginScrollView(statusScroll);

            EditorGUILayout.BeginVertical();

            //Splits output into many text fields, thanks to unity's text limit
            foreach (string line in outputString.Split('\n'))
            {
                EditorGUILayout.LabelField(line);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Status", GUILayout.ExpandWidth(false)))
            {
                ReadStatus(true);
            }

            if (GUILayout.Button("Clear", GUILayout.ExpandWidth(false)))
            {
                outputString = "";
            }

            EditorGUILayout.EndHorizontal();
        }

        private void RenderCommandPrompt()
        {
            EditorGUILayout.BeginHorizontal();

            commandString = EditorGUILayout.TextField(commandString);

            if (GUILayout.Button("Execute", GUILayout.ExpandWidth(false)))
            {
                GitGud.RunCommand(commandString,
                (output) =>
                {
                    if (output.outputData != null)
                        outputString += output.outputData;

                    if (output.errorData != null)
                        outputString += "[ERROR] " + output.errorData;

                    commandString = "";

                    //Force to scroll to bottom
                    statusScroll = new Vector2(0, 1000000);
                });
            }

            EditorGUILayout.EndHorizontal();
        }


        /// <summary>
        /// Splits a string into [chunkSize]-sized array
        /// </summary>
        /// <param name="str"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }


    }
}