using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using MoonBehavior.Perception;

namespace MoonBehaviorEditor.Core.Inspector
{
    /// <summary>
    /// Custom inspector for perception module
    /// </summary>
    [CustomEditor(typeof(MoonPerception))]
    public class MoonPerceptionInspector : Editor
    {
        /// <summary>
        /// Visual sensors list
        /// </summary>
        SerializedProperty VisualSensors;
        
        /// <summary>
        /// Nice reorderable list for displaying elements
        /// </summary>
        ReorderableList m_list;

        /// <summary>
        /// Perception instance
        /// </summary>
        MoonPerception instance;

        private void OnEnable()
        {
            instance = (MoonPerception)target;
            
            VisualSensors = serializedObject.FindProperty("VisualSensors");
            m_list = new ReorderableList(serializedObject, VisualSensors, true, true, true, true);

            m_list.drawHeaderCallback = (Rect r) =>
            {
                EditorGUI.LabelField(r, "Visual Sensors");
            };

            m_list.elementHeightCallback = (int i) =>
            {
                SerializedProperty elem = VisualSensors.GetArrayElementAtIndex(i);

                return elem.isExpanded ? 230f : 45f; 
            };

            m_list.drawElementCallback = (Rect r, int index, bool isactive, bool isfocused) =>
            {
                SerializedProperty Current = VisualSensors.GetArrayElementAtIndex(index);

                SerializedProperty Name = Current.FindPropertyRelative("SensorName");

                SerializedProperty SensorColor = Current.FindPropertyRelative("SensorColor");


                Rect rect_expand = new Rect(r.x , r.y + 5 , 16, 16);

                Rect rect_nameLabel = new Rect(r.x + 18, r.y + 5, r.width - 20, 18);
                
                if (GUI.Button(rect_expand, new GUIContent(Current.isExpanded ? "-" : "+", "Expand/Hide"),
                    GUI.skin.FindStyle((Current.isExpanded) ? "OL Minus" : "OL Plus")))
                {
                    Current.isExpanded = !Current.isExpanded;
                }


                EditorGUI.LabelField(rect_nameLabel, "Sensor Name");

                Rect rect_name = new Rect(r.x, rect_nameLabel.yMax, r.width - 60, 16);

                Rect rect_color = new Rect(rect_name.xMax + 5, rect_name.y, 55, 16);

                SensorColor.colorValue = EditorGUI.ColorField(rect_color, SensorColor.colorValue);

                Name.stringValue = EditorGUI.TextField(rect_name, Name.stringValue);

                if (Current.isExpanded)
                {
                    // Angle ---

                    SerializedProperty Angle = Current.FindPropertyRelative("Angle");

                    Rect rect_AngleLabel = new Rect(r.x, rect_name.yMax + 5, 60, 16);

                    EditorGUI.LabelField(rect_AngleLabel, "Angle");

                    Rect rect_Angle = new Rect(rect_AngleLabel.xMax, rect_AngleLabel.y, r.width - rect_AngleLabel.width, 16);

                    Angle.floatValue = EditorGUI.Slider(rect_Angle, Angle.floatValue, 0, 360);

                    // Distance ---

                    SerializedProperty Distance = Current.FindPropertyRelative("Distance");
                    
                    Rect rect_Distance = new Rect(r.x, rect_Angle.yMax + 5, r.width , 16);

                    Distance.floatValue = Mathf.Clamp(EditorGUI.FloatField(rect_Distance , "Distance",  Distance.floatValue) , 0 , float.MaxValue );

                    // Position offset --- 

                    SerializedProperty PositionOffset = Current.FindPropertyRelative("PositionOffset");

                    Rect rect_PositionLabel = new Rect(r.x, rect_Distance.yMax + 5, 100, 16);

                    EditorGUI.LabelField(rect_PositionLabel, "Position Offset");

                    Rect rect_Position = new Rect(rect_PositionLabel.xMax + 2, rect_PositionLabel.y, r.width - rect_PositionLabel.width, 16);

                    PositionOffset.vector3Value = EditorGUI.Vector3Field(rect_Position, "", PositionOffset.vector3Value);

                    // Rotation Offset ---

                    SerializedProperty RotationOffset = Current.FindPropertyRelative("RotationOffset");

                    Rect rect_RotationLabel = new Rect(r.x, rect_Position.yMax + 5, 100, 16);

                    EditorGUI.LabelField(rect_RotationLabel, "Rotation Offset");

                    Rect rect_Rotation = new Rect(rect_RotationLabel.xMax + 2, rect_RotationLabel.y, r.width - rect_RotationLabel.width, 16);

                    RotationOffset.vector3Value = EditorGUI.Vector3Field(rect_Rotation, "", RotationOffset.vector3Value);

                    // Target mask

                    SerializedProperty TargetMask = Current.FindPropertyRelative("TargetMask");
                    
                    Rect rect_TargetMask = new Rect(r.x , rect_Rotation.yMax + 5, r.width, 16);

                    EditorGUI.PropertyField(rect_TargetMask, TargetMask);
                    

                    // Obstacle mask

                    SerializedProperty ObstacleMask = Current.FindPropertyRelative("ObstacleMask");
                    
                    Rect rect_ObstacleMask = new Rect( r.x , rect_TargetMask.yMax + 5 , r.width , 16);

                    EditorGUI.PropertyField(rect_ObstacleMask, ObstacleMask);

                    // Deep detect

                    SerializedProperty DeepDetect = Current.FindPropertyRelative("DeepDetect");

                    Rect rect_DeepDetectLabel = new Rect(r.x, rect_ObstacleMask.yMax + 3, 100, 16);

                    EditorGUI.LabelField(rect_DeepDetectLabel, "Deep detect");

                    Rect rect_DeepDetect = new Rect(rect_DeepDetectLabel.xMax + 2, rect_DeepDetectLabel.y, 16, 16);

                    DeepDetect.boolValue = EditorGUI.Toggle(rect_DeepDetect, DeepDetect.boolValue);
                    
                    // Ignore Triggers 

                    SerializedProperty IgnoreTriggers = Current.FindPropertyRelative("IgnoreTriggers");

                    Rect rect_IgnoreTriggersLabel = new Rect(r.x, rect_DeepDetect.yMax + 5, 100, 16);

                    EditorGUI.LabelField(rect_IgnoreTriggersLabel, "Ignore Triggers");

                    Rect rect_IgnoreTriggers = new Rect(rect_IgnoreTriggersLabel.xMax + 2 , rect_IgnoreTriggersLabel.y, 16 , 16);

                    IgnoreTriggers.boolValue = EditorGUI.Toggle(rect_IgnoreTriggers, IgnoreTriggers.boolValue);

                    // Filters 

                    SerializedProperty filters = Current.FindPropertyRelative("FilterCollection");

                    Rect rect_filters = new Rect(r.x, rect_IgnoreTriggers.yMax + 5, r.width, 16);

                    filters.objectReferenceValue = EditorGUI.ObjectField(rect_filters, "Filters", filters.objectReferenceValue, typeof(SensorFilterCollection),false); 
                    
                }

            };

            m_list.onAddCallback = (ReorderableList list) =>
            {
                int nextindex = VisualSensors.arraySize > 0 ? VisualSensors.arraySize : 0;
                VisualSensors.InsertArrayElementAtIndex(nextindex);
                SerializedProperty added = VisualSensors.GetArrayElementAtIndex(nextindex);

                SerializedProperty added_color = added.FindPropertyRelative("SensorColor");

                SerializedProperty added_name = added.FindPropertyRelative("SensorName");

                added_name.stringValue = string.Empty;

                added_color.colorValue = new Color(
                    Random.Range(0.4f , 1f), Random.Range(0.0f, 1f), Random.Range(0.6f, 1f), 1f);
                
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Space(5);

            m_list.DoLayoutList();

            GUILayout.Space(5);

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void OnSceneGUI()
        {
            // Scene Drawing stuff

            for (int i = 0; i < instance.VisualSensors.Count; i++)
            {
                VisualSensor curr = instance.VisualSensors[i];

                Handles.color = curr.SensorColor;

                Vector3 Forward = Quaternion.Euler(curr.RotationOffset) * instance.transform.forward;

                Vector3 Normal = Quaternion.Euler(curr.RotationOffset) * instance.transform.up;

                Vector3 Origin = instance.transform.position + instance.transform.TransformDirection(curr.PositionOffset);

                Vector3 L = Origin + ((Quaternion.AngleAxis(-curr.Angle * 0.5f, Normal) * Forward).normalized * curr.Distance);

                Vector3 R = Origin + ((Quaternion.AngleAxis(curr.Angle * 0.5f, Normal) * Forward).normalized * curr.Distance);

                Handles.DrawLine(Origin, L);
                Handles.DrawLine(Origin, R);

                Handles.DrawWireArc(Origin, Normal, (L - Origin).normalized , curr.Angle * 0.5f, curr.Distance);
                Handles.DrawWireArc(Origin, Normal, (R - Origin).normalized , -curr.Angle * 0.5f, curr.Distance);

                Vector3 Normal_right = Quaternion.Euler(curr.RotationOffset) * instance.transform.right;

                Vector3 L_right = Origin + ((Quaternion.AngleAxis(-curr.Angle * 0.5f, Normal_right) * Forward).normalized * curr.Distance);

                Vector3 R_right = Origin + ((Quaternion.AngleAxis(curr.Angle * 0.5f, Normal_right) * Forward).normalized * curr.Distance);

                Handles.DrawLine(Origin, L_right);

                Handles.DrawLine(Origin, R_right);

                Handles.DrawWireArc(Origin, Normal_right, (L_right - Origin).normalized, curr.Angle * 0.5f, curr.Distance);
                Handles.DrawWireArc(Origin, Normal_right, (R_right - Origin).normalized, -curr.Angle * 0.5f, curr.Distance);


                Vector3 center_raidus = (L + R + L_right + R_right) / 4;

                Handles.DrawWireDisc(center_raidus, Forward, (L - R).magnitude * 0.5f);


            }


        }

    }
}