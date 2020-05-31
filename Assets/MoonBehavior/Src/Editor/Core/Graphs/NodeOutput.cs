using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MoonBehaviorEditor.Core.Graphs
{
    /// <summary>
    /// Node output base class
    /// </summary>
    [System.Serializable]
    public abstract class NodeOutput : NodePort
    {
        /// <summary>
        /// Node output constructor
        /// </summary>
        /// <param name="parent">Node parent</param>
        public NodeOutput(Node parent ) : base(parent)
        {
            rect.size = Vector2.one * 12f;
        }

        /// <summary>
        /// Set new index for the output
        /// </summary>
        /// <param name="parentNode">Node parent</param>
        /// <param name="newIndex">new index</param>
        public void SetIndex(Node parentNode, int newIndex)
        {
            if (string.Equals(parentNode.ID, Parent.ID))
            {
                index = newIndex;
            }
        }

        /// <summary>
        /// Stored Output index
        /// </summary>
        private int index;

        /// <summary>
        /// Current index
        /// </summary>
        public int Index { get { return index; } }

        /// <summary>
        /// Target input list instance
        /// </summary>
        [System.NonSerialized]
        protected List<NodeInput> targetInputs = new List<NodeInput>();

        /// <summary>
        /// Stored target inputs ids
        /// </summary>
        [SerializeField]
        private List<string> targetInputsids = new List<string>();

        /// <summary>
        /// Can add connect to that input ?
        /// </summary>
        /// <param name="input">input</param>
        /// <returns>Can connect boolean</returns>
        public virtual bool CanAddInput(NodeInput input) { return true;  }
        
        /// <summary>
        /// Target inputs
        /// </summary>
        public NodeInput[] TargetInputs
        {
            get
            {
                if (targetInputs == null || targetInputs.Count != targetInputsids.Count)
                {
                    targetInputs = new List<NodeInput>();
                    for (int i = 0; i < targetInputsids.Count; i++)
                    {
                        string currentid = targetInputsids[i];
                        if (!string.IsNullOrEmpty(currentid))
                        {
                            if (Parent != null && Parent.WorkingGraph != null)
                            {
                                Node n = Parent.WorkingGraph.FindNode(currentid);
                                if (n != null) targetInputs.Add(n.InputPort);

                            }
                        }
                    }
                }

                return targetInputs.ToArray();
            }
        }

        /// <summary>
        /// Sort inputs depending on node position on Y
        /// </summary>
        public void SortInputs()
        {
            if (TargetInputs != null )
            {
                targetInputs.Sort((NodeInput x, NodeInput y) => x.rect.y.CompareTo(y.rect.y));
                for (int i = 0; i < targetInputs.Count; i++)
                {
                    targetInputs[i].Order = i;
                }
                if (Parent != null)
                    Parent.OnSortTargets();
            }
        }

        /// <summary>
        /// Draws this element to GUI Layer
        /// </summary>
        public override void Draw()
        {
            UpdatePosition();

            GUI.Box(rect, "", Style);

            DrawLines();
        }

        /// <summary>
        /// Draws all lines to GUI Layer
        /// </summary>
        protected void DrawLines()
        {
            if (TargetInputs.Length > 0)
            {
                for (int i = 0; i < targetInputs.Count; i++)
                {
                    DrawLine(GetPosition(), targetInputs[i].GetPosition(), targetInputs[i]);
                }
            }
        }

        /// <summary>
        /// Draws a line for an Node input
        /// </summary>
        /// <param name="Start">Start position</param>
        /// <param name="End">End position</param>
        /// <param name="Target">Input target</param>
        public virtual void DrawLine(Vector2 Start , Vector2 End , NodeInput Target)
        {
            MoonGUI.DrawLine(Start, End, Color.white, true);
        }
        
        // Events

        public override void MouseDown(Event e, MoonEditorCanvas canvas)
        {

            if (EditorApplication.isPlaying) return;

            int btn = Event.current.button;

            if (InPort(e.mousePosition))
            {
                if (btn == 0 )
                {
                    canvas.ActiveOutput = this;
                }
            }   
        }
        
        /// <summary>
        /// Clears a node input target
        /// </summary>
        /// <param name="input">Node input</param>
        public void ClearTarget(NodeInput input)
        {
            if (input == null) return;

            if  (targetInputs.Contains(input)) 
            {
                targetInputsids.Remove(input.Parent.ID);
                targetInputs.Remove(input);
            }
            
            input.SetSourceOutput(null);
            input.OnRemoveConnection(this);
            input.Parent.OnRemovedConnection(Parent);
            SortInputs();
        }
        
        /// <summary>
        /// Clears all Targets 
        /// </summary>
        /// <param name="OutputOnly">Should clear from this output only ?</param>
        public void ClearAll( bool OutputOnly = false )
        {
            if (OutputOnly)
            {
                if (targetInputs != null)
                    targetInputs.Clear();
                targetInputsids.Clear();

            }
            else
            {
                for (int i = 0; i < TargetInputs.Length; i++)
                {
                    ClearTarget(targetInputs[i]);
                }
            }
        }

        /// <summary>
        /// set a new target
        /// </summary>
        /// <param name="input">Input target</param>
        public void SetTarget(NodeInput input)
        {
            if (input.Parent != Parent && !targetInputs.Contains(input) && CanAddInput(input))
            {
                targetInputsids.Add(input.Parent.ID);
                targetInputs.Add(input);
                input.SetSourceOutput(this);
                input.OnAddConnection(this);
                input.Parent.OnAddedConnection(Parent);
                SortInputs();
            }
        }

    }
}