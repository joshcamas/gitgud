using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{
    public class StashListGUI
    {
        private Vector2 scrollPosition;
        private List<ContextOption<string>> contextOptions;
        private List<string> selectedStashes;

        public void OnEnable(List<ContextOption<string>> contextOptions)
        {
            this.contextOptions = contextOptions;
        }

        public void Render(List<string> stashes)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);

            //Render list of files
            selectedStashes = SelectableListGUI.RenderList<string>(stashes, selectedStashes, RenderStash,(selectedStashes,mouse) =>
            {
                //Context click
                if (mouse == 1)
                {
                    ContextOption<string>.ShowContextMenu(selectedStashes, contextOptions);
                }

            },false);

            EditorGUILayout.EndScrollView();
        }

        private bool RenderStash(string commit, bool selected)
        {
            EditorGUILayout.BeginHorizontal();

            GUIStyle style = new GUIStyle("Label");

            if (selected)
                style.normal.textColor = Color.blue;

            //Buttons
            bool pressed = GUILayout.Button(commit, style);

            EditorGUILayout.EndHorizontal();

            return pressed;
        }
    }

}