using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using MoonBehavior;
using MoonBehaviorEditor.Core.Graphs;

namespace MoonBehaviorEditor.Modals.Nodes
{
    /// <summary>
    /// Node item element used on add node window
    /// </summary>
    public class NodeItem
    {
        /// <summary>
        /// Stored node type
        /// </summary>
        private System.Type _NodeType;
        
        /// <summary>
        /// Node type
        /// </summary>
        public System.Type NodeType
        {
            get
            {
                return _NodeType; 
            }
        }
        
        /// <summary>
        /// Stored node name
        /// </summary>
        private string _Name; 

        /// <summary>
        /// Node name
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
        }

        /// <summary>
        /// Stored category name
        /// </summary>
        private string _Category;

        /// <summary>
        /// Category name
        /// </summary>
        public string Category
        {
            get { return _Category; }
        }

        /// <summary>
        /// Stored icon instance
        /// </summary>
        private Texture2D _icon;

        /// <summary>
        /// Node icon
        /// </summary>
        public Texture2D Icon
        {
            get
            {
                return _icon;
            }
        }

        /// <summary>
        /// Node item constructor
        /// </summary>
        /// <param name="NodeType"></param>
        public NodeItem(System.Type NodeType , NodeGraph Graph)
        {
            _NodeType = NodeType;

            InfoAttribute info = MoonReflection.GetNodeData(NodeType);
            
            _Name = (info != null) ? ( string.IsNullOrEmpty(info.Name) ? NodeType.Name : info.Name ) : NodeType.Name;

            _Category = (info == null  || string.IsNullOrEmpty(info.Category)) ? "No category" : info.Category;

            _icon = MoonResources.LoadIcon(Graph.GetIconName(NodeType.BaseType));

        }

        /// <summary>
        /// Gets all aviable nodes
        /// </summary>
        /// <returns>Nodes array</returns>
        public static NodeItem[] GetAllNodes(NodeGraph Graph)
        {
            System.Type[] NodeTypes = MoonReflection.GetAllDerivedTypes(Graph.TaskType).Where
                ((System.Type tt) => !tt.IsAbstract).ToArray();

            List<NodeItem> items = new List<NodeItem>();

            for (int i = 0; i < NodeTypes.Length; i++)
                items.Add(new NodeItem(NodeTypes[i] , Graph));

            items.Sort((NodeItem x, NodeItem y) => x.Category.CompareTo(y.Category));
            
            return items.ToArray();
        }
    }
}
