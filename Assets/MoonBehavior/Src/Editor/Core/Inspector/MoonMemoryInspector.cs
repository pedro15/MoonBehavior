using System.Collections;
using System.Reflection;

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using MoonBehavior.Memory;

namespace MoonBehaviorEditor.Core.Inspector
{
    /// <summary>
    /// Inspector for Memory component
    /// </summary>
    [CustomEditor (typeof(MoonMemory))]
    class MoonMemoryInspector : Editor
    {

        /// <summary>
        /// Memory List Items Property
        /// </summary>
        SerializedProperty MemoryItems;

        /// <summary>
        /// Nice Reorderable list for displaying MemoryItem
        /// </summary>
        ReorderableList MemoryList;

        /// <summary>
        /// Memory field for debugging.
        /// </summary>
        FieldInfo memoryfield;
        
        private void OnEnable()
        {

            MemoryItems = serializedObject.FindProperty("MemoryItems");
            memoryfield = MoonReflection.GetField("memory", target);
            
            MemoryList = new ReorderableList(serializedObject, MemoryItems, true, true, true, true);
            MemoryList.elementHeight = 50;
            MemoryList.drawHeaderCallback = (Rect r) =>
            {
                EditorGUI.LabelField(r, new GUIContent("Memory Elements", "User pre-defined memory elements"));
            };

            MemoryList.drawElementCallback = (Rect rect, int index, bool isactive, bool isfocused) =>
            {
                SerializedProperty current = MemoryItems.GetArrayElementAtIndex(index);

                Rect toprect = new Rect(rect.x, rect.y + 2, rect.width, 20);

                SerializedProperty type = current.FindPropertyRelative("type");

                ItemType itype = (ItemType)type.enumValueIndex;

                // key gui

                GUI.BeginGroup(toprect);


                GUIContent labelcont = new GUIContent(MemoryItem.ConvertType(itype).Name);

                float w = GUI.skin.FindStyle("AssetLabel").CalcSize(labelcont).x;

                GUI.Label(new Rect(0, 0, w, 16), labelcont, GUI.skin.FindStyle("AssetLabel"));

                GUI.Label(new Rect(w + 5, 0, 30, 16), "key:");

                SerializedProperty key = current.FindPropertyRelative("Key");

                key.stringValue = EditorGUI.TextField(new Rect(w + 40, 0, toprect.width - (w + 40), 16), key.stringValue);

                GUI.EndGroup();

                // value gui

                Rect buttunrect = new Rect(rect.x, toprect.yMax + 2, rect.width, 20);

                GUI.BeginGroup(buttunrect);

                GUI.Label(new Rect(0, 0, 50, 15), "Value");

                Rect ValueRect = new Rect(60, 0, buttunrect.width - 60, 16);

                switch (itype)
                {
                    case ItemType.BOOLEAN:

                        SerializedProperty BoolValue = current.FindPropertyRelative("BoolValue");

                        BoolValue.boolValue = EditorGUI.Toggle(new Rect(ValueRect.position , Vector2.one * 16), BoolValue.boolValue);

                        break;

                    case ItemType.STRING:

                        SerializedProperty StringValue = current.FindPropertyRelative("StringValue");

                        StringValue.stringValue = EditorGUI.TextField(ValueRect, StringValue.stringValue);

                        break;

                    case ItemType.FLOAT:

                        SerializedProperty floatValue = current.FindPropertyRelative("floatValue");

                        floatValue.floatValue = EditorGUI.FloatField(ValueRect, floatValue.floatValue);

                        break;
                    case ItemType.INT:

                        SerializedProperty intValue = current.FindPropertyRelative("intValue");

                        intValue.intValue = EditorGUI.IntField(ValueRect, intValue.intValue);

                        break;
                    case ItemType.VECTOR2:

                        SerializedProperty Vector2Value = current.FindPropertyRelative("Vector2Value");

                        Vector2Value.vector2Value = EditorGUI.Vector2Field(ValueRect, "", Vector2Value.vector2Value);

                        break;
                    case ItemType.VECTOR3:

                        SerializedProperty Vector3Value = current.FindPropertyRelative("Vector3Value");

                        Vector3Value.vector3Value = EditorGUI.Vector3Field(ValueRect, "", Vector3Value.vector3Value);

                        break;

                    case ItemType.VECTOR4:

                        SerializedProperty Vector4Value = current.FindPropertyRelative("Vector4Value");

                        Vector4Value.vector4Value = EditorGUI.Vector4Field(ValueRect, "", Vector4Value.vector4Value);

                        break;

                    case ItemType.COLOR:

                        SerializedProperty ColorValue = current.FindPropertyRelative("ColorValue");

                        ColorValue.colorValue = EditorGUI.ColorField(ValueRect, ColorValue.colorValue);

                        break;

                    case ItemType.OBJECT:

                        SerializedProperty objectValue = current.FindPropertyRelative("objectValue");

                        objectValue.objectReferenceValue = EditorGUI.ObjectField(ValueRect, "",
                            objectValue.objectReferenceValue, typeof(Object), true);

                        break;

                    case ItemType.LAYERMASK:

                        SerializedProperty LayerValue = current.FindPropertyRelative("LayerValue");

                        EditorGUI.PropertyField(ValueRect, LayerValue, GUIContent.none, true);

                        break;
                }


                GUI.EndGroup();

            };

            MemoryList.onAddDropdownCallback = (Rect btnrect, ReorderableList list) =>
            {
                int targetindex = list.serializedProperty.arraySize;

                GenericMenu menu = new GenericMenu();
                string[] opcs = System.Enum.GetNames(typeof(ItemType));

                for (int i = 0; i < opcs.Length; i++)
                {
                    string elementName = opcs[i];
                    menu.AddItem(new GUIContent(elementName), false, new GenericMenu.MenuFunction(() =>
                    {
                        list.serializedProperty.InsertArrayElementAtIndex(targetindex);
                        serializedObject.ApplyModifiedProperties();
                        SerializedProperty current = MemoryItems.GetArrayElementAtIndex(targetindex);
                        SerializedProperty type = current.FindPropertyRelative("type");

                        for (int j = 0; j < type.enumNames.Length; j++)
                        {
                            if (string.Equals(elementName, type.enumNames[j]))
                            {
                                list.serializedProperty.serializedObject.Update();
                                type.enumValueIndex = j;
                                current.FindPropertyRelative("Key").stringValue = string.Empty;
                                list.serializedProperty.serializedObject.ApplyModifiedProperties();
                                break;
                            }
                        }
                    }));
                }
                menu.DropDown(btnrect);
            };

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();

            if (!EditorApplication.isPlaying)
            {
                
                MemoryList.DoLayoutList();
                EditorGUILayout.Space();
            }
            else
            {
                Hashtable hash = memoryfield.GetValue(target) as Hashtable;

                EditorGUILayout.BeginVertical(GUI.skin.box);

                MoonGUILayout.CenterLabel("Memory");
                EditorGUILayout.Space();

                if (hash != null && hash.Values.Count > 0)
                {
                    object[] Labels = new object[hash.Keys.Count];

                    hash.Keys.CopyTo(Labels, 0);

                    for (int i = 0; i < Labels.Length; i++)
                    {
                        Rect h = EditorGUILayout.BeginHorizontal();

                        if (Labels[i] == null) continue;


                        GUILayout.Label(Labels[i].ToString() + ":", GUILayout.Width(EditorStyles.label.CalcSize(
                            new GUIContent(Labels[i].ToString() + ":")).x));

                        object val = hash[Labels[i]];

                        if (val != null)
                        {
                            if (val.GetType() == typeof(Color))
                            {
                                Color oldcolor = GUI.color;
                                GUI.color = Color.white;

                                Rect r = GUILayoutUtility.GetRect(h.width - 5, 15);
                                EditorGUI.DrawRect(r, (Color)val);

                                GUI.color = oldcolor;
                            }
                            else
                            {
                                GUILayout.Label(val.ToString());
                            }
                        }
                        else
                        {
                            GUILayout.Label("null");
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();

                }
                else
                {
                    EditorGUILayout.LabelField("No memory elements found !");
                }

                EditorGUILayout.EndVertical();

                Repaint();
            }
            serializedObject.ApplyModifiedProperties();
        }

    }
}
