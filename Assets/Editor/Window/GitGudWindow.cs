using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{
    public enum FilePathMode
    {
        Staged, Unstaged
    }

    public class GitGudWindow : EditorWindow
    {
        private List<Tab> tabs;
        private int selectedTab;

        private List<TopButton> topButtons;

        private static bool planningRefresh = false;
        private static bool disableInput = false;

        private bool isDrawing = false;

        //Window creation
        [MenuItem("Window/GitGud")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            GitGudWindow.GetWindow(typeof(GitGudWindow), false, "GitGud");
        }

        //Scan for tab attributes
        public void BuildTabList()
        {
            tabs = new List<Tab>();

            GitUtility.ForEachTypeWithOrdered<TabAttribute>(true, (type, attribute) =>
            {
                tabs.Add((Tab)Activator.CreateInstance(type));
            });

        }

        public void BuildTopButtonList()
        {
            topButtons = new List<TopButton>();

            GitUtility.ForEachTypeWithOrdered<TopButtonAttribute>(true, (type, attribute) =>
            {
                TopButton button = (TopButton)Activator.CreateInstance(type);
                button.flexibleSpaceAfter = attribute.flexibleSpaceAfter;
                button.OnCreate();
                topButtons.Add(button);
            });
        }


        private void OnEnable()
        {
            BuildTabList();
            BuildTopButtonList();

            //Select default tab
            if (tabs.Count > 0)
                tabs[0].OnEnable();

            //Hook onto events
            GitEvents.OnLocalChange += PlanRefresh;
            GitEvents.OnGitCommandStart += () => { disableInput = true; };
            GitEvents.OnGitCommandComplete += () => { disableInput = false; };
        }

        private void PlanRefresh()
        {
            //Only refresh if not drawing
            if (isDrawing)
                planningRefresh = true;
            else
                Refresh();
                
        }

        //Triggers a refresh on all gui elements
        void Refresh()
        {
            //Top buttons
            foreach(TopButton button in topButtons)
            {
                button.Refresh();
            }

            //Current tab
            tabs[selectedTab].Refresh();

            planningRefresh = false;

            Repaint();
        }

        void OnGUI()
        {
            isDrawing = true;

            EditorGUI.BeginDisabledGroup(disableInput);

            RenderTopBar();

            RenderTabBar();

            if (tabs.Count > 0)
                tabs[selectedTab].Render(this);

            EditorGUI.EndDisabledGroup();

            if (planningRefresh && Event.current.type == EventType.Repaint)
                Refresh();

            isDrawing = false;

        }

        private void RenderTopBar()
        {

            EditorGUILayout.BeginHorizontal("Toolbar", GUILayout.ExpandWidth(true));
            
            foreach(TopButton button in topButtons)
            {
                button.Render(this);
            }

            EditorGUILayout.EndHorizontal();

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