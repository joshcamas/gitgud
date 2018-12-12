using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GitGud
{
    //Base attribute that supports ordering
    public interface IOrderedAttribute
    {
        int GetIndex();
    }

}