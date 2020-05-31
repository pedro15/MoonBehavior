using MoonBehavior.Memory;
using UnityEngine.AI;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "Navigation" , Description = "Set a new speed to the navmesh agent component")]
    public class SetAgentSpeed : Task
    {
        public MemoryItem Speed = new MemoryItem(ItemType.FLOAT);

        [System.NonSerialized]
        private NavMeshAgent Agent;

        private MoonMemory Mem; 
        
        public override void OnEnter(MoonAI ai)
        {
            Mem = ai.GetComponent<MoonMemory>(); 
            Agent = ai.GetComponent<NavMeshAgent>();
        }

        public override TaskResult OnExecute(MoonAI ai)
        {
            float speed = Speed.GetValue<float>(Mem);
            if (Agent != null)
            {
                Agent.speed = speed;
                return TaskResult.Success;
            }
            return TaskResult.Failure;
        }
    }
}