using UnityEngine;
using UnityEditor;

using System.Collections.Generic;

using MoonBehaviorEditor.Core.Graphs;
using MoonBehaviorEditor.Modals;
using MoonBehaviorEditor.Modals.Nodes;

namespace MoonBehaviorEditor.Core
{
    /// <summary>
    /// Handles all node events and drawing
    /// </summary>
    public class MoonEditorCanvas
    {
        /// <summary>
        /// Context menu modes
        /// </summary>
        private enum ContextMode
        {
            None,
            SingleNode,
            MultiMode,
            Canvas
        }

        // context menu 

        /// <summary>
        /// Current context menu mode
        /// </summary>
        private ContextMode Context_Mode = ContextMode.None;
        /// <summary>
        /// Current context menu data
        /// </summary>
        private object Context_Data { get; set; }

        /// <summary>
        /// Active Node Output
        /// </summary>
        public NodeOutput ActiveOutput { get; set; }

        /// <summary>s
        /// Are the mouse inside of a node?
        /// </summary>
        private bool inNode = false;
        /// <summary>
        /// User multi selection nodes
        /// </summary>
        private List<Node> SelectedNodes = new List<Node>();

        /// <summary>
        /// Start position of multi selection rect
        /// </summary>
        private Vector2 MultiSelection_Start = Vector2.zero;
        /// <summary>
        /// Multi selection Rect
        /// </summary>
        private Rect MultiSelection_Rect = new Rect(0, 0, 0, 0);
        /// <summary>
        /// Are we drawing the selection ?
        /// </summary>
        private bool DrawSelection = false;

        /// <summary>
        /// Add node window rect
        /// </summary>
        private Rect AddNode_Rect;
        /// <summary>
        /// Add node scroll point
        /// </summary>
        private Vector2 AddNode_Scroll = Vector2.zero;
        /// <summary>
        /// Add node Position
        /// </summary>
        private Vector2 AddNode_Position = Vector2.zero;

        /// <summary>
        /// Add node search filter
        /// </summary>
        private string AddNode_Search = "" , AddNode_LastSearch = ""; 

        /// <summary>
        /// Are the position chached on add node window ?
        /// </summary>
        private bool AddNode_PositionCached = false;

        /// <summary>
        /// Aviable nodes
        /// </summary>
        private NodeCollection[] AddNode_Collections = null;

        /// <summary>
        /// Cached node for copy
        /// </summary>
        private Node CachedNode = null; 

        #region Public API 

        /// <summary>
        /// Are the add node window Displaying ?
        /// </summary>
        public bool DisplayingAddNode { get; private set; }

        private MoonEditorWindow Window { get; set; }

        private NodeGraph Graph { get { return Window.CurrentGraph; } }

        public void Init(MoonEditorWindow wind)
        {
            GUIScaleUtility.CheckInit();
            Window = wind;
            if (Graph != null && Graph.Nodes.Count > 0 )
            {
                Graph.Nodes.ForEach((Node n) => 
                {
                    n.SetNodeGraph(wind.CurrentGraph);
                    n.IOInit();
                });
            }
        }
        
        /// <summary>
        /// Draws and enables the node editor
        /// </summary>
        /// <param name="e">User event</param>
        public void DoNodeEditor(Event e )
        {
            Rect _zoomrect = new Rect(new Vector2(0,19),Window.position.size);

            Vector2 pivotscale = _zoomrect.size / 2 ;

            Graph.ZoomPivot = GUIScaleUtility.BeginScale(ref _zoomrect , pivotscale , Graph.Zoom , true , false );
            
            // Node drawing

            UnityEngine.Profiling.Profiler.BeginSample("MoonBehavior: Node Drawing");

            if (Graph.Nodes.Count > 0 && (e.type == EventType.Layout || e.type == EventType.Repaint))
            {
                DrawNodes(Graph.Nodes,_zoomrect);
            }
            
            UnityEngine.Profiling.Profiler.EndSample();

            // Node interaction

            UnityEngine.Profiling.Profiler.BeginSample("MoonBehavior: Node events");
            
            HandleNodeEvents(e , Graph);

            UnityEngine.Profiling.Profiler.EndSample();
            
            if (ActiveOutput != null)
            {
                MoonGUI.DrawLine(ActiveOutput.GetPosition(), e.mousePosition, new Color(1, 1, 1, 0.6f));
            }
            
            GUIScaleUtility.EndScale();

            HandleAddNodes(e);
            HandleContext();
        }

