using UnityEngine;
using MoonBehaviorEditor.Core.Graphs;

namespace MoonBehaviorEditor.BehaviorTrees
{
    /// <summary>
    /// BehaviorTree Node: Input
    /// </summary>
    [System.Serializable]
    public class BTNodeInput : NodeInput
    {
        public BTNodeInput(BTNode parent) : base(parent)
        {

        }
        
        /// <summary>
        /// Custom Offset
        /// </summary>
        protected override Vector2 GetOffset()
        {
            return new Vector2(-0.5f, (Parent.rect.height / 2) - (rect.height * 0.5f));
        }

        /// <summary>
        /// Custom Valid connection
        /// in behavior tress you can't conect to a parent node
        /// </summary>
        protected override bool ValidConnection(NodeOutput output)
        {
            BTNode parentnode = Parent as BTNode;

            if (parentnode.IsDecision)
            {
                if (output.Parent.Asendents().ContainsNode(Parent))
                {
                    Debug.LogWarning("[MoonBehavior] You can't connect to a parent node !");
                    return false; 
                }
            }

            return true; 
        }
        
        /// <summary>
        /// Input style
        /// </summary>
        public override GUIStyle Style
        {
            get
            {
                return GUI.skin.FindStyle("flow shader in 0");
            }
        }
    }
}
