using UnityEngine;
using UnityEditor;

using MoonBehaviorEditor.Core.Graphs;
using MoonBehaviorEditor.Modals;

using System.Linq;
using System.Collections.Generic;

namespace MoonBehaviorEditor.Core
{
    /// <summary>
    /// Editor controller that handles all base events and calls
    /// </summary>
    [System.Serializable]
    public class MoonEditorController
    {
        private MoonEditorWindow _window;
        public MoonEditorWindow Window { get { return _window; } }

        public enum GUIState : int
        {
            Normal = 0,
            New = 1,
            Open = 2,
            SaveAs = 3
        }

        /// <summary>
        /// Default controller constructor
        /// </summary>
        public MoonEditorController()
        {
            canvas = new MoonEditorCanvas();
        }

        /// <summary>
        /// Initializes the controller
        /// </summary>
        public void Init( MoonEditorWindow Window )
        {
            _window = Window;
            canvas.Init(Window);
            CurrentGUIState = 0;
            InitStates();
        }

        private void InitStates()
        {
            if (WorkingGraph != null)
            {
                UserStates = WorkingGraph.CanvasGUIStates(this);
                UserKeyStates = new int[UserStates.Keys.Count];
                UserStates.Keys.CopyTo(UserKeyStates, 0);
            }
        }
        
        /// <summary>
        /// Working NodeGraph 
        /// </summary>
        public NodeGraph WorkingGraph
        {
            get
            {
                return Window.CurrentGraph;
            }

            set
            {
                Window.CurrentGraph = value;
            }
        }

        /// <summary>
        /// User Defined GUI States
        /// </summary>
        Dictionary<int, System.Action> UserStates = new Dictionary<int, System.Action>();

        /// <summary>
        /// User Defined GUI States Keys
        /// </summary>
        int[] UserKeyStates;

        /// <summary>
        /// Open graph panel scroll offset
        /// </summary>
        private Vector2 OpenGraphOffset = Vector2.zero;

        /// <summary>
        /// Open graph panel found graphs
        /// </summary>
        private NodeGraph[] FoundGraphs;

        /// <summary>
        /// Open graph search string
        /// </summary>
        private string OpenGraphSearch = "" , LastOpenGraphSearch = "";

        /// <summary>
        /// Working canvas
        /// </summary>
        [SerializeField]
        private MoonEditorCanvas canvas;
        
        /// <summary>
        /// Working module data
        /// </summary>
        private GraphModuleAttribute ModuleData;
        
        /// <summary>
        /// New graph name
        /// </summary>
        private string NewGraphName = ""; 
        
        /// <summary>
        /// Current GUI State
        /// </summary>
        public int CurrentGUIState = 0;

        /// <summary>
        /// Menu toolbar buttuns width 
        /// </summary>
        public const float MenuBtnWidth = 45f;

       
        
        /// <summary>
        /// Displays the toolbar menu
        /// </summary>
        public void DoMenu()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button("New" , EditorStyles.toolbarDropDown , GUILayout.Width(MenuBtnWidth)))
            {
                System.Type[] foundmodules = MoonReflection.GetAllDerivedTypes(typeof(NodeGraph)).ToArray();

                GenericMenu menu = new GenericMenu();

                for (int i = 0; i < foundmodules.Length ; i++)
                {
                    System.Type curr = foundmodules[i];

                    var atr = (GraphModuleAttribute)MoonReflection.GetAttribute(curr, typeof(GraphModuleAttribute), true);

                    if (atr != null )
                    {
                        atr.Init(curr);
                        menu.AddItem(new GUIContent(atr.Name), false, OnNewGraph, atr);
                    }
                }

                menu.DropDown(new Rect(5, 15, 0, 0));
            }

            if (GUILayout.Button("Open", EditorStyles.toolbarButton,GUILayout.Width(MenuBtnWidth)))
            {
                canvas.HideAddNode();
                CurrentGUIState = (int)GUIState.Open;
                FoundGraphs = MoonIO.GetAllGraphs();
            }

            if (WorkingGraph != null)
            {
                if (GUILayout.Button("Save", EditorStyles.toolbarDropDown, GUILayout.Width(MenuBtnWidth)))
                {
                    GenericMenu men = new GenericMenu();
                    men.AddItem(new GUIContent("Save"), false, new GenericMenu.MenuFunction(() =>
                   {
                       AssetDatabase.SaveAssets();
                       AssetDatabase.Refresh();
                   }));

                    men.AddItem(new GUIContent("Save as..."), false, new GenericMenu.MenuFunction(() =>
                   {
                       CurrentGUIState = (int)GUIState.SaveAs;
                   }));

                    float x = (MenuBtnWidth * 2) + 5;
                    men.DropDown(new Rect(x, 15, 0, 0));
                }

                DoZoomArea();
                GUILayout.Space(5);
                WorkingGraph.OnMenuGUI();
            }
            
            GUILayout.FlexibleSpace();
            
            if (WorkingGraph != null)
            {
                WorkingGraph.OnToolBarGUI();
            }else
            {
                GUILayout.FlexibleSpace();
            }
            
