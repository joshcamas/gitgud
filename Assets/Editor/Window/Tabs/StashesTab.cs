using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    [Tab(index = 20)]
    public class StashesTab : Tab
    {
        private bool readStashes = false;
        private List<string> stashes;
        private StashListGUI stashListGUI;
        private List<ContextOption<string>> contextOptions;

        public override string GetName()
        {
            return "Stashes";
        }

        public override void OnEnable()
        {
            BuildContextLists();
            stashListGUI = new StashListGUI();
            stashListGUI.OnEnable(contextOptions);
            ScanStashes();
        }

        //Scan attributes for context lists
        private void BuildContextLists()
        {
            contextOptions = new List<ContextOption<string>>();

            GitUtility.ForEachTypeWithOrdered<StashContextAttribute>(true, (type, attribute) =>
            {
                contextOptions.Add((ContextOption<string>)Activator.CreateInstance(type));
            });
        }


        private void ScanStashes()
        {
            GitCore.ListStash((output, stashes) =>
            {
                if (output.errorData != null)
                {
                    stashes = null;
                    return;
                }

                this.stashes = stashes;
            });
        }

        public override void Render(GitGudWindow window)
        {
            if (stashes == null)
                return;

            stashListGUI.Render(stashes);
        }

    }

}