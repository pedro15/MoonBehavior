using UnityEditor;
using UnityEngine;
using MoonBehaviorEditor.Core.Graphs;

namespace MoonBehaviorEditor.Core
{
    [System.Serializable]
    public class MoonEditorWindow : EditorWindow,IHasCustomMenu
    {

        #region Window loading and instance
        
        public static MoonEditorWindow LoadWindow()
        {
            MoonEditorWindow instance = GetWindow<MoonEditorWindow>();
            instance.titleContent.text = "Moon Behavior";

            if (MoonSettings.HelpOnStartUp)
            {
                MoonHelpWindow.Load();
            }

            return instance;
        }

        [MenuItem("Window/MoonBehavior/Editor" , priority = 0)]
        private static void DisplayWindow()
        {
            LoadWindow();
        }
        
        #endregion

        /// <summary>
        //  Stored current graph instance
        /// </summary>
        [SerializeField]
        private NodeGraph currentGraph ;
        
        /// <summary>
        /// Working Graph workspace
        /// </summary>
        public NodeGraph CurrentGraph
        {
            get
            {
                return currentGraph;
            }
            
            set
            {
                if (value != currentGraph)
                {
                    currentGraph = value;
                    if (value != null)
                    {
                        EditorUtility.SetDirty(currentGraph);

                        if (controller != null)
                            controller.Init(this);

                    }
                }
            }
        }
        

        /// <summary>
        /// controller instance
        /// </summary>
        [SerializeField]
        private MoonEditorController controller; 

        /// <summary>
        /// current controller
        /// </summary>
        public MoonEditorController Controller
        {
            get
            {
                return controller;
            }
        }

        private void OnEnable()
        {
            if (controller == null )
            {
                controller = new MoonEditorController();
            }
            controller.Init(this);
        }

        private void OnGUI()
        {
            controller.DoMenu();
            controller.DoEditor(Event.current);
        }

        private void Update()
        {
            wantsMouseMove = true;
            Repaint();
        }

        private void OnDestroy()
        {
            if (Selection.activeObject != null)
            {
                if (Selection.activeObject.GetType().BaseType == typeof(Node))
                {
                    Selection.activeObject = null;
                }
            }
        }

        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Add New tab"), false, () => 
            {
                MoonEditorWindow wind = Instantiate(this);
                wind.CurrentGraph = null;
                wind.Show();
            });
        }
    }
}
