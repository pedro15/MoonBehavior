using System;

namespace MoonBehavior.BehaviorTrees
{
    /// <summary>
    /// Attribute to limit an Decision to have only one child task.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class , AllowMultiple = false )]
    public class SingleChildAttribute : Attribute
    {

    }
}