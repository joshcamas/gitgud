using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using UnityEngine;

namespace GitGud
{
    //Helper functions
    public class GitUtility
    {

        //Converts a path relative to the repo to an absolute path
        public static string RepoPathToAbsolutePath(string repoPath)
        {
            return System.IO.Path.Combine(GitGudSettings.GetString("repo_path"), repoPath);
        }

        //Converts a path relative to the repo to a path relative to the unity assets folder
        public static string RepoPathToRelativeAssetPath(string repoPath)
        {
            string absolutePath = RepoPathToAbsolutePath(repoPath);

            //Make sure repopath is actually inside the asset path
            if (Application.dataPath.Length > absolutePath.Length)
                return null;

            return "Assets" + absolutePath.Substring(Application.dataPath.Length);
        }

        /// <summary>
        /// Helper function that runs a function for every type that has a certain attribute
        /// </summary>
        public static void ForEachTypeWith<TAttribute>(bool inherit, Action<Type, TAttribute> function) where TAttribute : System.Attribute
        {
            foreach (Type type in GetTypesWith<TAttribute>(inherit))
            {
                TAttribute attribute =
                    type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;

                function(type, attribute);
            }
        }

        /// <summary>
        /// Simple helper function that returns types that have a certain attribute
        /// </summary>
        public static IEnumerable<Type> GetTypesWith<TAttribute>(bool inherit)
                                     where TAttribute : System.Attribute
        {
            return from a in AppDomain.CurrentDomain.GetAssemblies()
                   from t in a.GetTypes()
                   where t.IsDefined(typeof(TAttribute), inherit)
                   select t;
        }

    }

}