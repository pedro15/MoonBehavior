using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MoonBehaviorEditor.Core.Graphs
{
    /// <summary>
    /// Graph extensions methods
    /// </summary>
    static class GraphExtensions
    {
        /// <summary>
        /// Finds all descendents nodes
        /// </summary>
        /// <param name="n">Node</param>
        /// <returns>Descendents nodes</returns>
        public static IEnumerable<Node> Descendents(this Node n)
        {
            for (int i = 0; i < n.OutputPorts.Length; i++)
            {
                NodeOutput o = n.OutputPorts[i];
                for (int j = 0; j < o.TargetInputs.Length; j++)
                {
                    Node child = o.TargetInputs[j].Parent;
                    yield return child;
                    foreach (Node children in Descendents(child))
                    {
                        yield return children;
                    }
                }
            }
        }

        /// <summary>
        /// Finds all asendents nodes
        /// </summary>
        /// <param name="n">Node</param>
        /// <returns>Asendents nodes</returns>
        public static IEnumerable<Node> Asendents(this Node n)
        {
            var parentnode = n.InputPort.SourceOutput != null ? n.InputPort.SourceOutput.Parent : null;
            if (parentnode != null)
            {
                yield return parentnode;
                foreach (Node nx in Asendents(parentnode)) yield return nx;
            }
        }

        /// <summary>
        /// Check if the list contains an specifyc node
        /// </summary>
        /// <param name="n">Node</param>
        /// <returns>true if the node are inside the list</returns>
        public static bool ContainsNode(this IEnumerable<Node> list , Node n )
        {
            return ContainsNode(list, n.ID);
        }

        /// <summary>
        /// Check if the list contains an specifyc node
        /// </summary>
        /// <param name="nodeID">Node id</param>
        /// <returns></returns>
        public static bool ContainsNode(this IEnumerable<Node> list , string nodeID)
        {
            return list.ToList().FindIndex((Node n) => string.Equals(n.ID, nodeID)) >= 0; 
        }

        /// <summary>
        /// Are the given point inside any node of the list ?
        /// </summary>
        /// <param name="nodelist">List</param>
        /// <param name="Point">Mouse point</param>
        /// <returns>Inside boolean</returns>
        public static bool ContainsPoint(this List<Node> nodelist , Vector2 Point )
        {
            bool inNodes = false;

            if (nodelist.Count > 0)
            {
                nodelist.ForEach((Node n) =>
                {
                    if (!inNodes && (n.InNode(Point) && !n.InPorts(Point))) inNodes = true;
                });
            }

            return inNodes;
        }
    }
}
