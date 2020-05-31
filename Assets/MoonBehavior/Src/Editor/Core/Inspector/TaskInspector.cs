using UnityEditor;
using MoonBehavior.BehaviorTrees;

namespace MoonBehaviorEditor.Core.Inspector
{
    [CustomEditor(typeof(Task) , true )]
    class TaskInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            MoonGUILayout.DrawDefaultInspectorNoScript(serializedObject);
        }
    }
}
