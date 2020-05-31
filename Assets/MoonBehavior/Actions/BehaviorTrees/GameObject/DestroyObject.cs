using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "GameObject" , Description = "Destroys an Object from the memory")]
    public class DestroyObject : Task
    {

        public MemoryItem ObjectKey = new MemoryItem(ItemType.STRING);
        MoonMemory Mem;

        public override void OnEnter(MoonAI ai)
        {
            Mem = ai.GetComponent<MoonMemory>();
        }

        public override TaskResult OnExecute(MoonAI ai)
        {
            string k = ObjectKey.GetValue<string>(Mem);

            if (!string.IsNullOrEmpty(k))
            {
                Object obj = Mem.GetValue<Object>(k) as Object;

                if (obj != null)
                {
                    Destroy(obj);
                }
                return TaskResult.Success;
            }

            return TaskResult.Failure;
        }
    }
}