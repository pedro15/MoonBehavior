using UnityEngine;
using UnityEditor;
using MoonBehaviorEditor.Core.Graphs;
using System.Collections.Generic;
using MoonBehavior.BehaviorTrees;
using System.Linq;

namespace MoonBehaviorEditor.BehaviorTrees
{
    /// <summary>
    /// Behavior tree node
    /// </summary>
    [System.Serializable]
    public class BTNode : Node
    {
        /// <summary>
        /// Stored Behavior tree node outputs
        /// </summary>
        [SerializeField]
        private List<BTNodeOutput> outputs = new List<BTNodeOutput>();
        
        /// <summary>
        /// Stored Behavior tree node input
        /// </summary>
        [SerializeField]
        private BTNodeInput inputPort;

        /// <summary>
        /// Behvaior Tree Input Port
        /// </summary>
        public override NodeInput InputPort
        {
            get
            {
                return inputPort;
            }
        }

        /// <summary>
        /// Behavior Tree OutPut Ports 
        /// </summary>
        public override NodeOutput[] OutputPorts
        {
            get
            {
                return outputs.ToArray();
            }
        }

        /// <summary>
        /// Default Node Size
        /// </summary>
        protected override Vector2 DefaultSize
        {
            get
            {
                return new Vector2(100, 40);
            }
        }

        /// <summary>
        /// Is this BehaviorTree Node an Decision node ?
        /// </summary>
        public bool IsDecision
        {
            get { return Task is Decision; }
        }

        /// <summary>
        /// Is this node a single child node ??
        /// </summary>
        public bool IsSingleChild
        {
            get
            {
                if (IsDecision)
                {
                    Decision dec = (Decision)Task;
                    var SingleChildattr = MoonReflection.GetAttribute(dec.GetType(), typeof(SingleChildAttribute), true);
                    return SingleChildattr != null;
                }
                return false;
            }
        }

        /// <summary>
        /// Is this Node the root node ?
        /// </summary>
        public bool IsRoot
        {
            get
            {
                return ((BTGraph) WorkingGraph).CheckRoot(ID);
            }
        }

        /// <summary>
        /// Stored runtime task instance for debugging
        /// </summary>
        [System.NonSerialized]
        private ScriptableObject _runTimeTask;
        /// <summary>
        /// Stored instance id for checking AI on debugging
        /// </summary>
        [System.NonSerialized]
        private int LastSelectedID = -1;

        public override ScriptableObject Task
        {
            get
            {
                if (EditorApplication.isPlaying)
                {
                    Task m_task = task as Task;

                    if (m_task != null)
                    {
                        BTGraph bgraph = WorkingGraph as BTGraph;

                        if (bgraph != null)
                        {
                            MoonAI ai = bgraph.SelectedAI;
                            int currid = Selection.activeInstanceID;

                            if (ai != null && LastSelectedID != currid)
                            {
                                MoonBT bt = ai.CurrentBT;
                                if (bt != null && bt.Tasks != null && bt.Tasks.Count > 0)
                                {
                                    var found = bt.FindTask(m_task.Id);
                                    if (found != null)
                                    {
                                        _runTimeTask = found;
                                        LastSelectedID = currid;
                                    }
                                }
                            }
                        }
                    }
                    if (_runTimeTask != null)
                        return _runTimeTask;
                }else if (_runTimeTask != null)
                {
                    _runTimeTask = null; 
                }
                return task;
            }
        }

        /// <summary>
        /// Node style
        /// </summary>
        public override GUIStyle Style
        {
            get
            {
                if (EditorApplication.isPlaying && Task != null)
                {
                    
                    Task t = Task as Task;
                    TaskResult tres = t.LastResult;
                    int colindex = 0;
                    if (tres == TaskResult.Success) colindex = 3;
                    else if (tres == TaskResult.Running) colindex = 4;
                    else if (tres == TaskResult.Failure) colindex = 6;
                    return MoonResources.GetNodeStyle(Selected, colindex);
                }

                int colorindex = (IsDecision) ? ((IsRoot) ? 5 : 1) : 2;
                return MoonResources.GetNodeStyle(Selected, colorindex);

            }
        }

        

        /// <summary>
        /// Adds a output to Behavior tree node
        /// </summary>
        public void AddOutput()
        {
            if (IsDecision)
            {
                BTNodeOutput o = new BTNodeOutput(this);
                outputs.Add(o);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (IsDecision && outputs.Count > 0 )
            {
                BTNodeOutput output = outputs[0];
                if (IsSingleChild && output.TargetInputs.Length > 1)
                {
                    for (int j = 1; j < output.TargetInputs.Length; j++)
                    {
                        Debug.Log("[MoonBehavior] Single child node; Removing extra connections: " + output.TargetInputs[j].Parent.name);
                        output.ClearTarget(output.TargetInputs[j]);
                    }
                }
            }
        }

        protected override void OnInit()
        {
            Task t = Task as Task;
            t.GenerateID();
            inputPort = new BTNodeInput(this);
            AddOutput();
        }

        public override void OnSortTargets()
        {
            if (IsDecision)
            {
                Decision dec = (Decision)Task;
                List<Task> m_tasks = new List<Task>();
                NodeOutput output = outputs[0];

                m_tasks = output.TargetInputs.Select((NodeInput input) => (Task)input.Parent.Task).ToList();

                dec.SetChilds(m_tasks);
            }
        }
        
        /// <summary>
        /// Custom node context menu
        /// </summary>
        public override Dictionary<string, System.Action<object>> ContextMenu()
        {
            Dictionary<string, System.Action<object>> dic = new Dictionary<string, System.Action<object>>();

            if (IsDecision)
            {
                dic.Add("Set root", (object node) =>
                {
                    ((BTGraph) WorkingGraph).SetRoot(this);
                });
            }

            dic.Add("$", null);
            dic.Add("Edit script", (object node) => {
                Node nodeobj = (Node)node;
                MonoScript scriptinstance = MonoScript.FromScriptableObject(nodeobj.Task);
                AssetDatabase.OpenAsset(scriptinstance);
            });

            return dic;
        }
    }
}