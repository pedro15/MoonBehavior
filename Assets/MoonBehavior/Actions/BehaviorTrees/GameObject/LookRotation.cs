using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "GameObject" , Description = "Rotates the transform to the specified forward direction" ,
        Name = "Look Rotation")]   
    public class LookRotation : Task
    {
        public MemoryItem ForwardVector = new MemoryItem(ItemType.VECTOR3);

        public MemoryItem LookSpeed = new MemoryItem(ItemType.FLOAT);

        public override TaskResult OnExecute(MoonAI ai)
        {
            Vector3 forward = ForwardVector.GetValue<Vector3>(ai.Memory);

            float speed = LookSpeed.GetValue<float>(ai.Memory);

            if (forward != Vector3.zero)
            {
                Quaternion tres = Quaternion.LookRotation(forward);
                ai.transform.rotation = Quaternion.Slerp(ai.transform.rotation, tres, speed * Time.deltaTime);
            }
            return TaskResult.Success;
        }
    }
}