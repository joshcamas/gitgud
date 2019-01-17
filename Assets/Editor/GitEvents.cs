using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GitGud
{
    //Static events to hook onto git
    public class GitEvents
    {
        public delegate void BaseEvent();

        /// <summary>
        /// Triggered when there has been some sort of change to local files
        /// </summary>
        public static BaseEvent OnLocalChange;

        /// <summary>
        /// Triggered when a git command has begun
        /// </summary>
        public static BaseEvent OnGitCommandStart;

        /// <summary>
        /// Triggered when a git command has completed
        /// </summary>
        public static BaseEvent OnGitCommandComplete;

        public static void TriggerOnLocalChange()
        {
            if (OnLocalChange != null)
                OnLocalChange();
        }

        public static void TriggerOnGitCommandStart()
        {
            if (OnGitCommandStart != null)
                OnGitCommandStart();
        }

        public static void TriggerOnGitCommandComplete()
        {
            if (OnGitCommandComplete != null)
                OnGitCommandComplete();
        }
    }

}