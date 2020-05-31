using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using MoonBehavior.BehaviorTrees.Actions;
using MoonBehavior.Conditions;
using MoonBehavior.Conditions.Operations;

namespace MoonBehaviorEditor.Core.Inspector
{
    /// <summary>
    /// Inspector for condition node
    /// </summary>
    [CustomEditor(typeof(BTCondition))]
    public class ConditionInspector : Editor
    {
        /// <summary>
        /// Conditions list
        /// </summary>
        SerializedProperty Conditions;
        ReorderableList list;

        private void OnEnable()
        {
            try
            {
                Conditions = serializedObject.FindProperty("conditions");
            }catch { return; }

            list = new ReorderableList(serializedObject, Conditions, true, true, true, true);
            list.drawHeaderCallback = (Rect r) =>
            {
                EditorGUI.LabelField(r, "Conditions");
            };

            list.onAddDropdownCallback = (Rect r, ReorderableList l) =>
            {
                GenericMenu menu = new GenericMenu();

                BasicCondition.ConditionType m_type;

                int index = Conditions.arraySize;

                menu.AddItem(new GUIContent("Numeric"), false, () => 
                {
                    m_type = BasicCondition.ConditionType.Numeric;
                    InsertConditionElement(m_type, index);
                });

                menu.AddItem(new GUIContent("String"), false, () =>
                {
                    m_type = BasicCondition.ConditionType.String;
                    InsertConditionElement(m_type, index);
                });

                menu.AddItem(new GUIContent("Boolean"), false, () =>
                {
                    m_type = BasicCondition.ConditionType.Boolean;
                    InsertConditionElement(m_type, index);
                });

                menu.AddItem(new GUIContent("Object"), false, () =>
                {
                    m_type = BasicCondition.ConditionType.Object;
                    InsertConditionElement(m_type, index);
                });


                menu.ShowAsContext();
            };

            list.elementHeightCallback = (int index) =>
            {
                SerializedProperty curr = Conditions.GetArrayElementAtIndex(index);

                SerializedProperty currtype = curr.FindPropertyRelative("Type");

                BasicCondition.ConditionType mType = (BasicCondition.ConditionType)currtype.enumValueIndex;

                return mType == BasicCondition.ConditionType.Object ? 45f : 60f;
            };


            list.drawElementCallback = (Rect r, int index, bool active, bool focused) =>
            {
                SerializedProperty curr = Conditions.GetArrayElementAtIndex(index);

                SerializedProperty currtype = curr.FindPropertyRelative("Type");

                SerializedProperty aKey = curr.FindPropertyRelative("AKey");

                SerializedProperty bKey = curr.FindPropertyRelative("BKey");

                SerializedProperty bconstant = curr.FindPropertyRelative("BConstant");


                BasicCondition.ConditionType mType = (BasicCondition.ConditionType)currtype.enumValueIndex;

                EditorGUI.LabelField(new Rect(r.x, r.y + 2, 60, 16), mType.ToString(), GUI.skin.FindStyle("AssetLabel"));

                Rect PropRect = new Rect(r.x, r.y + 18, r.width, r.height);

                switch (mType)
                {
                    case BasicCondition.ConditionType.Boolean:
                        
                        SerializedProperty operation_bool = curr.FindPropertyRelative("op_bool");

                        SerializedProperty b_val = operation_bool.FindPropertyRelative("BValue");

                        SerializedProperty b_mode = operation_bool.FindPropertyRelative("Mode");
                        
                        DrawConditionProperty(index, PropRect,mType, aKey, bKey, bconstant, b_val, b_mode, 
                            typeof(ObjectOperation.OperationMode),curr);

                        return;

                    case BasicCondition.ConditionType.Numeric:

                        SerializedProperty operation_numeric = curr.FindPropertyRelative("op_numeric");

                        SerializedProperty n_val = operation_numeric.FindPropertyRelative("BValue");

                        SerializedProperty n_mode = operation_numeric.FindPropertyRelative("Mode");

                        DrawConditionProperty(index, PropRect, mType, aKey, bKey, bconstant, n_val, n_mode,
                            typeof(NumericOperation.NumericOperationMode),curr);
                        
                        return;
                    case BasicCondition.ConditionType.String:

                        SerializedProperty operation_string = curr.FindPropertyRelative("op_string");

                        SerializedProperty s_val = operation_string.FindPropertyRelative("BValue");

                        SerializedProperty s_mode = operation_string.FindPropertyRelative("Mode");

                        DrawConditionProperty(index, PropRect, mType, aKey, bKey, bconstant, s_val, s_mode, 
                            typeof(ObjectOperation.OperationMode),curr);

                        return;

                    case BasicCondition.ConditionType.Object:

                        SerializedProperty operation_object = curr.FindPropertyRelative("op_obj");

                        SerializedProperty o_mode = operation_object.FindPropertyRelative("Mode");

                        DrawConditionProperty(index, PropRect, mType, aKey, bKey, bconstant, null, o_mode , 
                            typeof(ObjectOperation.OperationMode),curr);

                        return;
                }

                

            };


        }

