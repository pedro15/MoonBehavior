using UnityEngine;
using UnityEditor;

namespace MoonBehaviorEditor
{
    /// <summary>
    /// Resources and settings loading class
    /// </summary>
    public static class MoonResources
    {
        #region Public APi 

        /// <summary>
        /// Gets the node style
        /// </summary>
        /// <param name="colorindex">Node color index</param>
        /// <param name="on">Is selected ?</param>
        /// <returns>Node style</returns>
        public static GUIStyle GetNodeStyle(bool on, int colorindex)
        {
            if (colorindex < 0 | colorindex > 6) return null;
            
            string stylestr = "flow node " + colorindex.ToString() + (on ? " on" : string.Empty);

            return GUI.skin.FindStyle(stylestr);
        }

        /// <summary>
        /// Stored grid texture
        /// </summary>
        private static Texture2D gridTexture;

        /// <summary>
        /// Grid texture background
        /// </summary>
        public static Texture2D GridTexture
        {
            get
            {
                if (!gridTexture)
                {
                    gridTexture = LoadCustomResource<Texture2D>("Core/MoonGrid"); 
                }
                return gridTexture;
            }
        }

        /// <summary>
        /// Width used for generate grid
        /// </summary>
        private static int width { get { return 120; } }
        /// <summary>
        /// height used for generate grid
        /// </summary>
        private static int height { get { return 120; } }
        
        /// <summary>
        /// grid step used for generate grid
        /// </summary>
        private static Vector2 step { get { return new Vector2(width / 10, height / 10); } }
        
        /// <summary>
        /// Generates Handles line texture
        /// </summary>
        /// <returns>The line pixel texture (1x2 px) white and transparent colors</returns>
        public static Texture2D GenerateLineText()
        {
            Color transparentw = new Color(1, 1, 1, 0.1f);
            Color Opaquew = Color.white;

            Color[] pix = new Color[] { transparentw, Opaquew };

            Texture2D text = new Texture2D(1, 2, TextureFormat.RGBA32, true);

            text.SetPixels(pix);

            text.Apply(true);

            return text;
        }

        /// <summary>
        /// Stored dot connection style
        /// </summary>
        private static GUIStyle dotStyle = null;

        /// <summary>
        /// Dot connection style
        /// </summary>
        public static GUIStyle DotStyle
        {
            get
            {
                if (dotStyle == null || !dotStyle.normal.background  )
                {
                    dotStyle = new GUIStyle()
                    {
                        normal = new GUIStyleState()
                        {
                            background = LoadCustomResource<Texture2D>("Core/Moon_Node_Dot")
                        }
                    };
                    dotStyle.padding = new RectOffset(12, 12, 12, 12);
                }
                return dotStyle;
            }
        }

        /// <summary>
        /// Stored Arrow connection style
        /// </summary>
        private static GUIStyle arrowStyle = null; 

        /// <summary>
        /// Arrow connection style
        /// </summary>
        public static GUIStyle ArrowStyle
        {
            get
            {
                if (arrowStyle == null || !arrowStyle.normal.background )
                {
                    arrowStyle = new GUIStyle()
                    {
                        normal = new GUIStyleState()
                        {
                            background = LoadCustomResource<Texture2D>("Core/Moon_Arrow")
                        }
                    };
                    arrowStyle.padding = new RectOffset(6, 6, 12, 12);
                }

                return arrowStyle;
            }
        }

        /// <summary>
        /// Stored label order style
        /// </summary>
        private static GUIStyle labelorderStyle = null;  

        /// <summary>
        /// Label order style
        /// </summary>
        public static GUIStyle LabelOrderStyle
        {
            get
            {
                if (labelorderStyle == null)
                {
                    labelorderStyle = new GUIStyle()
                    {
                        normal = new GUIStyleState()
                        {
                            background = LoadCustomResource<Texture2D>("Core/Moon_Node_Order")
                        },
                        alignment = TextAnchor.MiddleCenter,
                        fontSize = 10

                    };
                    labelorderStyle.padding = new RectOffset(12, 12, 6, 6);
                    arrowStyle.stretchWidth = true; 
                }
                return labelorderStyle;
            }
        }

        

        /// <summary>
        /// Loads an custom icon
        /// </summary>
        /// <param name="IconName">Icon name</param>
        /// <returns>Texture icon</returns>
        public static Texture2D LoadIcon(string IconName)
        {
            return LoadCustomResource<Texture2D>("Icons/Moon_" + IconName);
        }

        /// <summary>
        /// Loads custom Resource located on: 
        /// Resources/MoonBehavior/
        /// </summary>
        /// <typeparam name="T">Resource Type</typeparam>
        /// <param name="path">Resource path relative to Resources/MoonBehavior</param>
        /// <returns>Resource instance</returns>
        public static T LoadCustomResource<T>( string path ) where T : Object
        {
            return Resources.Load<T>("MoonBehavior/" + path);
        }

        #endregion
    }
}
