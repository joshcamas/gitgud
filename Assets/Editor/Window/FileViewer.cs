using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    public class FileViewer
    {
        private List<ContextOption<string>> contextOptions;
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

            return viewModes;
        }

        public void Init(string title,List<ContextOption<string>> contextOptions)
        {
            this.title = title;
            this.contextOptions = contextOptions;

            if (viewMode != null)
                viewMode.Init(this.contextOptions);
        }

        public void Render(List<GitFile> files)
        {
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);

            RenderModeSelection();

            if (viewMode == null)
                return;

            viewMode.Render(files);
        }

        public List<string> GetSelectedPaths()
        {
            if (viewMode == null)
                return new List<string>();

            return viewMode.GetSelectedPaths();
        }

        public void SetSelectedPaths(List<string> paths)
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

            //Don't display selection if there isn't at least two options
            if (GetFileModes().Count < 2)
                return;

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