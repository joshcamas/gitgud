using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    public class FileViewer
    {
        private List<PathContextOption> contextOptions;
        private FileViewMode viewMode;
        protected string title;

        //Returns a list of supported file modes. Currently this is hardcoded,
        //eventually I will add an attribute that will be used to automatically
        //find all file modes
        public Dictionary<string,Type> GetFileModes()
        {
            //TODO: Add cacheing

            Dictionary<string, Type> viewModes = new Dictionary<string, Type>();

            viewModes.Add("List", typeof(ListFileViewMode));
            viewModes.Add("Tree", typeof(TreeFileViewMode));

            return viewModes;
        }

        public void Init(string title,List<PathContextOption> contextOptions)
        {
            this.title = title;
            this.contextOptions = contextOptions;

            if (viewMode != null)
                viewMode.Init(this.contextOptions);
        }

        public void Render(GitFile[] files)
        {
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);

            RenderModeSelection();

            if (viewMode == null)
                return;

            viewMode.Render(files);
        }

        public string[] GetSelectedPaths()
        {
            if (viewMode == null)
                return new string[0];

            return viewMode.GetSelectedPaths();
        }

        public void SetSelectedPaths(string[] paths)
        {
            if (viewMode == null)
                return;

            viewMode.SetSelectedPaths(paths);
        }

        private void SelectViewMode(Type type)
        {
            viewMode = (FileViewMode)Activator.CreateInstance(type);
            viewMode.Init(contextOptions);

        }
 
        private void RenderModeSelection()
        {
            if (GetFileModes() == null || GetFileModes().Count == 0)
                return;

            //Default selection
            if(viewMode == null)
            {
                SelectViewMode(GetFileModes().First().Value);
            }

            EditorGUILayout.BeginHorizontal();

            foreach(KeyValuePair<string,Type> pair in GetFileModes())
            {
                bool pressed = (viewMode.GetType() == pair.Value);

                bool newPressed = GUILayout.Toggle(pressed, pair.Key, "Button", GUILayout.ExpandWidth(false));

                //Detect change in toggle
                if(pressed != newPressed && newPressed == true)
                {
                    SelectViewMode(pair.Value);
                }

            }

            EditorGUILayout.EndHorizontal();

        }
    }
}