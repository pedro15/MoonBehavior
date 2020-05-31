using UnityEngine;
using MoonBehavior.Memory;
using UnityEngine.AI;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "Navigation" , Description = "Moves the AI GameObject to a target")]
    public class Move : Task
    {

        public MemoryItem Target = new MemoryItem(ItemType.VECTOR3);

        public MemoryItem StopDistance = new MemoryItem(ItemType.FLOAT);

        public MemoryItem Speed = new MemoryItem(ItemType.FLOAT);
        
        private Vector3 TargetPos { get; set; }

        [HideOnDebug]
        private NavMeshAgent agent { get; set; }

        [HideOnDebug]
        private Vector3 velocity { get; set; }

        [HideOnDebug]
        private float speed { get; set; }

        [HideOnDebug]
        private GameObject TargetGo { get; set; }
        
        public override void OnEnter(MoonAI ai)
        {
            velocity = Vector3.zero;
            agent = ai.GetComponent<NavMeshAgent>();
            speed = Speed.GetValue<float>(ai.Memory);
            TargetGo = ai.Memory.GetValue<GameObject>(Target.key);

        }

        public override TaskResult OnExecute(MoonAI ai)
        {
            TargetPos = (TargetGo != null) ? TargetGo.transform.position : Target.GetValue<Vector3>(ai.Memory);
            
            Vector3 m_dir = (TargetPos - ai.transform.position);
            float stopdist = StopDistance.GetValue<float>(ai.Memory);
            
            if (agent != null)
            {
                agent.speed = speed;
                agent.stoppingDistance = stopdist;

                NavMeshHit h;
                if (NavMesh.SamplePosition(TargetPos, out h, 1.0f, NavMesh.AllAreas))
                    agent.destination = TargetPos;
                else return TaskResult.Failure;

                if (m_dir.magnitude <= stopdist)
                {
                    agent.velocity = Vector3.Lerp(agent.velocity, Vector3.zero, agent.acceleration);
                    return TaskResult.Success;
                }

                return TaskResult.Running;
            }else
            {
                Vector3 desiredVelocity = m_dir.normalized * speed;
                Vector3 Steering = desiredVelocity - velocity;
                velocity = Vector3.ClampMagnitude(velocity + Steering, speed);
                ai.transform.position += velocity;

                return (m_dir.magnitude <= stopdist) ? TaskResult.Success : TaskResult.Running;
            }
        }
    }
}