        /// <summary>
        /// Draws property for an condition type
        /// </summary>
        /// <param name="BaseRect">Draw rect</param>
        /// <param name="ctype">Condition type</param>
        /// <param name="AKey">A Memory Key</param>
        /// <param name="BKey">B Memory Key</param>
        /// <param name="BConstant">B Constant value</param>
        /// <param name="BValue">B value</param>
        /// <param name="Mode">Mode property</param>
        /// <param name="ModeType">Mode enum type</param>
        private void DrawConditionProperty(int index,Rect BaseRect, BasicCondition.ConditionType ctype , SerializedProperty AKey, SerializedProperty BKey,
            SerializedProperty BConstant , SerializedProperty BValue , SerializedProperty Mode , System.Type ModeType , SerializedProperty currentElement)
        {
            GUI.BeginGroup(BaseRect);

            Rect r_A_label = new Rect(0, 2, 30, 16);

            EditorGUI.LabelField(r_A_label, "Key");

            Rect r_A_Key = new Rect(r_A_label.xMax + 2, 2, (BaseRect.width * 0.35f) - 25f, 16);

            AKey.stringValue = EditorGUI.TextField(r_A_Key, AKey.stringValue);

            Rect r_Mode = new Rect(r_A_Key.xMax + 2, 2, (BaseRect.width * 0.25f), 16);

            Mode.enumValueIndex = EditorGUI.Popup(r_Mode, Mode.enumValueIndex, System.Enum.GetNames(ModeType));

            bool con = BConstant.boolValue && ctype != BasicCondition.ConditionType.Object;

            Rect r_B_Label = new Rect(r_Mode.xMax + 2, 2, (con ? 35 : 30) , 16);

            EditorGUI.LabelField(r_B_Label, con ? "Value" : "Key" );

            float w = r_A_Key.width + r_A_label.width + r_Mode.width + 10;
            Rect r_BValue = new Rect( r_B_Label.xMax + 2, r_Mode.y, BaseRect.width - w , 16);
            
            if (!BConstant.boolValue || ctype == BasicCondition.ConditionType.Object)
            {
                BKey.stringValue = EditorGUI.TextField(r_BValue, BKey.stringValue);
            }else
            {
                if (ctype == BasicCondition.ConditionType.Boolean)
                {
                    BValue.boolValue = EditorGUI.Toggle(new Rect(r_BValue.x , r_BValue.y,16,16), BValue.boolValue);
                }else if (ctype == BasicCondition.ConditionType.Numeric)
                {
                    BValue.floatValue = EditorGUI.FloatField(r_BValue, BValue.floatValue);
                }else if (ctype == BasicCondition.ConditionType.String)
                {
                    BValue.stringValue = EditorGUI.TextField(r_BValue, BValue.stringValue);
                }
            }

            Rect r_bconstant_label = new Rect(r_B_Label.x, r_B_Label.yMax + 2, 60, 16);
            Rect r_constant = new Rect(r_bconstant_label.xMax + 2, r_bconstant_label.y, 16, 16);

            if (ctype != BasicCondition.ConditionType.Object)
            {

                EditorGUI.LabelField(r_bconstant_label, "Constant");


                BConstant.boolValue = EditorGUI.Toggle(r_constant, BConstant.boolValue);
            }

            Rect Operator_rect = new Rect(r_Mode.x , r_Mode.yMax + 2 , r_Mode.width , r_Mode.height);
            
            SerializedProperty Operator = currentElement.FindPropertyRelative("Operator");

            if (index != 0)
            {
                //Rect Operator_label = new Rect(r_A_Key.xMax - 60, r_A_Key.yMax + 2, 60, 16);
                //EditorGUI.LabelField(Operator_label, "Operator");
                Operator.enumValueIndex = EditorGUI.Popup(Operator_rect, string.Empty, Operator.enumValueIndex,
                    System.Enum.GetNames(typeof(MoonCondition.ConditionOperator)));
            }else if (Operator.enumValueIndex != 0)
            {
                Operator.enumValueIndex = 0;
            }

            GUI.EndGroup();
        }

        /// <summary>
        /// Insert a condition element on the condition list based on the type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        private void InsertConditionElement(BasicCondition.ConditionType type , int index)
        {
            Conditions.InsertArrayElementAtIndex(index);
            SerializedProperty prop = Conditions.GetArrayElementAtIndex(index);
            SerializedProperty propt = prop.FindPropertyRelative("Type");
            propt.enumValueIndex = (int)type;

            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            MoonGUILayout.DrawDefaultInspectorNoScript(serializedObject);

            serializedObject.Update();

            GUILayout.Space(5);

            list.DoLayoutList();


            serializedObject.ApplyModifiedProperties();

        }

    }
}