        #endregion

        /// <summary>
        /// Hides the 'add node' window
        /// </summary>
        public void HideAddNode()
        {
            DisplayingAddNode = false;
            AddNode_Rect = new Rect();
            AddNode_PositionCached = false;
            AddNode_Collections = null; 
        }


        // private API 

        /// <summary>
        /// Hanldles the 'add node' window
        /// </summary>
        /// <param name="e">User event</param>
        private void HandleAddNodes(Event e)
        {
            if (DisplayingAddNode && !EditorApplication.isPlaying)
            {
                if (!AddNode_PositionCached)
                {
                    AddNode_Position = e.mousePosition;
                    AddNode_PositionCached = true;
                }
                
                if (AddNode_Collections == null || (AddNode_Search != AddNode_LastSearch))
                {

                    AddNode_Collections = NodeCollection.GetAllCollections(AddNode_Search,Graph);

                    AddNode_LastSearch = AddNode_Search;
                }

                MoonModal.AddNodePanel( Graph , AddNode_Position, ref AddNode_Search ,  ref AddNode_Scroll, AddNode_Collections 
                    , out AddNode_Rect,
                    (System.Type t ) =>
                    {
                        Graph.AddNode(AddNode_Position, t);
                        HideAddNode();
                    });
                
                if (e.type == EventType.MouseDown)
                {
                    if (!AddNode_Rect.Contains(e.mousePosition))
                    {
                        HideAddNode();
                    }
                }
            }

        }

