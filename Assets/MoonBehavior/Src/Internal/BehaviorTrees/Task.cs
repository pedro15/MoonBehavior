using System.Collections.Generic;
using MoonBehavior.Memory;
using UnityEngine;

namespace MoonBehavior.BehaviorTrees 
{
    [System.Serializable]
    public abstract class Task  : ScriptableObject
    {
        /// <summary>
        /// ID of this task
        /// </summary>
        [HideOnDebug]
        public string Id { get { return TaskID; } }

        [SerializeField, HideInInspector]
        private string TaskID;
        
        [System.NonSerialized,HideOnDebug]
        private bool _Started;
        
        public bool Inizialized { get { return _Started; } }

        [SerializeField,HideOnDebug]
        private MemoryItem _Priority = new MemoryItem(ItemType.FLOAT);
        
        private float m_priority { get; set; }
        private bool PriorityLoaded { get; set; }

        /// <summary>
        /// Task Priority
        /// </summary>
        public float GetPriority(MoonAI ai)
        {
            if (!PriorityLoaded)
            {
                m_priority = _Priority.GetValue<float>(ai.Memory);
                PriorityLoaded = true;
            }
            return m_priority;
        }

        /// <summary>
        /// Last task Result
        /// </summary>
        public TaskResult LastResult { get; private set; }
        
        /// <summary>
        /// Execution of the task and it's result
        /// </summary>
        /// <returns>Task result</returns>
        public abstract TaskResult OnExecute(MoonAI ai);
        
        /// <summary>
        /// Called when enters the task
        /// </summary>
        /// <param name="ai">AI agent</param>
        public virtual void OnEnter(MoonAI ai) { }
        
        /// <summary>
        /// Called when tasks returns Success or Failure
        /// </summary>
        /// <param name="ai">AI Agent</param>
        public virtual void OnExit(MoonAI ai) { }
        
        public TaskResult Execute( MoonAI ai)
        {
            if (!Inizialized)
            {
                OnEnter(ai);
                _Started = true;
            }

            LastResult = OnExecute(ai);
            
            
            if (LastResult == TaskResult.Success || LastResult == TaskResult.Failure)
            {
                OnExit(ai);
                _Started = false;
            }else
            {
                LastResult = TaskResult.Running;
            }

            return LastResult;
        }

       

        /// <summary>
        /// Generates a new id for this task
        /// </summary>
        public void GenerateID ()
        {
            TaskID = System.Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return GetType().Name; 
        }

    }

    public class TaskComparer : IEqualityComparer<Task>
    {
        public bool Equals(Task x, Task y)
        {
            return string.Equals(x.Id, y.Id);
        }

        public int GetHashCode(Task obj)
        {
            return obj.GetHashCode();
        }
    }
}