using UnityEditor;
using UnityEngine;

using MoonBehavior.BehaviorTrees.Actions;

namespace MoonBehaviorEditor.Core.Inspector
{
    /// <summary>
    /// Inspector for math node
    /// </summary>
    [CustomEditor(typeof(Math))]
    class MathInspector : Editor
    {

        SerializedProperty Mode;
        SerializedProperty AValue;
        SerializedProperty BValue;
        SerializedProperty SaveKey;

        private void OnEnable()
        {
            try
            {
                Mode = serializedObject.FindProperty("Mode");
                AValue = serializedObject.FindProperty("AValue");
                BValue = serializedObject.FindProperty("BValue");
                SaveKey = serializedObject.FindProperty("SaveKey");

            }catch { return; }
        }

        public override void OnInspectorGUI()
        {
            MoonGUILayout.DrawDefaultInspectorNoScript(serializedObject);

            serializedObject.Update();

            Math.MathMode mode = (Math.MathMode)Mode.enumValueIndex;

            EditorGUILayout.PropertyField(AValue);

            if (mode != Math.MathMode.Pow & mode != Math.MathMode.Sin & mode != Math.MathMode.Cosin & 
                mode != Math.MathMode.Sqrt & mode != Math.MathMode.Round & mode != Math.MathMode.Tan)
            {
                EditorGUILayout.PropertyField(BValue);
            }

            EditorGUILayout.PropertyField(SaveKey);

            if (GUI.changed)
                serializedObject.ApplyModifiedProperties();

        }
    }
}
