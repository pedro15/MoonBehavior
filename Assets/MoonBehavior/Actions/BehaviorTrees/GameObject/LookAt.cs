using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "GameObject" , Description = "Rotates the transform's forward vector to point to Target position")]
    public class LookAt : Task
    {
        public MemoryItem TargetKey = new MemoryItem(ItemType.STRING);

        public MemoryItem LookSpeed = new MemoryItem(ItemType.FLOAT);

        private GameObject TargetGo { get; set; }

        public override void OnEnter(MoonAI ai)
        {
            string key = TargetKey.GetValue<string>(ai.Memory);
            TargetGo = ai.Memory.GetValue<GameObject>(key);
        }

        public override TaskResult OnExecute(MoonAI ai)
        {
            if (TargetGo != null)
            {
                Vector3 dir = (TargetGo.transform.position - ai.transform.position);

                if (dir != Vector3.zero)
                {
                    Quaternion tr = Quaternion.LookRotation(dir);
                    ai.transform.rotation = Quaternion.Slerp(ai.transform.rotation, tr, LookSpeed.GetValue<float>(ai.Memory)
                        * Time.deltaTime);
                }
                return TaskResult.Success;
            }
            return TaskResult.Failure;
        }
    }
}