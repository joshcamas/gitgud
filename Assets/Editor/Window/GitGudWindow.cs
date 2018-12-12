using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{
    public enum FilePathModes
    {
        Staged, Unstaged
    }

    public class GitGudWindow : EditorWindow
    {
        public List<Tab> tabs;
        public int selectedTab;

        //Window creation
        [MenuItem("GitGud/GitGud")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            GitGudWindow.GetWindow(typeof(GitGudWindow), false, "GitGud");
        }

        //Scan for tab attributes
        public void BuildTabList()
        {
            tabs = new List<Tab>();

            GitUtility.ForEachTypeWith<TabAttribute>(true, (type, attribute) =>
            {
                Tab newTab = (Tab)Activator.CreateInstance(type);

                //No set index, don't care about it then
                if (attribute.index < 0)
                    tabs.Add(newTab);
                else
                {
                   //Crazy code that adds tab at correct index

                    if(tabs.Count >= attribute.index)
                        tabs.Insert(attribute.index, newTab);

                    else if(tabs.Count == attribute.index)
                        tabs.Add(newTab);

                    else
                    {
                        while (tabs.Count <= attribute.index)
                            tabs.Add(null);

                        tabs[attribute.index] = newTab;
                    }
                }
            });

            //Delete null tabs
            for(int i =0;i<tabs.Count;i++)
            {
                if (tabs[i] == null)
                {
                    tabs.RemoveAt(i);
                    i--;
                } 
            }

        }

        private void OnEnable()
        {
            BuildTabList();

            //Select default tab
            if(tabs.Count > 0)
                tabs[0].OnEnable();

        }

        void OnGUI()
        {
            RenderTopBar();

            RenderTabBar();

            if (tabs.Count > 0)
                tabs[selectedTab].Render(this);

        }

        private void RenderTopBar()
        {
            //Top bar doesn't do anything yet
            EditorGUI.BeginDisabledGroup(true);

            EditorGUILayout.BeginHorizontal("Toolbar", GUILayout.ExpandWidth(true));

            GUILayout.Button("Commit", "ToolbarButton", GUILayout.ExpandWidth(false));
            GUILayout.Button("Pull", "ToolbarButton", GUILayout.ExpandWidth(false));
            GUILayout.Button("Push (0)", "ToolbarButton", GUILayout.ExpandWidth(false));
            GUILayout.Button("Fetch", "ToolbarButton", GUILayout.ExpandWidth(false));
            GUILayout.FlexibleSpace();
            GUILayout.Button("Merge", "ToolbarButton", GUILayout.ExpandWidth(false));
            GUILayout.Button("Branch", "ToolbarButton", GUILayout.ExpandWidth(false));
            GUILayout.FlexibleSpace();
            GUILayout.Button("Stash", "ToolbarButton", GUILayout.ExpandWidth(false));
            GUILayout.Button("Discard", "ToolbarButton", GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();

            EditorGUI.EndDisabledGroup();
        }

        private void RenderTabBar()
        {
            EditorGUILayout.BeginHorizontal();

            for(int i =0;i< tabs.Count;i++)
            {
                bool selected = GUILayout.Toggle((selectedTab == i), tabs[i].GetName(), "Button",GUILayout.Height(30));

                //Detect new selection
                if(selectedTab != i && selected)
                {
                    tabs[selectedTab].OnDisable();
                    tabs[i].OnEnable();
                    selectedTab = i;
                }
            }

            EditorGUILayout.EndHorizontal();

        }

    }
}