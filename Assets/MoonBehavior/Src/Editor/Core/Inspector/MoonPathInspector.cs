using UnityEditor;
using UnityEditorInternal; 
using UnityEngine;
using MoonBehavior.Navigation;

namespace MoonBehaviorEditor.Core.Inspector
{
    /// <summary>
    /// Custom Inspector for MoonPath
    /// </summary>
    [CustomEditor(typeof(MoonPath))]
    class MoonPathInspector : Editor
    {
        /// <summary>
        /// Curren Path Instance
        /// </summary>
        MoonPath m_path;
        /// <summary>
        /// Path Last Position
        /// </summary>
        Vector3 m_lastPost = Vector3.zero;
        /// <summary>
        /// Nice reorderable list for displaying path list
        /// </summary>
        ReorderableList m_list;
        /// <summary>
        /// Waypoints path list
        /// </summary>
        SerializedProperty Waypoints;
        
        private void OnEnable()
        {
            m_path = (MoonPath)target;
            m_lastPost = m_path.transform.position; 

            Waypoints = serializedObject.FindProperty("Waypoints");
            m_list = new ReorderableList(serializedObject, Waypoints, true, true, true, true);
            m_list.elementHeight = 25f; 
            m_list.drawHeaderCallback = (Rect r) =>
            {
                EditorGUI.LabelField(r, "Waypoints");
            };

            m_list.drawElementCallback = (Rect r, int index, bool active, bool focused) =>
            {
                SerializedProperty curr = Waypoints.GetArrayElementAtIndex(index);

                Rect curr_label = new Rect(r.x, r.y + 2, 20, 16);

                EditorGUI.LabelField(curr_label, index.ToString());

                Rect valrect = new Rect( curr_label.xMax , r.y + 2, r.width - 40, 16);

                curr.vector3Value = EditorGUI.Vector3Field(valrect, string.Empty , curr.vector3Value);
                
                if (GUI.Button(new Rect(valrect.xMax , valrect.y , 20 , 16 )  , new GUIContent("D" , "Drop to ground" ) , GUI.skin.FindStyle("minibuttonright") ) )
                {
                    curr.vector3Value = DropToGround(curr.vector3Value);
                }
                
            };

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Update waypoints on move:");
            Waypoints.isExpanded = EditorGUILayout.Toggle(Waypoints.isExpanded);
            EditorGUILayout.EndHorizontal();

            m_list.DoLayoutList();

            GUILayout.Space(5);
            
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Drops to ground assigned vector3
        /// </summary>
        /// <param name="Origin">Vector3 to drop</param>
        /// <returns>Grounded Vector</returns>
        private Vector3 DropToGround(Vector3 Origin)
        {
            RaycastHit hit; 
            if (Physics.Raycast(Origin , -Vector3.up , out hit ) )
            {
                return hit.point;
            }
            return Origin;
        }

        private void OnSceneGUI()
        {

            // Scene drawing stuff: Drawing position handles for every Waypoint like native transform tool.

            serializedObject.Update();

            for (int i = 0; i < Waypoints.arraySize; i++)
            {
                SerializedProperty curr = Waypoints.GetArrayElementAtIndex(i);

                Handles.BeginGUI();

                Vector2 pos2D = HandleUtility.WorldToGUIPoint(curr.vector3Value);


                GUI.Label(new Rect(pos2D.x, pos2D.y + 15, 100, 100), i.ToString(), new GUIStyle(EditorStyles.label)
                {
                    normal = new GUIStyleState()
                    {
                        textColor = Color.black
                    },
                    fontSize = 16
                });


                Handles.EndGUI();

                Vector3 desiredPosition = Handles.DoPositionHandle(curr.vector3Value, Quaternion.identity);

                Vector3 diff = (curr.vector3Value - desiredPosition);

                curr.vector3Value = desiredPosition;

                if (Event.current.keyCode == KeyCode.Space && Event.current.type == EventType.KeyDown && diff.sqrMagnitude > 0.03f)
                {
                    curr.vector3Value = DropToGround(curr.vector3Value);
                }

                if (Waypoints.isExpanded)
                {
                    Vector3 delta = (m_path.transform.position - m_lastPost);


                    if (delta.sqrMagnitude > 0.01f)
                    {
                        curr.vector3Value += delta;

                        if (i == Waypoints.arraySize - 1)
                        {
                            m_lastPost = m_path.transform.position;
                        }
                    }
                }

            }

            serializedObject.ApplyModifiedProperties();
            
        }
    }
}
