using UnityEditor;
using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehaviorEditor.Core.Inspector
{
    /// <summary>
    /// Custom drawer for memory item class..
    /// </summary>
    [CustomPropertyDrawer(typeof(MemoryItem))]
    class MemoryItemDrawer : PropertyDrawer
    {
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.Box(new Rect(position.position , new Vector2(position.size.x , 
                position.size.y - 5)), "" , EditorStyles.helpBox);

            GUI.Box(new Rect(position.x + 1, position.y, position.width - 2, 20 ) , "" , EditorStyles.toolbar);

            Rect Label_rect = new Rect(position.x + 5, position.y + 2, position.width - 125 , 16);
            


            EditorGUI.LabelField(Label_rect, label , EditorStyles.label);
            
            
            SerializedProperty type = property.FindPropertyRelative("type");

            SerializedProperty predefined = property.FindPropertyRelative("_PredefinedType");

            if (!predefined.boolValue)
            {
                Rect Type_rect = new Rect(position.width - 100, position.y , 100, 16);

                type.enumValueIndex = EditorGUI.Popup(Type_rect, type.enumValueIndex,
                    System.Enum.GetNames(typeof(ItemType)) , EditorStyles.toolbarDropDown);
            }

            SerializedProperty NoMemory = property.FindPropertyRelative("constantOnly");
            Rect constant_rect = new Rect(position.x + 5, Label_rect.yMax + 5, 70, 16);
            Rect constantValue_rect = new Rect(constant_rect.xMax + 2, constant_rect.y, 15, 15);
            SerializedProperty constant = property.FindPropertyRelative("IsConstant");

            EditorGUI.BeginDisabledGroup(NoMemory.boolValue);

            GUI.Label(constant_rect, "Constant ?");
            constant.boolValue = EditorGUI.Toggle(constantValue_rect, constant.boolValue);

            if (NoMemory.boolValue)
            {
                if (!constant.boolValue)
                    constant.boolValue = true; 
            }

            EditorGUI.EndDisabledGroup();

            Rect LabelValue_rect = new Rect(constantValue_rect.xMax, constantValue_rect.y, 35, 15);

            Rect Value_rect = new Rect(LabelValue_rect.xMax + 5, constantValue_rect.y,
                 (position.width - (LabelValue_rect.xMax + 5)), 16);


            GUI.Label(LabelValue_rect, (constant.boolValue ? "Value" : "key"));

            if (constant.boolValue)
            {
                ItemType m_type = (ItemType)type.enumValueIndex;

                switch (m_type)
                {
                    case ItemType.BOOLEAN:

                        SerializedProperty BoolValue = property.FindPropertyRelative("BoolValue");

                        BoolValue.boolValue = EditorGUI.Toggle(Value_rect, BoolValue.boolValue);

                        break;
                    case ItemType.STRING:

                        SerializedProperty StringValue = property.FindPropertyRelative("StringValue");

                        StringValue.stringValue = EditorGUI.TextField(Value_rect, StringValue.stringValue);
                        
                        break;
                    case ItemType.FLOAT:

                        SerializedProperty floatValue = property.FindPropertyRelative("floatValue");

                        floatValue.floatValue = EditorGUI.FloatField(Value_rect, floatValue.floatValue);

                        break;
                    case ItemType.INT:

                        SerializedProperty intValue = property.FindPropertyRelative("intValue");

                        intValue.intValue = EditorGUI.IntField(Value_rect, intValue.intValue);

                        break;
                    case ItemType.VECTOR2:

                        SerializedProperty Vector2Value = property.FindPropertyRelative("Vector2Value");

                        Vector2Value.vector2Value = EditorGUI.Vector2Field(Value_rect, string.Empty,
                            Vector2Value.vector2Value);
                        
                        break;
                    case ItemType.VECTOR3:

                        SerializedProperty Vector3Value = property.FindPropertyRelative("Vector3Value");

                        Vector3Value.vector3Value = EditorGUI.Vector3Field(Value_rect, string.Empty,
                            Vector3Value.vector3Value);

                        break;
                    case ItemType.VECTOR4:

                        SerializedProperty Vector4Value = property.FindPropertyRelative("Vector4Value");

                        Vector4Value.vector4Value = EditorGUI.Vector4Field(Value_rect, string.Empty, 
                            Vector4Value.vector4Value);

                        break;
                    case ItemType.COLOR:

                        SerializedProperty ColorValue = property.FindPropertyRelative("ColorValue");

                        ColorValue.colorValue = EditorGUI.ColorField(Value_rect, ColorValue.colorValue);

                        break;
                    case ItemType.OBJECT:

                        SerializedProperty objectValue = property.FindPropertyRelative("objectValue");

                        objectValue.objectReferenceValue = EditorGUI.ObjectField(Value_rect, string.Empty,
                            objectValue.objectReferenceValue, typeof(Object), false);

                        break;
                    case ItemType.LAYERMASK:

                        SerializedProperty LayerValue = property.FindPropertyRelative("LayerValue");

                        EditorGUI.PropertyField(Value_rect, LayerValue, GUIContent.none, true);
                        
                        break;
                }

            }else
            {
                SerializedProperty Key = property.FindPropertyRelative("Key");
                Key.stringValue = EditorGUI.TextField(Value_rect, Key.stringValue);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 50f;
        }
    }
}
