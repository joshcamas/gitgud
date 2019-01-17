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

        public static void TriggerOnLocalChange()
        {
            if (OnLocalChange != null)
                OnLocalChange();
        }

    }

}