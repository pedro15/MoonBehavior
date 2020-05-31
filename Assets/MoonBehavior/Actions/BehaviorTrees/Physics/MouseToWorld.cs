using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "Physics", Description = "Casts a raycast from the Main Camera's mouse position " +
        "to world point, and saves the position where the ray hits" , Name = "Mouse to world")]
    public class MouseToWorld : Task
    {

        public MemoryItem SaveKey = new MemoryItem(ItemType.STRING);

        public MemoryItem Mask = new MemoryItem(ItemType.LAYERMASK);

        public MemoryItem RayDistance = new MemoryItem(ItemType.FLOAT);

        public override TaskResult OnExecute(MoonAI ai)
        {

            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            float raydist = RayDistance.GetValue<float>(ai.Memory);

            LayerMask mask = Mask.GetValue<LayerMask>(ai.Memory);

            string savek = SaveKey.GetValue<string>(ai.Memory);

            if (!string.IsNullOrEmpty(savek))
            {
                if (Physics.Raycast(r.origin, r.direction, out hit, raydist, mask))
                {
                    ai.Memory.SetValue(savek, hit.point);
                }

                return TaskResult.Success;
            }
            
            return TaskResult.Failure;
        }
    }
}