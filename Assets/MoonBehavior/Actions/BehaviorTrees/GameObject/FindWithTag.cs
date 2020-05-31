using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "GameObject" , Description = "Finds a gemobject with tag and saves it to the AI Memory")]
    public class FindWithTag : Task
    {
        public MemoryItem Tag = new MemoryItem(ItemType.STRING);

        public string Savekey;

        private MoonMemory Mem;

        public override void OnEnter(MoonAI ai)
        {
            Mem = ai.GetComponent<MoonMemory>(); 
        }

        public override TaskResult OnExecute(MoonAI ai)
        {
            string _tag = Tag.GetValue<string>(Mem);

            if (!string.IsNullOrEmpty(_tag) && !string.IsNullOrEmpty(Savekey))
            {
                GameObject obj = GameObject.FindGameObjectWithTag(_tag);
                Mem.SetValue(Savekey, obj);
                return TaskResult.Success;
            }

            return TaskResult.Failure;
        }
    }
}