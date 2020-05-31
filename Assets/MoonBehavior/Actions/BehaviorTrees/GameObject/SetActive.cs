using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Name = "Set Active" , Category = "GameObject" , Description = "Activates/Desactivates the assigned Game Object from the Memory component" +
        " (AI itself if no gameObject is found)")]
    public class SetActive : Task
    {
        public MemoryItem GameObjectKey = new MemoryItem(ItemType.STRING);

        public MemoryItem Active = new MemoryItem(ItemType.BOOLEAN);

        public override TaskResult OnExecute(MoonAI ai)
        {
            string key = GameObjectKey.GetValue<string>(ai.Memory);
            
            if (!string.IsNullOrEmpty(key))
            {
                GameObject go = ai.Memory.GetValue<GameObject>(key);
                if (!go) go = ai.gameObject;
                go.SetActive(Active.GetValue<bool>(ai.Memory));
                return TaskResult.Success;
            }
            return TaskResult.Failure;
        }
    }
}