        /// <summary>
        /// Handles all node events
        /// </summary>
        /// <param name="e">User event</param>
        private void HandleNodeEvents(Event e , NodeGraph workspace)
        {
            EventType etype = e.type;

            if (workspace.Nodes != null && workspace.Nodes.Count > 0)
            {

                // Selected nodes handling

                if (SelectedNodes.Count > 0)
                {
                    if (e.type == EventType.MouseDown)
                    {
                        if (SelectedNodes.ContainsPoint(e.mousePosition))
                        {
                            if (e.button == 0)
                            {
                                inNode = true;
                            }
                            else if (e.button == 1)
                            {
                                SetContext(ContextMode.MultiMode);
                            }
                        }
                    }

                    if (e.type == EventType.MouseUp || e.type == EventType.MouseDown)
                    {
                        if (!SelectedNodes.ContainsPoint(e.mousePosition))
                        {
                            SelectedNodes.ForEach((Node n) => n.Dragging = false);
                            inNode = false;
                            SelectedNodes.Clear();
                        }
                    }

                    if (e.type == EventType.MouseDrag)
                    {
                        SelectedNodes.ForEach((Node n) =>
                        {
                            if (n.InNode(e.mousePosition))
                            {
                                inNode = true;
                            }

                            if (inNode & !n.Dragging)
                                n.Dragging = true;

                            if (n.Dragging && e.mousePosition.y >= 0)
                                n.Drag(e.delta);
                        });
                    }
                }
                bool Dragging = false;

                // Individual nodes handling

                if (SelectedNodes.Count == 0)
                {
                    inNode = false;

                    for (int i = 0; i < workspace.Nodes.Count; i++)
                    {
                        Node currentNode = workspace.Nodes[i];

                        if (!inNode) inNode = (currentNode.InNode(e.mousePosition) || currentNode.InPorts(e.mousePosition));

                        if (etype == EventType.MouseDown && !DisplayingAddNode)
                        {
                            currentNode.MouseDownPorts(this, e);

                            if (e.button == 0) // Left mouse click
                            {
                                currentNode.MouseDown(e.mousePosition, this);

                                if (currentNode.InNode(e.mousePosition))
                                {
                                    if (!currentNode.InPorts(e.mousePosition))
                                    {
                                        currentNode.Selected = true;
                                        Selection.activeObject = currentNode;
                                    }

                                    if (ActiveOutput != null)
                                    {
                                        currentNode.InputPort.SetSourceOutput(ActiveOutput);

                                        if (!currentNode.InputPort.InPort(e.mousePosition))
                                            ActiveOutput = null;
                                    }
                                }
                                else
                                {
                                    currentNode.Selected = false;
                                }
                            }
                            else if (e.button == 1) // Right mouse click
                            {
                                if (currentNode.InNode(e.mousePosition) && !currentNode.InPorts(e.mousePosition))
                                {
                                    SetContext(ContextMode.SingleNode, currentNode);
                                }
                            }
                        }
                        else if (etype == EventType.MouseUp)
                        {
                            currentNode.MouseUp(e.mousePosition, this);
                        }
                        else if (etype == EventType.MouseDrag)
                        {
                            if (currentNode.Dragging)
                            {
                                Dragging = currentNode.MouseDrag(e.delta, this);
                            }
                        }
                        
                        if (Dragging)
                        {
                            inNode = true;

                            int lastindex = workspace.Nodes.Count - 1;

                            if (i >= 0 && i < workspace.Nodes.Count && lastindex >= 0 && lastindex < workspace.Nodes.Count)
                            {
                                Node backup = workspace.Nodes[i];
                                workspace.Nodes[i] = workspace.Nodes[lastindex];
                                workspace.Nodes[lastindex] = backup;
                            }
                        }
                    }
                }

                if (!inNode)
                {
                    if (etype == EventType.MouseDown)
                    {
                        Selection.activeObject = null; 
                        if (e.button == 1)
                        {
                            if (ActiveOutput != null)
                            {
                                ActiveOutput = null;
                                return;
                            }
                            else if (!DisplayingAddNode)
                            {
                                SetContext(ContextMode.Canvas);
                            }
                        }
                        else if (e.button == 0 && !DisplayingAddNode)
                        {
                            MultiSelection_Start = e.mousePosition;
                            DrawSelection = true;
                        }
                    }
                }

                if (DrawSelection)
                {
                    if (e.type == EventType.MouseDrag && !e.shift)
                    {
                        MultiSelection_Rect = MoonGUIUtility.GetScreenRect(MultiSelection_Start, e.mousePosition);
                    }
                    else if (e.rawType == EventType.MouseUp)
                    {
                        List<Node> _selected = workspace.Nodes.FindAll((Node n) => MultiSelection_Rect.Overlaps(n.rect));

                        if (_selected.Count > 1)
                        {
                            _selected.ForEach((Node n) => n.Selected = true);
                            SelectedNodes = _selected;
                            inNode = true;
                        }

                        DrawSelection = false;
                        MultiSelection_Rect = new Rect(0, 0, 0, 0);
                    }

                    EditorGUI.DrawRect(MultiSelection_Rect, new Color(0.616f, 0.796f, 0.918f, 0.350f));

                }

                if (etype == EventType.MouseDrag && !Dragging && !(SelectedNodes.Count > 0 && inNode) && !DisplayingAddNode)
                {
                    if (e.button == 0 || e.button == 2)
                    {
                        if (e.shift || e.button == 2)
                        {
                            workspace.Offset += e.delta;
                        }
                    }
                }
            }
            else if (e.type == EventType.MouseDown)
            {
                if (e.button == 1 && !DisplayingAddNode)
                {
                    SetContext(ContextMode.Canvas);
                }
            }
        }

        /// <summary>
        /// Draws all the nodes on the GUI Layer
        /// </summary>
        /// <param name="nodes">nodes to draw</param>
        private void DrawNodes(List<Node> nodes, Rect Zoomrect)
        {
            for (int i = 0; i < nodes.Count; i++ )
            {
                Node n = nodes[i];
                n.PositionOffset = Graph.GraphOffset;
                if (Zoomrect.Overlaps(n.rect))
                {
                    n.Draw();
                }
                n.DrawIO();
            }
        }

        #region Context Menu
        
        /// <summary>
        /// Hides the context menu
        /// </summary>
        private void ResetContext()
        {
            Context_Data = null;
            Context_Mode = ContextMode.None;
        }

        /// <summary>
        /// Displays the context menu
        /// </summary>
        /// <param name="mode">Display mode</param>
        /// <param name="data">User data</param>
        private void SetContext(ContextMode mode, object data = null)
        {
            Context_Mode = mode;
            Context_Data = data;
        }

