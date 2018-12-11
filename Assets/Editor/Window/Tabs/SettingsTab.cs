using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    public class SettingsTab : Tab
    {
        public override string GetName()
        {
            return "Settings";
        }

        public override void Render(EditorWindow window)
        {
            EditorGUILayout.LabelField("GitGud: A rad source control from within Unity", EditorStyles.boldLabel);

            string gitPath = EditorGUILayout.TextField("Git Path", GitGudSettings.GetString("git_path"));
            GitGudSettings.SetString("git_path", gitPath);

            string projectPath = EditorGUILayout.TextField("Repo Path", GitGudSettings.GetString("repo_path"));
            GitGudSettings.SetString("repo_path", projectPath);

            if (GUILayout.Button("Reset to Default", GUILayout.ExpandWidth(false)))
                GitGudSettings.ApplyDefaultSettings();
        }
    }

}