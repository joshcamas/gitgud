using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
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

        //Temporary function that is all tabs (will use attribute eventually)
        public void BuildTabList()
        {
            tabs = new List<Tab>() { new ChangesTab(), new HistoryTab(),new SettingsTab() };
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