        /// <summary>
        /// Individual node context menu
        /// </summary>
        private void NodeContextMenu()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Copy"), false, NodeCopyed, Context_Data);
            menu.AddItem(new GUIContent("Delete"), false, NodeDeleted, Context_Data);

            Node n = (Node)Context_Data;

            Dictionary<string, System.Action<object>> usermenu = n.ContextMenu();

            if (usermenu != null && usermenu.Keys.Count > 0)
            {
                string[] useroptions = new string[usermenu.Keys.Count];

                usermenu.Keys.CopyTo(useroptions, 0);

                for (int i = 0; i < useroptions.Length; i++)
                {
                    if (useroptions[i][0] != '$')
                    {
                        System.Action<object> useraction;
                        if (usermenu.TryGetValue(useroptions[i], out useraction))
                        {
                            menu.AddItem(new GUIContent(useroptions[i]), false,
                                new GenericMenu.MenuFunction2((object xdata) => useraction.Invoke(xdata)), Context_Data);
                        }
                    }
                    else
                    {
                        menu.AddSeparator(useroptions[i].Replace("$", string.Empty));
                    }
                }
            }
            menu.ShowAsContext();
            ResetContext();
        }

        /// <summary>
        /// Canvas context menu
        /// </summary>
        private void CanvasContextMenu()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add node"), false, new GenericMenu.MenuFunction( () => DisplayingAddNode = true ));
            if (CachedNode != null )
            {
                menu.AddItem(new GUIContent("Paste node"), false, NodePasted, Event.current.mousePosition);
            }

            Dictionary<string, System.Action> UserOptions = Graph.CanvasContextMenu(Window.Controller);

            string[] UserKeys = new string[UserOptions.Keys.Count];

            UserOptions.Keys.CopyTo(UserKeys,0);

            for (int i = 0; i < UserKeys.Length; i++)
            {
                string key = UserKeys[i];
                if (key[0] != '$')
                {
                    System.Action act;
                    if (UserOptions.TryGetValue(key , out act))
                    {
                        menu.AddItem(new GUIContent(key), false, new GenericMenu.MenuFunction(act));
                    }
                }else
                {
                    menu.AddSeparator(key.Replace("$", string.Empty));
                }
            }
            
            menu.ShowAsContext();
            ResetContext();
        }

        /// <summary>
        /// Context menu for multi selected nodes
        /// </summary>
        private void MultiNodeContextMenu()
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Delete"), false, new GenericMenu.MenuFunction(() => { foreach (Node n in SelectedNodes) Graph.RemoveNode(n); }));

            menu.ShowAsContext();
            ResetContext();
        }

        /// <summary>
        /// Handles the context menu drawing
        /// </summary>
        private void HandleContext()
        {
            if (!EditorApplication.isPlaying)
            {
                switch (Context_Mode)
                {
                    case ContextMode.Canvas:

                        CanvasContextMenu();

                        break;
                    case ContextMode.SingleNode:

                        NodeContextMenu();

                        break;
                    case ContextMode.MultiMode:

                        MultiNodeContextMenu();

                        break;

                    case ContextMode.None:
                        return;
                }
            }else if (Context_Mode != ContextMode.None)
            {
                ResetContext();
            }
        }

        #endregion
        
        /// <summary>
        /// Copys a Node
        /// </summary>
        /// <param name="n">Node Instance</param>
        private void CopyNode(Node n)
        {
            if (n != null )
            {
                CachedNode = n; 
            }
        }

        /// <summary>
        /// Pastes the cached node
        /// </summary>
        /// <param name="MousePoint"></param>
        private void PasteNode(Vector2 MousePoint)
        {
            if (CachedNode != null  )
            {
                Graph.CopyNode(MousePoint, CachedNode);
                CachedNode = null; 
            }
        }

        // Context Events

        /// <summary>
        /// Called when the user pastes a node
        /// </summary>
        private void NodePasted(object data)
        {
            PasteNode((Vector2)data);
        }

        /// <summary>
        /// Called when the user copys a node
        /// </summary>
        /// <param name="data">Current node data</param>
        private void NodeCopyed( object data)
        {
            CopyNode((Node)data);
        }

        /// <summary>
        /// Called when the user deletes a node
        /// </summary>
        /// <param name="data"></param>
        private void NodeDeleted( object data)
        {
            Graph.RemoveNode((Node)data);
        }
    }
}