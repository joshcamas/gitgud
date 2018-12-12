using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    [Tab(1)]
    public class HistoryTab : Tab
    {
        private List<Commit> commits;
        private CommitListGUI commitList;

        public override string GetName()
        {
            return "History";
        }

        public override void OnEnable()
        {
            commitList = new CommitListGUI();
            commitList.OnEnable();

            ScanHistory();
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

        public override void Render(EditorWindow window)
        {
            if (commits == null)
                return;

            commitList.Render(commits);
        }
    }

}