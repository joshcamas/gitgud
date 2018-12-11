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
            RenderTabBar();

            if (tabs.Count > 0)
                tabs[selectedTab].Render(this);

        }

        private void RenderTabBar()
        {
            EditorGUILayout.BeginHorizontal();

            for(int i =0;i< tabs.Count;i++)
            {
                bool selected = GUILayout.Toggle((selectedTab == i), tabs[i].GetName(), "Button");

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