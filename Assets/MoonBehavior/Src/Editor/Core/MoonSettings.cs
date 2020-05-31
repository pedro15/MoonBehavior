using UnityEditor;
using UnityEngine;

namespace MoonBehaviorEditor
{
    /// <summary>
    /// User preferences settings
    /// </summary>
    public static class MoonSettings 
    {
        /// <summary>
        /// Graphs directory EditorPrefs key
        /// </summary>
        const string GraphsDirKey = "MoonBehavior_GraphsDir";
        
        /// <summary>
        /// Actions directory EditorPrefs key
        /// </summary>
        const string ActionsDirKey = "MoonBehavior_ActionsDir";

        /// <summary>
        /// Help on startUp EditorPrefs Key
        /// </summary>
        const string HelpOnStartUpKey = "MoonBehavior_HelpOnStartUp";

        /// <summary>
        /// Are the user settings loaded ?
        /// </summary>
        static bool SettingsLoaded = false;

        /// <summary>
        /// Stored Graphs directory
        /// </summary>
        private static string _GraphsDir;
        
        /// <summary>
        /// Stored Actions directory;
        /// </summary>
        private static string _ActionsDir;

        /// <summary>
        /// Stored help on startup user preference
        /// </summary>
        private static bool _HelpOnStartUp; 

        /// <summary>
        /// Graphs save directory Path relative to "Assets" folder
        /// </summary>
        public static string GraphsSaveDirectory
        {
            get
            {
                DoLoadSettings();
                return _GraphsDir;
            }
        }
        
        /// <summary>
        /// Actions save directory path relative to "Assets" folder
        /// </summary>
        public static string ActionsSaveDirectory
        {
            get
            {
                DoLoadSettings();

                return _ActionsDir; 
            }
        }

        /// <summary>
        /// Should show help window on Open MoonBehavior window ? 
        /// </summary>
        public static bool HelpOnStartUp
        {
            get
            {
                DoLoadSettings();

                return _HelpOnStartUp;
            }
        }

        /// <summary>
        /// Check loaded settings state and loads the user settings depending on the state.
        /// </summary>
        static void DoLoadSettings()
        {
            if (!SettingsLoaded)
            {
                LoadSettings();
            }
        }
        
        /// <summary>
        /// Settings user preferences Item
        /// </summary>
        [PreferenceItem("MoonBehavior")]
        static void SettingsGUI()
        {
            DoLoadSettings();

            OnSettingsGUI();
        }

        /// <summary>
        /// Settings GUI
        /// </summary>
        static void OnSettingsGUI()
        {
            GUILayout.Space(5);

            GUILayout.Label("Exports Save Directory");

            if (GUILayout.Button("Select...", GUILayout.Width(75)))
            {
                string selected = SelectDirectory();
                if (!string.IsNullOrEmpty(selected))
                {
                    _GraphsDir = selected;
                }
                else
                {
                    Debug.LogWarning("[MoonBehavior] Invalid directory selected");
                }
            }

            GUILayout.Box(_GraphsDir, EditorStyles.helpBox);
            
            GUILayout.Label("Actions Save Diectory");

            if (GUILayout.Button("Select...", GUILayout.Width(75)))
            {
                string selected = SelectDirectory();
                if (!string.IsNullOrEmpty(selected))
                {
                    _ActionsDir = selected;
                }
                else
                {
                    Debug.LogWarning("[MoonBehavior] Invalid directory selected");
                }
            }

            GUILayout.Box(_ActionsDir, EditorStyles.helpBox);

            MoonGUILayout.Separator();

            _HelpOnStartUp = EditorGUILayout.Toggle(new GUIContent("Display about window on Open" , 
                "Should display about window when open the editor?"), _HelpOnStartUp);

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Save", GUILayout.Width(75)))
            {
                SaveSettings();
            }

            if (GUILayout.Button("Reset", GUILayout.Width(75)))
            {
                DeleteSettings();
                LoadSettings();
            }
            
            GUILayout.EndHorizontal();

        }

        /// <summary>
        /// Select directory and returns Path relative to Assets Folder
        /// </summary>
        /// <returns>Directory path relative to assets folder or empty string if the path is invalid</returns>
        static string SelectDirectory()
        {
            string s = EditorUtility.OpenFolderPanel("Select Directoy" , "Assets" , "");
            s = s.Replace(Application.dataPath, "Assets");
            if (AssetDatabase.IsValidFolder(s))
            {
                return s; 
            }
            return string.Empty;
        }

        /// <summary>
        /// Loads user settings
        /// </summary>
        static void LoadSettings()
        {
            _GraphsDir = EditorPrefs.GetString(GraphsDirKey, "Assets/MoonBehavior/Exports");
            
            _ActionsDir = EditorPrefs.GetString(ActionsDirKey, "Assets/MoonBehavior/Actions");

            _HelpOnStartUp = EditorPrefs.GetBool(HelpOnStartUpKey, true);
            SettingsLoaded = true; 
        }

        /// <summary>
        /// Remove user settings
        /// </summary>
        static void DeleteSettings()
        {
            EditorPrefs.DeleteKey(GraphsDirKey);
            EditorPrefs.DeleteKey(ActionsDirKey);
            EditorPrefs.DeleteKey(HelpOnStartUpKey);
        }
        
        /// <summary>
        /// Saves user settings
        /// </summary>
        static void SaveSettings()
        {
            EditorPrefs.SetString(GraphsDirKey, _GraphsDir);
            EditorPrefs.SetString(ActionsDirKey, _ActionsDir);
            EditorPrefs.SetBool(HelpOnStartUpKey, _HelpOnStartUp);
        }
    }
}