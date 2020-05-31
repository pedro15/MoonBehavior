using System;
using UnityEditor;

namespace MoonBehaviorEditor.Core.Graphs
{
    /// <summary>
    /// Attribute used to build graphs modules
    /// </summary>
    [AttributeUsage(AttributeTargets.Class , AllowMultiple = false)]
    public class GraphModuleAttribute : Attribute
    {
        /// <summary>
        /// Module name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Module Type
        /// </summary>
        public Type ModuleType { get; private set; }

        /// <summary>
        /// Module GraphType WorkSpace
        /// </summary>
        public Type GraphType { get; private set; }

        /// <summary>
        /// Exports folder name to save asset graphs
        /// </summary>
        public virtual string ExportFolderName { get; set; }
        
        /// <summary>
        /// Graph module constructor
        /// </summary>
        /// <param name="Name">Module name</param>
        public GraphModuleAttribute(string Name , Type ModuleType )
        {
            this.Name = ObjectNames.NicifyVariableName(Name);
            this.ModuleType = ModuleType;
        }

        /// <summary>
        /// Inizializes the GrapHModule
        /// </summary>
        /// <param name="GraphType">WorkSpace Type</param>
        public void Init(Type GraphType)
        {
            this.GraphType = GraphType;
        }
    }
}