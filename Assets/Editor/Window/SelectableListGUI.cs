using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace GitGud.UI
{
    public class SelectableListGUI
    {
        //Generic rendering function for a list of objects
        public static List<T> RenderList<T>(List<T> allObjects, List<T> selectedObjects,Func<T,bool, bool> renderObject,Action<List<T>,int> onClick=null,bool allowMultiple=true)
        {
            EditorGUILayout.BeginVertical();

            if (selectedObjects == null)
                selectedObjects = new List<T>();

            //Remove any missing files from selected files
            List<object> objectsToDeselect = new List<object>();

            foreach (T obj in selectedObjects)
            {
                if (!allObjects.Contains(obj))
                    objectsToDeselect.Add(obj);
            }

            foreach (T obj in objectsToDeselect)
            {
                selectedObjects.Remove(obj);
            }

            //Render Objects
            foreach (T obj in allObjects)
            {
                EditorGUILayout.BeginHorizontal();

                bool isSelected = selectedObjects.Contains(obj);

                bool pressed = renderObject(obj, isSelected);

                if (pressed)
                {
                    //Selection
                    if (Event.current.button == 0)
                    {
                        //Select multiple - in between (shift) mode
                        if (Event.current.shift && selectedObjects.Count != 0 && allowMultiple)
                        {
                            int startIndex = allObjects.IndexOf(selectedObjects[selectedObjects.Count - 1]);
                            int stopIndex = allObjects.IndexOf(obj);

                            //TODO: Replace double for loop with a multidirectional one somehow
                            if (startIndex < stopIndex)
                            {
                                for (int i = startIndex + 1; i <= stopIndex; i++)
                                {
                                    //Toggle file
                                    if (selectedObjects.Contains(allObjects[i]))
                                        selectedObjects.Remove(allObjects[i]);
                                    else
                                        selectedObjects.Add(allObjects[i]);
                                }
                            }
                            else if (startIndex > stopIndex)
                            {
                                for (int i = stopIndex; i < startIndex; i++)
                                {
                                    //Toggle file
                                    if (selectedObjects.Contains(allObjects[i]))
                                        selectedObjects.Remove(allObjects[i]);
                                    else
                                        selectedObjects.Add(allObjects[i]);
                                }
                            }

                        }
                        else if (Event.current.control && allowMultiple)
                        {
                            //Toggle Selected
                            if (selectedObjects.Contains(obj))
                                selectedObjects.Remove(obj);
                            else
                                selectedObjects.Add(obj);
                        }
                        else
                        {
                            //Select file
                            selectedObjects = new List<T> { obj };
                        }

                    }

                    //On click
                    if(onClick != null)
                    {
                        onClick(selectedObjects, Event.current.button);
                    }

                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            return selectedObjects;
        }
    }

}