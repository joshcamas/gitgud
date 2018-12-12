using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    [Tab(index = 30)]
    public class HistoryTab : Tab
    {
        private List<Commit> commits;
        private CommitListGUI commitList;

        private List<ContextOption<Commit>> commitContextOptions;

        public override string GetName()
        {
            return "History";
        }

        public override void OnEnable()
        {
            BuildContextLists();

            commitList = new CommitListGUI();
            commitList.OnEnable(commitContextOptions);

            ScanHistory();
        }

        //Scan attributes for context lists
        private void BuildContextLists()
        {
            commitContextOptions = new List<ContextOption<Commit>>();

            GitUtility.ForEachTypeWithOrdered<CommitContextAttribute>(true, (type, attribute) =>
            {
                commitContextOptions.Add((ContextOption<Commit>)Activator.CreateInstance(type));
            });
        }


        private void ScanHistory()
        {
            GitCore.Log(null, (output, commits) =>
            {
                if (output.errorData != null)
                {
                    commits = null;
                    return;
                }

                this.commits = commits;
            });
        }

        public override void Render(GitGudWindow window)
        {
            if (commits == null)
                return;

            commitList.Render(commits);
        }
    }

}