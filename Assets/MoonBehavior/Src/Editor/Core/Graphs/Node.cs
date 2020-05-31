using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace MoonBehaviorEditor.Core.Graphs
{ 
    /// <summary>
    /// Node Base Class
    /// </summary>
    [System.Serializable]
    public abstract class Node : ScriptableObject
    {
        public NodeGraph WorkingGraph { get; private set; }

        private bool NameChanged = false;

        /// <summary>
        /// Sets the Working Graph.
        /// Used on node inizialitation on graph loading.
        /// </summary>
        /// <param name="WorkingGraph">Graph Instance</param>
        public void SetNodeGraph(NodeGraph WorkingGraph)
        {
            this.WorkingGraph = WorkingGraph;
            InitMovement();
        }

        /// <summary>
        /// Inicializes Node Ports
        /// </summary>
        public void IOInit()
        {
            InputPort.SetParent(this);
            foreach (NodeOutput o in OutputPorts)
                o.SetParent(this);
        }

        /// <summary>
        /// Incialize this node
        /// </summary>
        /// <param name="Position">Init position</param>
        public void Init(Vector2 Position , System.Type taskType)
        {
            ScriptableObject taskinst = CreateInstance(taskType);
            SetID();
            UpdateName(taskType);
            rect = new Rect(Position, DefaultSize);
            SetPosition(Position);
            SetTask(taskinst);
            task.name = "Task__" + taskType.Name;
            EditorUtility.SetDirty(task);
            task.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector; 
            AssetDatabase.AddObjectToAsset(task, WorkingGraph);
            OnInit();
        }
        
        /// <summary>
        /// OnEnable unity event to update name and init movement at change
        /// </summary>
        protected virtual void OnEnable()
        {
            if (Task != null)
            {
                UpdateName(Task.GetType());
            }
            InitMovement();
        }

        /// <summary>
        /// Initializes node movement
        /// </summary>
        protected void InitMovement()
        {
            Selected = false;
            Dragging = false;
        }
        
        /// <summary>
        /// Updates the node name based on the Task type and it's width acording to name
        /// </summary>
        /// <param name="Tasktype">Task type to get Node data</param>
        protected void UpdateName(System.Type Tasktype)
        {
            if (Tasktype != null && !NameChanged)
            {
                name = MoonReflection.GetNodeData(Tasktype).Name;
                NameChanged = true; 
            }
        }

        /// <summary>
        /// Assign a new task
        /// </summary>
        /// <param name="newtask">New task instance</param>
        public void SetTask(ScriptableObject newtask)
        {
            task = newtask;
        }
        
        /// <summary>
        /// Assing a new id for this node
        /// </summary>
        public void SetID()
        {
            id = System.Guid.NewGuid().ToString();
            if (InputPort != null)
            {
                InputPort.ClearOutput();
                InputPort.SetParent(this);
                
            }
            for (int i = 0; i < OutputPorts.Length; i++)
            {
                OutputPorts[i].ClearAll(true);
                OutputPorts[i].SetParent(this);
            }
        }

        /// <summary>
        /// Are we actually dragging a node ?
        /// </summary>
        private static bool ActuallyDragging = false;

        private enum TooltipPosition { Down , Up }

        /// <summary>
        /// Node user description
        /// </summary>
        [TextArea()]
        public string Tooltip = "";

        [SerializeField]
        private TooltipPosition tooltipPosition;

        /// <summary>
        /// Stored task
        /// </summary>
        [SerializeField]
        protected ScriptableObject task; 
        
        /// <summary>
        /// Task object
        /// </summary>
        public virtual ScriptableObject Task { get { return task; } }

        /// <summary>
        /// Stored node id
        /// </summary>
        [SerializeField]
        private string id;

        /// <summary>
        /// Node unique id
        /// </summary>
        public string ID
        {
            get
            {
               return id;
            }
        }
        
        /// <summary>
        /// Are dragging ?
        /// </summary>
        public bool Dragging { get; set; }

        /// <summary>
        /// Are Selected ? 
        /// </summary>
        public bool Selected { get; set; }
        
        /// <summary>
        /// Stored Position Pan Offset
        /// </summary>
        [SerializeField]
        private Vector2 _PositionOffset;

        /// <summary>
        /// Position Pan Offset
        /// </summary>
        public Vector2 PositionOffset
        {
            get
            {
                return _PositionOffset;
            }

            set
            {
                _PositionOffset = value;
                UpdatePosition();
            }

        }

        /// <summary>
        /// Stored base position
        /// </summary>
        [SerializeField]
        private Vector2 _Position;
        
        /// <summary>
        /// Node base Positon 
        /// </summary>
        private Vector2 Position
        {
            get
            {
                return _Position;
            }

            set
            {
                _Position = value;
                UpdatePosition();
            }

        }

        /// <summary>
        /// Node rect
        /// </summary>
        public Rect rect;

        /// <summary>
        /// Node default size
        /// </summary>
        protected abstract Vector2 DefaultSize { get;  }

        /// <summary>
        /// Called when the node are initialized
        /// </summary>
        protected virtual void OnInit() { }

        /// <summary>
        /// User defined node context menu items
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string,System.Action<object>> ContextMenu()
        { return new Dictionary<string, System.Action<object>>(); }
        
        /// <summary>
        /// Node style
        /// </summary>
        public virtual GUIStyle Style { get { return GUI.skin.box; } }

        public virtual void OnAddedConnection(Node source) { }
        
        public virtual void OnRemovedConnection(Node node) { }

        public virtual void OnSortTargets() { }

        /// <summary>
        /// Node input port
        /// </summary>
        public abstract NodeInput InputPort { get; }
        
        /// <summary>
        /// Node output ports
        /// </summary>
        public abstract NodeOutput[] OutputPorts { get; }
        
        /// <summary>
        /// Mouse down event on node ports
        /// </summary>
        /// <param name="canvas">Working canvas</param>
        /// <param name="e">user event</param>
        public void MouseDownPorts(MoonEditorCanvas canvas , Event e)
        {
            InputPort.MouseDown(e, canvas);

            if (OutputPorts != null && OutputPorts.Length > 0)
            {
                for (int i = 0; i < OutputPorts.Length; i++)
                {
                    NodePort current = OutputPorts[i];
                    if (current != null)
                        current.MouseDown(e, canvas);
                }
            }
        }
        
        /// <summary>
        /// Mouse down event
        /// </summary>
        /// <param name="MousePoint">Mouse point</param>
        /// <param name="canvas">working canvas</param>
        public void MouseDown(Vector2 MousePoint , MoonEditorCanvas canvas)
        {
            if (InNode(MousePoint) && !InPorts(MousePoint) && !ActuallyDragging)
            {
                Dragging = true;
                ActuallyDragging = true;
            }
        }
        
        /// <summary>
        /// Mouse up event
        /// </summary>
        /// <param name="MousePoint">Mouse Point</param>
        /// <param name="canvas">Working canvas</param>
        public void MouseUp(Vector2 MousePoint , MoonEditorCanvas canvas)
        {
            if (Dragging)
            {
                Dragging = false; 
            }

            if (OutputPorts != null && OutputPorts.Length > 0 )
            {
                foreach (NodeOutput output in OutputPorts) output.SortInputs();
            }

            ActuallyDragging = false;
            
        }

        /// <summary>
        /// Mouse drag event
        /// </summary>
        /// <param name="Delta">Mouse delta</param>
        /// <param name="canvas">Working canvas</param>
        /// <returns></returns>
        public bool MouseDrag(Vector2 Delta , MoonEditorCanvas canvas)
        {
            if (Dragging)
            {
                Drag(Delta);
                if (Event.current.shift)
                {
                    this.Descendents().ToList().ForEach((Node n) => n.Drag(Delta));
                }
                return true; 
            }
            return false;
        }

        /// <summary>
        /// Draws the node and it's ports to the GUI layer
        /// </summary>
        public virtual void Draw()
        {
            if (Application.isPlaying)
                GUI.color = Color.white;
            
            if (NameChanged)
            {
                float actualw = DefaultSize.x;
                float width = Mathf.Clamp(Style.CalcSize(new GUIContent(name)).x + 20, actualw, float.MaxValue);
                rect.width = width;
                NameChanged = false; 
            }
            
            GUI.Box(rect, name, Style);

            GUIContent labelcont = new GUIContent( ( !string.IsNullOrEmpty(Tooltip) ? "// " + Tooltip : string.Empty )  );

            Vector2 labelsize = EditorStyles.label.CalcSize(labelcont);

            float LW = labelsize.x + 5;

            float y = (tooltipPosition == TooltipPosition.Down) ? rect.yMax + 5 : (rect.y - 5) - labelsize.y; 

            GUI.Label(new Rect((rect.x + rect.width) - LW, y , LW  , labelsize.y) , labelcont , 
                new GUIStyle(EditorStyles.label)
                {
                    normal = new GUIStyleState() { textColor = new Color(0.7f,0.7f,0.7f,0.8f) }
                });
            
        }

        /// <summary>
        /// Draws the output port to the GUI layer
        /// </summary>
        public void DrawIO()
        {
            InputPort.Draw();
            if (OutputPorts != null && OutputPorts.Length > 0)
            {
                for (int i = 0; i < OutputPorts.Length; i++)
                {
                    OutputPorts[i].Draw();
                }
            }
        }

        /// <summary>
        /// Drags the node
        /// </summary>
        /// <param name="delta">Mouse delta</param>
        public void Drag(Vector2 delta)
        {
            Position += delta;
            InputPort.UpdatePosition();

            if (OutputPorts != null && OutputPorts.Length > 0 )
            {
                for (int i = 0; i < OutputPorts.Length; i++)
                {
                    OutputPorts[i].UpdatePosition();
                }
            }
        }
        
        /// <summary>
        /// Update the node position
        /// </summary>
        private void UpdatePosition()
        {
            rect.position = GetPosition();
        }

        /// <summary>
        /// Gets the node position
        /// </summary>
        /// <returns>Node position vector</returns>
        public Vector2 GetPosition()
        {
            return PositionOffset + Position;
        }

        public void SetPosition(Vector2 NewPosition)
        {
            Position = (NewPosition * WorkingGraph.Zoom ) - WorkingGraph.GraphOffset;
        }

        /// <summary>
        /// Checks if the given point are inside this node
        /// </summary>
        /// <param name="Point"></param>
        public bool InNode(Vector2 Point)
        {
            return rect.Contains(Point);
        }

        /// <summary>
        /// Checks if the given point are inside this node or any port of it.
        /// </summary>
        /// <param name="Point">Point</param>
        public virtual bool InPorts(Vector2 Point)
        {
            bool status = false;

            status = InputPort.InPort(Point);

            if (!status && OutputPorts != null && OutputPorts.Length > 0 )
            {
                for (int i = 0; i < OutputPorts.Length; i++)
                {
                    if (!status  && OutputPorts[i] != null)
                    {
                        status = OutputPorts[i].InPort(Point);
                    }
                }
            }
            return status;
        }

        /// <summary>
        /// Search for an output on this node
        /// </summary>
        /// <param name="id">Output index</param>
        /// <returns>Output found</returns>
        public NodeOutput FindOutput(int id )
        {
            if (id >= 0 && id < OutputPorts.Length)
                return OutputPorts[id];
            else return null;
        }
    }
}
