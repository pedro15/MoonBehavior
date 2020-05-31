using UnityEngine;
using UnityEditor;

namespace MoonBehaviorEditor.Core.Graphs
{
    /// <summary>
    /// Node input port base class
    /// </summary>
    [System.Serializable]
    public abstract class NodeInput : NodePort
    {
        /// <summary>
        /// Node input constructor
        /// </summary>
        /// <param name="parent"></param>
        public NodeInput(Node parent) : base(parent)
        {
            rect.size = Vector2.one * 12f;    
        }

        /// <summary>
        /// Input order
        /// </summary>
        public int Order;

        /// <summary>
        /// Does the connection with the output is valid ?
        /// </summary>
        /// <param name="output">Output port</param>
        /// <returns>Connection valid boolean</returns>
        protected abstract bool ValidConnection(NodeOutput output);

        /// <summary>
        /// Stored output node id
        /// </summary>
        [SerializeField]
        private string _outputNodeID;
        
        /// <summary>
        /// Stored output node index
        /// </summary>
        [SerializeField]
        private int _outputIndex = -1;
        
        /// <summary>
        /// source output instance
        /// </summary>
        [System.NonSerialized]
        private NodeOutput _sourceOutput = null; 

        /// <summary>
        /// Source output port
        /// </summary>
        public NodeOutput SourceOutput
        {
            get
            {
                if (_sourceOutput == null )
                {
                    Node n = Parent.WorkingGraph.FindNode(_outputNodeID);
                    if (n != null )
                    {
                        _sourceOutput = n.FindOutput(_outputIndex);

                    }
                }
                return _sourceOutput;  
            }
        }
        
        /// <summary>
        /// Called when the user clicks the input
        /// </summary>
        /// <param name="e">User event</param>
        /// <param name="canvas">Canvas</param>
        public override void MouseDown(Event e , MoonEditorCanvas canvas)
        {
            if (EditorApplication.isPlaying) return;

            if (InPort(e.mousePosition))
            {
                int btn = e.button;
                if (btn == 0 )
                {
                    NodeOutput activeoutput = canvas.ActiveOutput;
                    if (activeoutput != null )
                    {
                        SetSourceOutput(activeoutput);
                        canvas.ActiveOutput = null;
                    }
                }else if (btn == 1)
                {
                    ClearOutput();
                }
            }
        }

        /// <summary>
        /// Returns the input position
        /// </summary>
        public override Vector2 GetPosition()
        {
            return new Vector2(rect.center.x - 12, rect.center.y);
        }


        /// <summary>
        /// Called when an output connects to this input
        /// </summary>
        /// <param name="output"></param>
        public virtual void OnAddConnection( NodeOutput output) { }

        /// <summary>
        /// Called when disconnect an output from this input
        /// </summary>
        /// <param name="output"></param>
        public virtual void OnRemoveConnection(NodeOutput output) { }

        // Public api

        /// <summary>
        /// Set source output port
        /// </summary>
        /// <param name="output">Output port</param>
        public void SetSourceOutput(NodeOutput output)
        {

            if (output != null && !ValidConnection(output)) return; 
            
            if (output != null && output.Parent != null && output.Parent != Parent)
            {
                if (SourceOutput != output)
                {
                    ClearOutput();
                }

                _outputIndex = output.Index;
                _outputNodeID = output.Parent.ID;
                _sourceOutput = output;
                output.SetTarget(this);
            }
            else
            {
                _sourceOutput = null;
                _outputNodeID = string.Empty;
                _outputIndex = -1;
            }
        }

        /// <summary>
        /// Clears soruce output port
        /// </summary>
        public void ClearOutput()
        {

            if (SourceOutput != null)
            {
                SourceOutput.ClearTarget(this);
            }
        }
    }
}
