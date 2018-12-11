using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GitGud.UI
{
    public class Tab
    {
        public virtual string GetName()
        {
            return "Tab";
        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnDisable()
        {

        }
        
        public virtual void Render(EditorWindow window)
        {

        }

    }

}