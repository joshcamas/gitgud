using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    public class ContextOption<T>
    {
        //Path of context menu
        public virtual string GetContextPath()
        {
            return null;
        }

        //Function run when commit context option is clicked
        public virtual void OnSelect(List<T> commits)
        {

        }

        //Function to check if this context option is disabled for commits
        public virtual bool IsDisabled(List<T> commits)
        {
            return false;
        }

        //Opens a context menu with a list of context options
        public static void ShowContextMenu(List<T> selected, List<ContextOption<T>> contextOptions)
        {
            GenericMenu menu = new GenericMenu();
            bool multipleSelected = (selected.Count > 1);

            foreach (ContextOption<T> option in contextOptions)
            {
                //Render disabled item if option is disabled
                if (option.IsDisabled(selected))
                    menu.AddDisabledItem(new GUIContent(option.GetContextPath()));
                else
                    //Otherwise render actual item
                    menu.AddItem(new GUIContent(option.GetContextPath()), false, () =>
                    {
                        option.OnSelect(selected);
                    });
            }
            menu.ShowAsContext();
        }

    }

}