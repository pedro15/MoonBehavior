using System.Collections.Generic;
using UnityEngine;

namespace MoonBehavior.BehaviorTrees
{
    [System.Serializable]
    public abstract class Decision : Task
    {
        /// <summary>
        /// Child nodes
        /// </summary>
        [SerializeField,HideInInspector]
        private Task[] _Childs; 
        
        public Task[] Childs { get { return _Childs; } }
        
        /// <summary>
        /// Reemplaze all childs with new Task array
        /// </summary>
        /// <param name="other">child list</param>
        public void SetChilds(Task[] other)
        {
            _Childs = other;
        }

        /// <summary>
        /// Reemplaze all childs with new child list
        /// </summary>
        /// <param name="other">child list</param>
        public void SetChilds(List<Task> other)
        {
            SetChilds(other.ToArray());
        }

        public Decision() { }
    }
}