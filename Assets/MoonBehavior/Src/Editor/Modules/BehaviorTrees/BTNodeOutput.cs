using System.Linq;

using UnityEngine;

using MoonBehaviorEditor.Core.Graphs;
using MoonBehavior.BehaviorTrees;

namespace MoonBehaviorEditor.BehaviorTrees
{
    /// <summary>
    /// Behavior Tree Node: Output
    /// </summary>
    [System.Serializable]
    public class BTNodeOutput : NodeOutput
    {
        public BTNodeOutput(BTNode parent) : base(parent)
        {

        }

        /// <summary>
        /// Custom offset
        /// </summary>
        protected override Vector2 GetOffset()
        {
            return new Vector2(Parent.rect.width + 8f, (Parent.rect.height / 2) - rect.height / 2);
        }

        /// <summary>
        /// Output Style
        /// </summary>
        public override GUIStyle Style
        {
            get
            {
                return MoonResources.DotStyle;
            }
        }

        /// <summary>
        /// Custom CanAddInput
        /// if the node is decision and has the 'SingleChild' Attribute you can't add more than one child node
        /// </summary>
        /// <param name="input">Input</param>
        public override bool CanAddInput(NodeInput input)
        {
            BTNode _parent = (BTNode)Parent;
            if (_parent != null)
            {
                bool singlechild = _parent.IsSingleChild;
                bool result =  (singlechild && _parent.Descendents().Count() == 0) | (!singlechild);
                if (!result)
                {
                    Debug.LogWarning("[MoonBehavior] Can't add more targets: This node is a single child node.");
                }
                return result;
            }

            return true;
        }

        /// <summary>
        /// Custom Line drawing
        /// </summary>
        /// <param name="Start">Start point</param>
        /// <param name="End">End point</param>
        /// <param name="Target">Target point</param>
        public override void DrawLine(Vector2 Start, Vector2 End, NodeInput Target)
        {
            MoonGUI.DrawLine(Start, End, Color.white, new GUIContent(Target.Order.ToString(), "Order"), true);
        }
    }
}
