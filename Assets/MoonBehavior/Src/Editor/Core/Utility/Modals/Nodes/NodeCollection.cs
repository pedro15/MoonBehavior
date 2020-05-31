using System.Collections.Generic;
using System.Linq;
using MoonBehaviorEditor.Core.Graphs;

namespace MoonBehaviorEditor.Modals.Nodes
{
    /// <summary>
    /// Collection of nodes with an category used in add node window
    /// </summary>
    public class NodeCollection
    {
        /// <summary>
        /// Node collection constructor
        /// </summary>
        /// <param name="Items">Collection nodes</param>
        /// <param name="Category">Collection category</param>
        public NodeCollection(NodeItem[] Items, string Category)
        {
            Nodes = Items;
            CategoryName = Category;
        }

        /// <summary>
        /// Are the collection expanded ?
        /// </summary>
        public bool FoldOut = true;

        /// <summary>
        /// Collection container maximun screen Y Position 
        /// </summary>
        public float MaxY { get; set; }

        /// <summary>
        /// Node list
        /// </summary>
        public NodeItem[] Nodes { get; private set;  }

        /// <summary>
        /// Category for this collection
        /// </summary>
        public string CategoryName { get; private set;  }

        public const float NodeHeight = 25;
        
        public float GetHeight()
        {
            return FoldOut ? NodeHeight + (Nodes.Length * NodeHeight) : NodeHeight;
        }

        /// <summary>
        /// Gets all catorgorys from nodes array
        /// </summary>
        /// <param name="items">Nodes</param>
        /// <returns>Categorys array</returns>
        private static string[] GetCategorys(NodeItem[] items)
        {
            List<string> cats = new List<string>();
            for (int i = 0; i < items.Length; i++)
            {
                if (!cats.Contains(items[i].Category))
                {
                    cats.Add(items[i].Category);
                }
            }
            return cats.ToArray();
        }

        /// <summary>
        /// Get all collections elements height
        /// </summary>
        /// <param name="coll">Collections element</param>
        /// <returns>Total height</returns>
        public static float GetCollectionsHeight( NodeCollection[] coll )
        {
           return coll.Select((NodeCollection c) => c.GetHeight()).Sum();
        }

        /// <summary>
        /// Gets all collection aviable
        /// </summary>
        /// <returns>Node collections</returns>
        public static NodeCollection[] GetAllCollections(string searchfilter , NodeGraph Graph )
        {
            NodeItem[] AllNodes = NodeItem.GetAllNodes(Graph).Where((NodeItem n) =>
            string.IsNullOrEmpty(searchfilter) || (n.Name.ToLower().
                Contains(searchfilter.ToLower())|| n.Category.ToLower().Contains(searchfilter.ToLower()))).ToArray();
            

            string[] Categorys = GetCategorys(AllNodes);

            List<NodeCollection> Collections = new List<NodeCollection>();
            
            for (int i = 0; i < Categorys.Length; i++)
            {
                IEnumerable<NodeItem> CatNodes = AllNodes.Where((NodeItem n) => string.Equals(n.Category, Categorys[i]));
                Collections.Add(new NodeCollection(CatNodes.ToArray(), Categorys[i]));
            }

            return Collections.ToArray();
        }
        
    }
}