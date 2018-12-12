using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    [Tab(index = 40)]
    public class SettingsTab : Tab
    {
        public override string GetName()
        {
            return "Settings";
        }

        public override void Render(GitGudWindow window)
        {
            EditorGUILayout.LabelField("GitGud: A rad source control from within Unity", EditorStyles.boldLabel);

            string gitPath = EditorGUILayout.TextField("Git Path", GitGudSettings.GetString("git_path"));
            GitGudSettings.SetString("git_path", gitPath);

            string projectPath = EditorGUILayout.TextField("Repo Path", GitGudSettings.GetString("repo_path"));
            GitGudSettings.SetString("repo_path", projectPath);

            bool debug = EditorGUILayout.Toggle("Debug Mode", GitGudSettings.GetBool("debug"));
            GitGudSettings.SetBool("debug", debug);

            if (GUILayout.Button("Reset to Default", GUILayout.ExpandWidth(false)))
                GitGudSettings.ApplyDefaultSettings();
        }
    }

}