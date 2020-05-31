using UnityEngine;
using UnityEditor;
using System.Text;

namespace MoonBehaviorEditor.Core
{
    /// <summary>
    /// Help window
    /// </summary>
    internal class MoonHelpWindow : EditorWindow
    {
        /// <summary>
        /// Current version
        /// </summary>
        public const string MoonVersion = "1.5";

        // Help window textures
        private static Texture2D SupportIcon, RateIcon, ForumIcon;

        /// <summary>
        /// Loads and display the Help Window
        /// </summary>
        public static void Load()
        {
            Vector2 size = new Vector2(300, 180);
            MoonHelpWindow hw = GetWindow<MoonHelpWindow>();

            hw.titleContent.text = "Help";
            hw.maxSize = size;
            hw.minSize = size;

            hw.ShowUtility();
            hw.Focus();
        }
        
        [MenuItem("Window/MoonBehavior/Help" , priority = 2)]
        private static void Display()
        {
            Load();
        }

        [MenuItem("Window/MoonBehavior/About", priority = 3)]
        private static void DisplayAbout()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("MoonBehavior v" + MoonVersion);
            builder.AppendLine();
            builder.Append("Developed by Pedro Josue Duran Medina");
            builder.AppendLine();
            builder.Append("(Devil Kind Games)");
            EditorUtility.DisplayDialog("About", builder.ToString(), "Ok");
        }

        /// <summary>
        /// Inicializes the HelpWindow Icons.
        /// </summary>
        private static void Init()
        {
            SupportIcon = MoonResources.LoadIcon("SupportIcon");
            RateIcon = MoonResources.LoadIcon("FavoriteIcon");
            ForumIcon = MoonResources.LoadIcon("ForumsIcon");
        }
        
        private void OnGUI()
        {
            DoGUI(new Rect(0, 0, Screen.width, Screen.height),true);
        }

        /// <summary>
        /// Draws help window GUI
        /// </summary>
        /// <param name="BaseRect">Base rect to draw</param>
        /// <param name="nativewindow">Are drawing on native window?</param>
        private static void DoGUI(Rect BaseRect, bool nativewindow)
        {

            if (!SupportIcon | !RateIcon | !ForumIcon)
            {
                Init();
            }

            float w = BaseRect.width - 50; 
            GUI.BeginGroup(BaseRect);

            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

            GUI.Label(new Rect(10, 10, BaseRect.width , 30), (nativewindow? "" : "<b>Welcome to</b> ") + "<b>MoonBehavior</b> <size=10>v" + MoonVersion + "</size>",
                new GUIStyle(EditorStyles.label)
                {
                    richText = true,
                    fontSize = 15,
                    fontStyle = FontStyle.Normal,
                    alignment = TextAnchor.MiddleCenter
                });

            
            GUI.DrawTexture(new Rect(5, 40, 30, 30), RateIcon);

            if (GUI.Button(new Rect(40,40, w,35) , "Rate MoonBehavior"))
            {
                UnityEditorInternal.AssetStore.Open("/content/102731");
            }


            GUI.DrawTexture(new Rect(5, 80, 30, 30), ForumIcon);
            if (GUI.Button(new Rect(40, 80, w, 35), "Forum post"))
            {
                Application.OpenURL("https://forum.unity.com/threads/released-moonbehavior-behavior-trees-for-unity.504379/");
            }
            
            GUI.DrawTexture(new Rect(5, 120, 30, 30), SupportIcon);
            if (GUI.Button(new Rect(40, 120, w, 35), "Support"))
            {
                Application.OpenURL("mailto:devilkindgames@gmail.com");
            }
            
            GUI.EndGroup();
        }

    }
}
