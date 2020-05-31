using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System.Linq;

namespace MoonBehavior.BehaviorTrees
{
    /// <summary>
    /// Result of a task
    /// </summary>
    public enum TaskResult : int
    {
        /// <summary>
        /// The Task is not in execution
        /// </summary>
        None = 0,
        /// <summary>
        /// Task executed correctly
        /// </summary>
        Success = 1,
        /// <summary>
        /// Task is running
        /// </summary>
        Running = 2,
        /// <summary>
        /// Fail execution of a task
        /// </summary>
        Failure = 3
    }

    /// <summary>
    /// BehaviorTree Base
    /// </summary>
    [System.Serializable]
    public class MoonBT : ScriptableObject
    {
        /// <summary>
        /// Behavior Tree Description
        /// </summary>
        [TextArea()]
        public string Description;

        /// <summary>
        /// Is the behavior tree valid ?
        /// </summary>
        public bool IsValid()
        {
            return Root != null;
        }

        /// <summary>
        /// BehaviorTree Root task
        /// </summary>
        public Decision Root;
        
        /// <summary>
        /// Behavior tree task list
        /// </summary>
        public List<Task> Tasks = new List<Task>();

        /// <summary>
        /// Current Behavior Tree result
        /// </summary>
        public TaskResult BTResult { get; private set; }
        
        /// <summary>
        /// Finds a task by id
        /// </summary>
        /// <param name="id">Task found</param>
        /// <returns></returns>
        public Task FindTask(string id)
        {
            Task foundtask = Tasks.FirstOrDefault((Task t) => string.Equals(t.Id, id));
            if (!foundtask)
            {
                foundtask = (string.Equals(id, Root.Id) ? Root : null);
            }
            return foundtask;
        }
        
        /// <summary>
        /// Executes the behavior tree
        /// </summary>
        /// <param name="ai">AI component</param>
        public void ExecuteBT(MoonAI ai)
        {
            if (BTResult == TaskResult.None | BTResult == TaskResult.Running)
            {
                Profiler.BeginSample("MoonBehavior: Execute BehaviorTree (" + ai.name + ")" );
                BTResult = Root.Execute(ai);
                Profiler.EndSample();
            }
        }
        
        /// <summary>
        /// Prints the behavior tree to unity console
        /// </summary>
        public void LogBT()
        {
            PrintDecision(Root);
        }

        /// <summary>
        /// Prints a decision to unity console
        /// </summary>
        /// <param name="dec"></param>
        private void PrintDecision(Decision dec)
        {
            if ( dec.Childs == null || dec.Childs.Length == 0) return;

            for (int i = 0; i < dec.Childs.Length; i++)
            {
                Debug.Log(dec.GetType().Name + " --> " + dec.Childs[i].GetType());
                if (dec.Childs[i] is Decision)
                {
                    Decision de = (Decision)dec.Childs[i];
                    PrintDecision(de);
                }
            }
        }
        
        /// <summary>
        /// Duplicates a Behavior Tree
        /// </summary>
        /// <param name="other">Behavior tree to copy</param>
        /// <returns>Behavior tree copy</returns>
        public static MoonBT CopyBT(MoonBT other)
        {
            if (!other) return null;
            MoonBT bt = Instantiate(other);
            Decision _root = Instantiate(bt.Root);
            bt.Tasks = AddChilds(_root).ToList();
            bt.Root = _root;
            return bt;
        }
        
        private static IEnumerable<Task> AddChilds(Decision dec)
        {
            List<Task> dchilds = new List<Task>();

            if (dec.Childs.Length > 0)
            {
                for (int i = 0; i < dec.Childs.Length; i++)
                {
                    Task copyTask = Instantiate<Task>(dec.Childs[i]);

                    yield return copyTask;

                    if (copyTask is Decision)
                    {
                        foreach (Task d in AddChilds((Decision)copyTask))
                        {
                            yield return d;
                        }
                    }

                    dchilds.Add(copyTask);
                }
            }
            dec.SetChilds(dchilds);
        }
    }
}