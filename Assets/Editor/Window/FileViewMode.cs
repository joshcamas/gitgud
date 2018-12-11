using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    public class FileViewMode
    {
        protected List<PathContextOption> contextOptions;
        protected Dictionary<FileStatus, Texture2D> fileStatusIcons;

        //Initializes viewer, and applies context hooks
        public virtual void Init(List<PathContextOption> contextOptions)
        {
            this.contextOptions = contextOptions;
            BuildIcons();
        }

        //Renders the viewer
        public virtual void Render(GitFile[] files)
        {

        }

        //Returns the currently selected paths
        public virtual string[] GetSelectedPaths()
        {
            return new string[0];
        }

        //Forces selection of paths
        public virtual void SetSelectedPaths(string[] selectedPaths)
        {

        }

        protected void BuildIcons()
        {
            fileStatusIcons = new Dictionary<FileStatus, Texture2D>();

            //Loop all possible statuses, and check if there are any icons
            foreach (FileStatus f in Enum.GetValues(typeof(FileStatus)))
            {
                Texture2D icon = Resources.Load<Texture2D>("giticon-file-" + f.ToString());
                if (icon != null)
                    fileStatusIcons.Add(f, icon);
            }
        }

        protected virtual void ShowContextMenu(string[] selectedPaths)
        {
            GenericMenu menu = new GenericMenu();
            bool multipleSelected = (selectedPaths.Length > 1);

            foreach (PathContextOption option in contextOptions)
            {
                //Render disabled item if option is disabled
                if (option.IsDisabled(selectedPaths))
                    menu.AddDisabledItem(new GUIContent(option.GetContextPath()));
                else 
                    //Otherwise render actual item
                    menu.AddItem(new GUIContent(option.GetContextPath()), false, () => 
                    {
                        option.OnSelect(selectedPaths);
                    });

            }

            menu.ShowAsContext();
        }
    }

    public enum FileStatus
    {
        unmodified,modified,added,deleted,renamed,copied,untracked,ignored
    }

    public struct GitFile
    {
        public GitFile(string path, FileStatus status)
        {
            this.path = path;
            this.status = status;
        }

        public string path;
        public FileStatus status;

        public static FileStatus StatusCodeToFileStatus(char statusCode)
        {
            switch(statusCode)
            {
                case 'M':
                    return FileStatus.modified;
                case 'A':
                    return FileStatus.added;
                case 'D':
                    return FileStatus.deleted;
                case 'R':
                    return FileStatus.renamed;
                case 'C':
                    return FileStatus.copied;
                case '?':
                    return FileStatus.untracked;
                case '!':
                    return FileStatus.ignored;
                default:
                    return FileStatus.unmodified;
            }
        }

        public static string[] GetPaths(GitFile[] files)
        {
            string[] paths = new string[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                paths[i] = files[i].path;
            }

            return paths;
        }

        public static string[] GetPaths(List<GitFile> files)
        {
            string[] paths = new string[files.Count];

            for (int i = 0; i < files.Count; i++)
            {
                paths[i] = files[i].path;
            }

            return paths;
        }
    }

}