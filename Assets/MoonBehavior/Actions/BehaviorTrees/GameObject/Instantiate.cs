using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "GameObject" , Description = "Clones de object to the scene and saves the clone to the Memory Component")]
    public class Instantiate : Task
    {
        public MemoryItem _Object = new MemoryItem(ItemType.OBJECT);

        public MemoryItem Position = new MemoryItem(ItemType.VECTOR3);

        public MemoryItem Rotation = new MemoryItem(ItemType.VECTOR3);

        private Object obj;

        public MemoryItem SaveKey = new MemoryItem(ItemType.STRING);

        public override void OnEnter(MoonAI ai)
        {
            obj = _Object.GetValue<Object>(ai.Memory);
        }

        public override TaskResult OnExecute(MoonAI ai)
        {
            if (obj != null )
            {
                Object Clone = Instantiate(obj , Position.GetValue<Vector3>(ai.Memory) , 
                    Quaternion.Euler(Rotation.GetValue<Vector3>(ai.Memory)));

                if (Clone is GameObject)
                {
                    string key = SaveKey.GetValue<string>(ai.Memory);

                    if (!string.IsNullOrEmpty(key))
                    {
                        ai.Memory.SetValue(key, Clone);
                    }

                    return TaskResult.Success;
                }
#if UNITY_EDITOR
                else
                {
                    Debug.LogError("[MoonBehavior] Instantiate node only supports GameObject!");
                }
#endif
            }
            return TaskResult.Failure;
        }
    }
}