using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{
    [TopButton(index = 70)]
    public class DiscardTopButton : TopButton
    {
        public override string GetName()
        {
            return "Discard";
        }

        public override bool IsDisabled()
        {
            return false;
        }

        //Run when button is clicked
        public override void OnClick(GitGudWindow window)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddDisabledItem(new GUIContent("Discard..."));
            menu.AddItem(new GUIContent("Discard All Unstaged"), false, DiscardUnstaged);
            menu.AddItem(new GUIContent("Reset Local Repo"), false, Reset);
            menu.ShowAsContext();
        }

        private void DiscardUnstaged()
        {
            bool discard = EditorUtility.DisplayDialog("Warning", "This will discard all unstaged changes - is this okay?", "Discard", "Cancel");

            if (!discard)
                return;

            GitCore.DiscardAll((output) =>
            {
                GitEvents.TriggerOnLocalChange();
                AssetDatabase.Refresh();
            });
        }

        private void Reset()
        {
            bool reset = EditorUtility.DisplayDialog("Warning", "This will reset the entire local repository - is this okay?", "Reset", "Cancel");

            if (!reset)
                return;

            //Clean and reset
            GitGud.RunCommands(new string[]{ "clean -f -d", "reset --hard"}, (outputs) =>
            {
                GitEvents.TriggerOnLocalChange();
                AssetDatabase.Refresh();
            });

        }
    }

}