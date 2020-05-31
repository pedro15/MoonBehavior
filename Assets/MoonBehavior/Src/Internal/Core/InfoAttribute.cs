using System;

namespace MoonBehavior
{
    /// <summary>
    /// Node Info Attribute that allows to organize the nodes with names and categories and give descriptions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InfoAttribute : Attribute
    {
        public InfoAttribute() { }
        public virtual string Name { get; set; }
        public virtual string Category { get; set; }
        public virtual string Description { get; set; }
    }
}