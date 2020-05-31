using UnityEngine;
using UnityEditor;

namespace MoonBehaviorEditor
{
    /// <summary>
    /// GUI extra methods for auto positioning
    /// </summary>
    public static class MoonGUILayout
    {

        /// <summary>
        /// Draws a label centered
        /// </summary>
        /// <param name="cont">Label content</param>
        /// <param name="options">Options</param>
        public static void CenterLabel(GUIContent cont , params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            GUILayout.Label(cont, options);

            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws a label centered
        /// </summary>
        /// <param name="label">label text</param>
        /// <param name="options">Options</param>
        public static void CenterLabel(string label , params GUILayoutOption[] options)
        {
            CenterLabel(new GUIContent(label), options);
        }

        /// <summary>
        /// content separator
        /// </summary>
        public static void Separator()
        {
            GUILayout.Space(5);

            Color scolor = Color.black;

            if (EditorGUIUtility.isProSkin)
            {
                scolor = new Color(0.12f, 0.12f, 0.12f, 1.333f);
            }
            else
            {
                scolor = new Color(0.55f, 0.55f, 0.55f, 1.333f);
            }

            Rect r = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandWidth(true));
            r.height = 2f;

            EditorGUI.DrawRect(r, scolor);

            GUILayout.Space(5);
        }

        /// <summary>
        /// Draws an inspector ignoring the "script" field
        /// </summary>
        /// <param name="obj">Serialized object</param>
        /// <returns>Inspector changed state</returns>
        public static bool DrawDefaultInspectorNoScript(SerializedObject obj)
        {
            EditorGUI.BeginChangeCheck();
            obj.Update();
            SerializedProperty iterator = obj.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                if (!string.Equals("m_Script", iterator.propertyPath))
                {
                   EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);
                }

                enterChildren = false;
            }
            obj.ApplyModifiedProperties();
            return EditorGUI.EndChangeCheck();
        }
    }
}