            GUILayout.EndHorizontal();
        }

        private void DoZoomArea()
        {
            if (CurrentGUIState == 0 && WorkingGraph != null)
            {
                // Begin zoom zone --- 

                GUILayout.BeginHorizontal(GUILayout.Width(120));

                GUILayout.Label("Zoom");

                WorkingGraph.Zoom = GUILayout.HorizontalSlider(WorkingGraph.Zoom, NodeGraph.MaxZoom, NodeGraph.MinZoom, GUILayout.Width(75));

                if (Event.current.isScrollWheel && !canvas.DisplayingAddNode)
                {
                    WorkingGraph.Zoom += Event.current.delta.y * 0.03f;

                    WorkingGraph.Zoom = Mathf.Clamp(WorkingGraph.Zoom, NodeGraph.MinZoom, NodeGraph.MaxZoom);
                }

                GUILayout.EndHorizontal();

                // End zoom zone ---
            }
        }

        /// <summary>
        /// Displays the editor depending on it's state
        /// </summary>
        /// <param name="e">User event</param>
        public void DoEditor(Event e)
        {

            GUIState st = (GUIState)CurrentGUIState;

            switch (st)
            {
                case GUIState.Normal:

                    if (WorkingGraph != null)
                    {
                        MoonGUI.DrawGrid(new Rect(0, 17, Screen.width, Screen.height), WorkingGraph.Offset, WorkingGraph.Zoom);

                        canvas.DoNodeEditor(e);
                        WorkingGraph.OnCanvasGUI();
                    }
                    else
                    {
                        MoonGUI.DrawGrid(new Rect(0, 17, Screen.width, Screen.height), Vector2.zero);
                        MoonGUI.DrawCanvasLabel(new Rect(10, 15, 500, 300), new GUIContent("No Graph"), 26, true);
                    }

                    break;
                case GUIState.New:

                    MoonGUI.DrawGrid(new Rect(0, 17, Screen.width, Screen.height), Vector2.zero);

                    if (ModuleData != null)
                    {
                        NewGraphName = MoonModal.InputPanel(new Vector2(200, 100), "New " + ModuleData.Name, "Ok", "Cancel", "Name",
                            NewGraphName,
                            () =>
                            {
                                if (MoonIO.ValidFileName(NewGraphName))
                                {
                                    string foldername = (!string.IsNullOrEmpty(ModuleData.ExportFolderName)) ?
                                    ModuleData.ExportFolderName : ModuleData.Name;

                                    NodeGraph newg = MoonIO.CreateGraphAsset(NewGraphName, foldername , ModuleData.GraphType , 
                                        ModuleData.ModuleType);
                                    if (newg != null)
                                    {
                                        WorkingGraph = newg;
                                    }
                                    NewGraphName = string.Empty;
                                    CurrentGUIState = (int)GUIState.Normal;
                                }
                            },
                            () =>
                            {
                                NewGraphName = string.Empty;
                                CurrentGUIState = (int)GUIState.Normal;
                            });
                    }
                    else
                    {
                        CurrentGUIState = (int)GUIState.Normal;
                    }

                    break;
                case GUIState.Open:

                    MoonGUI.DrawGrid(new Rect(0, 17, Screen.width, Screen.height), Vector2.zero);

                    MoonModal.OpenGraphPanel(ref OpenGraphOffset, ref OpenGraphSearch, FoundGraphs, (NodeGraph ng) =>
                    {
                        WorkingGraph = ng;
                        CurrentGUIState = (int)GUIState.Normal;
                    }, () => CurrentGUIState = (int)GUIState.Normal, () => FoundGraphs = MoonIO.GetAllGraphs());

                    if (!string.Equals(OpenGraphSearch, LastOpenGraphSearch))
                    {
                        FoundGraphs = MoonIO.GetAllGraphs(OpenGraphSearch);
                        LastOpenGraphSearch = OpenGraphSearch;
                    }

                    break;
                case GUIState.SaveAs:

                    MoonGUI.DrawGrid(new Rect(0, 17, Screen.width, Screen.height), Vector2.zero);

                    NewGraphName = MoonModal.InputPanel(new Vector2(200, 100), "Save as ", "Ok", "Cancel", "Name",
                            NewGraphName,
                            () =>
                            {
                                if (MoonIO.ValidFileName(NewGraphName))
                                {
                                    NodeGraph copy = MoonIO.CopyGraph(WorkingGraph, NewGraphName);

                                    if (copy != null)
                                    {
                                        WorkingGraph = copy;
                                    }

                                    CurrentGUIState = (int)GUIState.Normal;
                                }
                            },
                            () => CurrentGUIState = (int)GUIState.Normal);

                    break;

                default:
                    
                    for (int i = 0; i < UserKeyStates.Length; i++)
                    {
                        if (CurrentGUIState == UserKeyStates[i])
                        {
                            System.Action act;
                            if (UserStates.TryGetValue(UserKeyStates[i], out act))
                            {
                                act.Invoke();
                            }
                        }
                    }

                    break;
            }
        }
        
        /// <summary>
        /// Called when user select a graph type to create
        /// </summary>
        /// <param name="data">graph data</param>
        private void OnNewGraph(object data)
        {
            ModuleData = (GraphModuleAttribute)data;
            CurrentGUIState = 1;
        }

    }
}