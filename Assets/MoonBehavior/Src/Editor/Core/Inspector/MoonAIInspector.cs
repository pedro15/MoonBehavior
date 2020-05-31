using UnityEditor;

using MoonBehavior.BehaviorTrees;

namespace MoonBehaviorEditor.Core.Inspector
{
    /// <summary>
    /// Custom Inspector for AI Component
    /// </summary>
    [CustomEditor(typeof(MoonAI))]
    class MoonAIInspector : Editor
    {
        /// <summary>
        /// Tick mode property
        /// </summary>
        SerializedProperty TickMode;

        /// <summary>
        /// Assignable Behavior Tree Property
        /// </summary>
        SerializedProperty BehaviorTree;
        
        private void OnEnable()
        {
            TickMode = serializedObject.FindProperty("_TickMode");
            BehaviorTree = serializedObject.FindProperty("BehaviorTree");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(TickMode);

            if (!EditorApplication.isPlaying)
            {
                // Normal editor
                BehaviorTree.objectReferenceValue = EditorGUILayout.ObjectField("Behavior Tree", BehaviorTree.objectReferenceValue, typeof(MoonBT), false);
            }else
            {
                // Playmode = Debug Only.
                EditorGUILayout.LabelField("Behavior Tree: " + (BehaviorTree.objectReferenceValue != null ? BehaviorTree.objectReferenceValue.name : "null"));
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
