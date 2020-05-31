using UnityEngine;

namespace MoonBehaviorEditor.Core.Graphs
{
    /// <summary>
    /// Node port base class
    /// </summary>
    [System.Serializable]
    public abstract class NodePort
    {
        /// <summary>
        /// Port rect
        /// </summary>
        public Rect rect;

        // Main abstract properties

        /// <summary>
        /// Stored Parent id
        /// </summary>
        [SerializeField]
        private string parentid;

        [System.NonSerialized]
        private NodeGraph m_WorkingGraph;

        /// <summary>
        /// parent instance
        /// </summary>
        [System.NonSerialized]
        private Node parent = null;
        
        /// <summary>
        /// Parent node
        /// </summary>
        public Node Parent
        {
            get
            {
                if (!parent)
                {
                    if (m_WorkingGraph != null)
                    {
                        parent = m_WorkingGraph.FindNode(parentid);
                    }
                }
                return parent; 
            }
        }
        
        /// <summary>
        /// Node port constructor
        /// </summary>
        /// <param name="Parent">Node parent</param>
        public NodePort(Node Parent)
        {
            SetParent(Parent);
        }
        
        public void SetParent(Node Parent)
        {
            m_WorkingGraph = Parent.WorkingGraph;
            parentid = Parent.ID;
            if (parent != null && parent.ID != Parent.ID)
                parent = null;
        }

        /// <summary>
        /// Port style
        /// </summary>
        public virtual GUIStyle Style { get { return GUI.skin.box; } }

        // Events

        /// <summary>
        /// Mouse down event
        /// </summary>
        /// <param name="e">User event</param>
        /// <param name="canvas">Working canvas</param>
        public abstract void MouseDown(Event e , MoonEditorCanvas canvas);

        protected abstract Vector2 GetOffset();

        /// <summary>
        /// Does the point are inside this port ?
        /// </summary>
        /// <param name="p">point</param>
        public virtual bool InPort(Vector2 p)
        {
            return rect.Contains(p);
        }

        /// <summary>
        /// Update port position relative to it's parent node and it's Offset
        /// </summary>
        public void UpdatePosition()
        {
            Vector2 DesiredPosition = Parent.rect.position + GetOffset();
            if (DesiredPosition != rect.position)
            {
                rect.position = DesiredPosition;
            }
        }
        
        /// <summary>
        /// Port position
        /// </summary>
        /// <returns>Position vector</returns>
        public virtual Vector2 GetPosition()
        {
            return rect.center;
        }

        /// <summary>
        /// Draws the port to the GUI layer
        /// </summary>
        public virtual void Draw()
        {
            UpdatePosition();
            GUI.Box(rect, "", Style);
        }
        
    }
}