using System.Reflection;
using System.Linq;

using UnityEngine;
using UnityEditor;

using MoonBehaviorEditor.Core.Graphs;
using MoonBehavior;

namespace MoonBehaviorEditor.Core.Inspector
{
    /// <summary>
    /// Custom node inspector
    /// </summary>
    [CustomEditor(typeof(Node), true)]
    internal class NodeInspector : Editor
    {
        /// <summary>
        /// inspector for node task
        /// </summary>
        Editor TaskInspector;
        /// <summary>
        /// Tooltip position on node 
        /// </summary>
        SerializedProperty tooltipPosition;
        
        /// <summary>
        /// Tooltip text
        /// </summary>
        SerializedProperty Tooltip;

        /// <summary>
        /// Task user defined info
        /// </summary>
        InfoAttribute TaskInfo;
        
        /// <summary>
        /// Node instance
        /// </summary>
        Node n;

        /// <summary>
        /// Invalid field names when displaying on Debug mode
        /// </summary>
        string[] InvalidFieldNames = new string[] { "name" , ">k__BackingField" , "hideFlags" };

        HelpURLAttribute Nodedoc = null;

        /// <summary>
        /// Are the field name valid ?
        /// </summary>
        /// <param name="name">Field Name</param>
        private bool ValidName(string name)
        {
            string foundstring = InvalidFieldNames.FirstOrDefault((string s) => name.Contains(s));
            return string.IsNullOrEmpty(foundstring);
        }
        
        private void OnEnable()
        {
            n = (Node)target;
            if (n != null)
            {
                TaskInspector = CreateEditor(n.Task);
                TaskInfo = MoonReflection.GetNodeData(n.Task.GetType());
            }

            tooltipPosition = serializedObject.FindProperty("tooltipPosition");
            Tooltip = serializedObject.FindProperty("Tooltip");

            Nodedoc = MoonReflection.GetAttribute(n.Task.GetType(), typeof(HelpURLAttribute), true) as HelpURLAttribute;
            
        }

        public override void OnInspectorGUI()
        {
            
            serializedObject.Update();

            EditorGUILayout.PropertyField(tooltipPosition);

            EditorGUILayout.PropertyField(Tooltip);

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }

            MoonGUILayout.Separator();
            
            if (TaskInfo != null )
            {

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(TaskInfo.Name , new GUIStyle(EditorStyles.label)
                {
                    fontStyle = FontStyle.Bold
                });

                GUILayout.FlexibleSpace();

                if (Nodedoc != null)
                {
                    if (GUILayout.Button(new GUIContent("?", "Open documentation"), EditorStyles.miniButton))
                    {
                        Application.OpenURL(Nodedoc.URL);
                    }
                }
                
                EditorGUILayout.EndHorizontal();

                if (!string.IsNullOrEmpty(TaskInfo.Description))
                    EditorGUILayout.HelpBox(TaskInfo.Description, MessageType.Info);

            }
            
            if (TaskInspector != null)
            {
                if (!EditorApplication.isPlaying)
                {
                    // Draw task inspector
                    TaskInspector.OnInspectorGUI();
                }
                else if (n != null)
                {
                    GUILayout.BeginVertical("Debug mode" , GUI.skin.box , GUILayout.ExpandWidth(true) );

                    GUILayout.Space(15);

                    if (!n.Task) return;
                    

                    FieldInfo[] fields = n.Task.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );

                    PropertyInfo[] props = n.Task.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    for (int i = 0; i < fields.Length; i++)
                    {
                        FieldInfo currentField = fields[i];

                        if (!ValidName(currentField.Name)) continue;

                        object[] atrs = currentField.GetCustomAttributes(typeof(HideOnDebugAttribute), true);

                        if (atrs != null && atrs.Length > 0) continue;
                        
                        object currentValue = currentField.GetValue(n.Task);

                        if (currentValue != null )
                        {
                            GUILayout.Label(ObjectNames.NicifyVariableName(currentField.Name) + ": " + currentValue.ToString());
                        }
                        else
                        {
                            GUILayout.Label(ObjectNames.NicifyVariableName(currentField.Name) + ": null");
                        }
                    }

                    for (int i = 0; i < props.Length; i++)
                    {
                        PropertyInfo currentProp = props[i];
                        object currentValue = currentProp.GetValue(n.Task , null);

                        if (!ValidName(currentProp.Name)) continue;
                        
                        object[] atrs = currentProp.GetCustomAttributes(typeof(HideOnDebugAttribute), true);

                        if (atrs != null && atrs.Length > 0) continue;

                        if (currentValue != null)
                        {
                            GUILayout.Label(ObjectNames.NicifyVariableName(currentProp.Name) + ": " + currentValue.ToString());
                        }
                        else
                        {
                            GUILayout.Label(ObjectNames.NicifyVariableName(currentProp.Name) + ": null");
                        }
                    }

                    GUILayout.EndVertical();

                    Repaint();
                }
            }
        }
    }
}