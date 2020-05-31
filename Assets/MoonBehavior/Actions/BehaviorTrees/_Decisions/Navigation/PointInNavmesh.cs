using UnityEngine;
using MoonBehavior.Memory;
using UnityEngine.AI;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [SingleChild]
    [Info(Category = "Navigation" , Description = "Executes it's child node when the given point are inside navmesh" )]
    public class PointInNavmesh : Decision
    {
        public MemoryItem point = new MemoryItem(ItemType.VECTOR3);
        
        public override TaskResult OnExecute(MoonAI ai)
        {
            NavMeshHit hit;

            Vector3 Point = point.GetValue<Vector3>(ai.Memory);

            if (NavMesh.SamplePosition(Point,out hit , 1.0f , NavMesh.AllAreas))
            {
                if (Childs.Length > 0)
                    return Childs[0].Execute(ai);
            }
            
            return TaskResult.Failure;
        }
    }
}