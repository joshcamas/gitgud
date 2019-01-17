using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GitGud
{
    //Wrapper for editor preferences related to GitGud
    public static class GitGudSettings
    {
        public static void ApplyDefaultSettings()
        {
            //Tag used to detect if default settings have been applied
            EditorPrefs.SetBool(BuildPreferenceKey("settings"), true);
            
            //Default git executable location
            EditorPrefs.SetString(BuildPreferenceKey("git_path"), @"C:\Program Files\Git\bin\git.exe");

            //Default project path is just up two folders, why not
            EditorPrefs.SetString(BuildPreferenceKey("repo_path"), Path.GetFullPath(Path.Combine(Application.dataPath, @"..\..\")));

            //Default git executable location
            EditorPrefs.SetBool(BuildPreferenceKey("debug"), false);

            EditorPrefs.SetBool(BuildPreferenceKey("refresh_on_edit"), true);
        }

        public static bool HasKey(string key)
        {
            //Detect settings not set
            if (!EditorPrefs.HasKey(BuildPreferenceKey("settings")))
                ApplyDefaultSettings();

            return EditorPrefs.HasKey(BuildPreferenceKey(key));
        }

        //STRING
        public static string GetString(string key)
        {
            //Detect settings not set
            if (!EditorPrefs.HasKey(BuildPreferenceKey("settings")))
                ApplyDefaultSettings();

            return EditorPrefs.GetString(BuildPreferenceKey(key));
        }

        public static void SetString(string key,string value)
        {
            //Detect settings not set
            if (!EditorPrefs.HasKey(BuildPreferenceKey("settings")))
                ApplyDefaultSettings();

            EditorPrefs.SetString(BuildPreferenceKey(key), value);
        }

        //BOOL
        public static bool GetBool(string key)
        {
            //Detect settings not set
            if (!EditorPrefs.HasKey(BuildPreferenceKey("settings")))
                ApplyDefaultSettings();

            return EditorPrefs.GetBool(BuildPreferenceKey(key));
        }

        public static void SetBool(string key, bool value)
        {
            //Detect settings not set
            if (!EditorPrefs.HasKey(BuildPreferenceKey("settings")))
                ApplyDefaultSettings();

            EditorPrefs.SetBool(BuildPreferenceKey(key), value);
        }

        //FLOAT
        public static float GetFloat(string key)
        {
            //Detect settings not set
            if (!EditorPrefs.HasKey(BuildPreferenceKey("settings")))
                ApplyDefaultSettings();

            return EditorPrefs.GetFloat(BuildPreferenceKey(key));
        }

        public static void SetFloat(string key, float value)
        {
            //Detect settings not set
            if (!EditorPrefs.HasKey(BuildPreferenceKey("settings")))
                ApplyDefaultSettings();

            EditorPrefs.SetFloat(BuildPreferenceKey(key), value);
        }

        //INT
        public static int GetInt(string key)
        {
            //Detect settings not set
            if (!EditorPrefs.HasKey(BuildPreferenceKey("settings")))
                ApplyDefaultSettings();

            return EditorPrefs.GetInt(BuildPreferenceKey(key));
        }

        public static void SetInt(string key, int value)
        {
            //Detect settings not set
            if (!EditorPrefs.HasKey(BuildPreferenceKey("settings")))
                ApplyDefaultSettings();

            EditorPrefs.SetInt(BuildPreferenceKey(key), value);
        }

        //Automatically saves key using the player setting's companyname and productname, to keep different
        //project data separate
        private static string BuildPreferenceKey(string key)
        {
            return "gitgud." + PlayerSettings.companyName + "." + PlayerSettings.productName + "." + key;
        }
    }
}