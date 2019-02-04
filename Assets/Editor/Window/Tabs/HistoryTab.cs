using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace GitGud.UI
{
    [Tab(index = 40)]
    public class HistoryTab : Tab  
    {
        private Dictionary<string,Commit> commitsDict;
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

                this.commitsDict = commits;

                //Cache list form of dictionary
                this.commits = commitsDict.Values.ToList();
            });
        }

        public override void Render(GitGudWindow window)
        {
            if (commitsDict == null)
                return;

            commitList.Render(commits, commitsDict);
        }
    }

}