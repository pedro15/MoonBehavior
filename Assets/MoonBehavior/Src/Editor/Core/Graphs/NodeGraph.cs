using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MoonBehaviorEditor.Core.Graphs
{
    /// <summary>
    /// Node Graph container base class
    /// </summary>
    public abstract class NodeGraph : ScriptableObject
    {
        /// <summary>
        /// Stored node list
        /// </summary>
        public List<Node> Nodes = new List<Node>();
        
        /// <summary>
        /// Pan Offset
        /// </summary>
        [SerializeField]
        public Vector2 Offset = Vector2.zero;

        /// <summary>
        /// Zoom pivot point
        /// </summary>
        public Vector2 ZoomPivot;

        /// <summary>
        /// Graph total Offset
        /// </summary>
        public Vector2 GraphOffset
        {
            get
            {
                return Offset + ZoomPivot; 
            }
        }
        
        /// <summary>
        /// Zoom divisor
        /// </summary>
        [SerializeField]
        public float Zoom = 1f;


        /// <summary>
        /// Max zoom divisor
        /// </summary>
        public const float MaxZoom = 1.7f;
        /// <summary>
        /// Minimun zoom divisor
        /// </summary>
        public const float MinZoom = 1f;

        /// <summary>
        /// User defined context menu elements on empty space
        /// </summary>
        /// <returns>Dictionary with user options</returns>
        public virtual Dictionary<string, System.Action> CanvasContextMenu(MoonEditorController controller) { return new Dictionary<string, System.Action>(); }

        /// <summary>
        /// Canvas GUI States and it's default actions
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<int, System.Action> CanvasGUIStates(MoonEditorController controller)
        {
            return new Dictionary<int, System.Action>();
        }
        
        /// <summary>
        /// Finds a Node by id
        /// </summary>
        /// <param name="nodeid">Id to find</param>
        /// <returns>node found</returns>
        public Node FindNode(string nodeid)
        {
            //return Nodes.Where((Node n) => string.Equals(nodeid, n.ID)).First();
            return Nodes.Find((Node n) => string.Equals(nodeid, n.ID));
            //return Nodes.Select((Node n) => string.Equals(nodeid, n.ID)).First();
        }
        
        /// <summary>
        /// Adds a new node to the graph
        /// </summary>
        /// <param name="ScreenPoint">Canvas screen position</param>
        /// <param name="type">Node task type</param>
        /// <returns>Node Instance</returns>
        public Node AddNode(Vector2 ScreenPoint , System.Type type)
        {
            Node node = (Node)CreateInstance(GetNodeType());
            node.SetNodeGraph(this);
            node.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
            AssetDatabase.AddObjectToAsset(node, this);
            Nodes.Add(node);
            node.Init(ScreenPoint, type);
            node.InputPort.SetParent(node);
            foreach (NodeOutput o in node.OutputPorts) o.SetParent(node);
            AssetDatabase.SaveAssets();
            OnNodeAdded(node);
            return node;
        }
        
        /// <summary>
        /// Copys a node
        /// </summary>
        /// <param name="NewScreenPoint">Screen point for node copy</param>
        /// <param name="nodeInstance">Node instance to copy</param>
        public void CopyNode(Vector2 NewScreenPoint , Node nodeInstance)
        {
            Node nodecopy = Instantiate(nodeInstance);

            nodecopy.SetNodeGraph(this);
            nodecopy.IOInit();
            nodecopy.SetID();
            
            nodecopy.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
            nodecopy.name = nodecopy.name.Replace("(Clone)", string.Empty);
            
            AssetDatabase.AddObjectToAsset(nodecopy, this);
  
            ScriptableObject taskcopy = Instantiate(nodecopy.Task);

            taskcopy.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;

            taskcopy.name = taskcopy.name.Replace("(Clone)", string.Empty);

            AssetDatabase.AddObjectToAsset(taskcopy,this);

            AssetDatabase.SaveAssets();

            nodecopy.SetPosition(NewScreenPoint);

            nodecopy.SetTask(taskcopy);

            Nodes.Add(nodecopy);
            
            OnNodePasted(nodecopy);
            
        }
        
        /// <summary>
        /// Remove a node from the Graph
        /// </summary>
        /// <param name="n">Node instance</param>
        public void RemoveNode(Node n)
        {
            if (n != null )
            {
                RemoveNode(n.ID);
            }
        }
        
        /// <summary>
        /// Remove a node from the Graph
        /// </summary>
        /// <param name="NodeId">Node id</param>
        public void RemoveNode(string NodeId)
        {
            int index = Nodes.FindIndex((Node n) => string.Equals(n.ID, NodeId));
            if (index >= 0 & index < Nodes.Count)
            {
                Node n = Nodes[index];
                n.InputPort.ClearOutput();
                
                for (int i = 0; i < n.OutputPorts.Length; i++)
                {
                    n.OutputPorts[i].ClearAll();
                }

                Nodes.RemoveAt(index);

                OnNodeDeleted(n);

                DestroyImmediate(n.Task, true);
                DestroyImmediate(n, true);

                AssetDatabase.SaveAssets();
            }
        }
        

        public virtual void OnMenuGUI() { }

        /// <summary>
        /// Custom toolbar: Called on the right corner of the toolbar
        /// </summary>
        public virtual void OnToolBarGUI() { }

        /// <summary>
        /// Called in canvas space
        /// </summary>
        public virtual void OnCanvasGUI() { }


        public virtual void OnNodeAdded(Node node) { }
        
        /// <summary>
        /// Called when user pastes a node on the node graph
        /// </summary>
        public virtual void OnNodePasted(Node node) { }

        /// <summary>
        /// Called when user deletes a node from the node graph
        /// </summary>
        /// <param name="node"></param>
        public virtual void OnNodeDeleted(Node node) { }

        
        /// <summary>
        /// Node base type
        /// </summary>
        protected abstract System.Type NodeType { get; }
        
        /// <summary>
        /// Node tasks base type
        /// </summary>
        public abstract System.Type TaskType { get; }

        /// <summary>
        /// Icon Name for each node type found at Resources/NodeIcons/
        /// </summary>
        /// <param name="BaseTaskType">Node type</param>
        /// <returns></returns>
        public abstract string GetIconName(System.Type BaseTaskType);

        /// <summary>
        /// Gets the node base type
        /// </summary>
        /// <returns>type</returns>
        private System.Type GetNodeType()
        {
            System.Type usertype = NodeType;
            if (usertype != null )
            {
                if (usertype.BaseType != typeof(Node))
                {
                    Debug.LogError("[NodeGraph:" + GetType().Name + "] Invalid node base type !");
                }
            }else
            {
                Debug.LogError("[NodeGraph:" + GetType().Name + "] NULL node base type !");
            }
            return usertype;
        }
    }
}
