using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    public class FileViewMode
    {
        //Context options that handle paths
        protected List<ContextOption<string>> contextOptions;
        protected Dictionary<FileStatus, Texture2D> fileStatusIcons;

        //Initializes viewer, and applies context hooks
        public virtual void Init(List<ContextOption<string>> contextOptions)
        {
            this.contextOptions = contextOptions;
            BuildIcons();
        }

        //Renders the viewer
        public virtual void Render(List<GitFile> files)
        {

        }

        //Returns the currently selected paths
        public virtual List<string> GetSelectedPaths()
        {
            return new List<string>();
        }

        //Forces selection of paths
        public virtual void SetSelectedPaths(List<string> selectedPaths)
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

        public static List<string> GetPaths(List<GitFile> files)
        {
            List<string> paths = new List<string>();

            for (int i = 0; i < files.Count; i++)
            {
                paths.Add(files[i].path);
            }

            return paths;
        }
    }

}