using UnityEngine;
using UnityEditor;
using MoonBehavior.Perception;
using System.Linq;
using System.Collections.Generic;

namespace MoonBehaviorEditor.Core.Inspector
{
    /// <summary>
    /// Filter collection inspector
    /// </summary>
    [CustomEditor(typeof(SensorFilterCollection))]
    class SensorFilterCollectionInspector : Editor
    {
        /// <summary>
        /// Filters list
        /// </summary>
        SerializedProperty Filters;
        
        private void OnEnable()
        {
            Filters = serializedObject.FindProperty("Filters");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            // Draw inspector for each filter

            for (int i = 0; i < Filters.arraySize; i++)
            {
                SerializedProperty currentf = Filters.GetArrayElementAtIndex(i);

                GUILayout.BeginHorizontal();

                currentf.isExpanded = EditorGUILayout.Foldout(currentf.isExpanded, new GUIContent( ObjectNames.NicifyVariableName(currentf.objectReferenceValue.GetType().Name)));

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Remove" , EditorStyles.miniButton ))
                {
                    DestroyImmediate(currentf.objectReferenceValue, true);
                    AssetDatabase.SaveAssets();
                    Filters.DeleteArrayElementAtIndex(i);
                    Filters.DeleteArrayElementAtIndex(i);
                    serializedObject.ApplyModifiedProperties();
                    continue; 
                }

                GUILayout.EndHorizontal();

                if (currentf.isExpanded)
                {
                    Editor m_editor = CreateEditor(currentf.objectReferenceValue);

                    GUILayout.Space(5);

                    m_editor.OnInspectorGUI();
                }

                MoonGUILayout.Separator();

            }
            
            GUILayout.Space(5);

            // add buttun filter

            if (GUILayout.Button("Add Filter" , GUILayout.Height(20)))
            {
                GenericMenu Menu = new GenericMenu();

                // Gets all aviable filters via reflection

                List<System.Type> m_types = MoonReflection.GetAllDerivedTypes(typeof(SensorFilter)).ToList();

                for (int i = 0; i < m_types.Count; i++)
                {
                    System.Type currt = m_types[i];

                    if (currt != null )
                    {
                        Menu.AddItem(new GUIContent(currt.Name), false, new GenericMenu.MenuFunction(() => 
                        {
                            int index = Filters.arraySize;
                            SensorFilter filter = CreateInstance(currt) as SensorFilter;
                            filter.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector; 

                            AssetDatabase.AddObjectToAsset(filter, target);

                            AssetDatabase.SaveAssets();

                            Filters.InsertArrayElementAtIndex(index);

                            SerializedProperty curr = Filters.GetArrayElementAtIndex(index);

                            curr.objectReferenceValue = filter;

                            serializedObject.ApplyModifiedProperties();

                        }));
                    }

                }

                Menu.ShowAsContext();

            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
