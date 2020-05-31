using System;
namespace MoonBehavior
{
    /// <summary>
    /// Hides a field or a property on the debug box when the editor is in PlayMode.
    /// Only Applicable on Custom Nodes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property , AllowMultiple = false)]
    public class HideOnDebugAttribute : Attribute { }
}