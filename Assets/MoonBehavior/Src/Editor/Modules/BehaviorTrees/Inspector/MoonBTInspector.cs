using UnityEngine;
using UnityEditor;

using MoonBehavior.BehaviorTrees;

namespace MoonBehaviorEditor.Core.Inspector
{ 
    /// <summary>
    /// Moon Behavior tree inspector override
    /// </summary>
    [CustomEditor(typeof(MoonBT))]
    public class MoonBTInspector : Editor
    {
        SerializedProperty Description;
        private void OnEnable()
        {
            Description = serializedObject.FindProperty("Description");
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical("", GUI.skin.box);

            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            GUILayout.Label("Moon BehaviorTree" , EditorStyles.largeLabel);


            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("Attach this asset to a MoonAI component to see this BehaviorTree work. Or " +
                "open it to edit the BehaviorTree", MessageType.Info);
        
            GUILayout.EndVertical();

            serializedObject.Update();

            EditorGUILayout.PropertyField(Description, GUILayout.Height(100));

            if (GUI.changed)
                serializedObject.ApplyModifiedProperties();

        }
    }
}
