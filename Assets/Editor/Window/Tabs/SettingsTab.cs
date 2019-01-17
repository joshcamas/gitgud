using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    [Tab(index = 50)]
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

            GUIContent refreshOnEditContent = new GUIContent("Refresh on Edit", "If enabled, window will refresh when assets are edited");
            bool refreshOnEdit = EditorGUILayout.Toggle(refreshOnEditContent, GitGudSettings.GetBool("refresh_on_edit"));
            GitGudSettings.SetBool("refresh_on_edit", refreshOnEdit);

            GUIContent debugContent = new GUIContent("Debug Mode", "If enabled, will log all commands run and all output git processes return");
            bool debug = EditorGUILayout.Toggle(debugContent, GitGudSettings.GetBool("debug"));
            GitGudSettings.SetBool("debug", debug);

            if (GUILayout.Button("Reset to Default", GUILayout.ExpandWidth(false)))
                GitGudSettings.ApplyDefaultSettings();
        }
    }

}