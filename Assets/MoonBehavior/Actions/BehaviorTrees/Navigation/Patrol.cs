using System.Collections.Generic;
using UnityEngine;
using MoonBehavior.Memory;
using MoonBehavior.Navigation;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "Navigation")]
    public class Patrol : Task
    {
        public enum PatrolMode : int
        {
            Loop = 0 , PingPong = 1
        }

        public PatrolMode Mode;

        public MemoryItem PatrolPathKey = new MemoryItem(ItemType.STRING);

        public MemoryItem NextWayPointDistance = new MemoryItem(ItemType.FLOAT);

        public MemoryItem TargetPointSaveKey = new MemoryItem(ItemType.STRING);

        public MemoryItem CurrentDistanceSaveKey = new MemoryItem(ItemType.STRING);
        
        [HideOnDebug]
        private float m_NextWaypoint;

        [HideOnDebug]
        private string m_saveKey,m_currdistkey;

        [HideOnDebug]
        private Vector3[] Path = null;

        private int Index = 0;

        public override void OnEnter(MoonAI ai)
        {
            Index = 0;
            m_saveKey = TargetPointSaveKey.GetValue<string>(ai.Memory);
            m_currdistkey = CurrentDistanceSaveKey.GetValue<string>(ai.Memory);
            string pathkey = PatrolPathKey.GetValue<string>(ai.Memory);
            if (!string.IsNullOrEmpty(pathkey) && Path == null)
            {
                GameObject Pathgo = ai.Memory.GetValue<GameObject>(pathkey);
                if (Pathgo != null && Pathgo.scene.IsValid())
                {
                    var _path = Pathgo.GetComponent<MoonPath>();
                    if (_path != null)
                        Path = _path.Waypoints.ToArray();
                }
            }
            m_NextWaypoint = NextWayPointDistance.GetValue<float>(ai.Memory);
        }

        public override TaskResult OnExecute(MoonAI ai)
        {
            if (Path != null && Path.Length > 0  && !string.IsNullOrEmpty(m_saveKey))
            {
                while(Index < Path.Length)
                {

                    Vector3 Target = Path[Index];
                    Vector3 Dir = (Target - ai.transform.position);

                    ai.Memory.SetValue(m_saveKey, Target);
                    ai.Memory.SetValue(m_currdistkey, Dir.magnitude);

                    if (Dir.magnitude <= m_NextWaypoint)
                    {
                        Index++;
                    }

                    return TaskResult.Running;
                }

                Index = 0; 

                if (Mode == PatrolMode.PingPong)
                {
                    System.Array.Reverse(Path);
                }
                
                return TaskResult.Success;
            }

            return TaskResult.Failure;
        }
    }
}