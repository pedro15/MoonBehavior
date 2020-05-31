using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "GameObject", Description = "Saves GameObject Position to memory component",
           Name = "Save Position")]
    public class SavePosition : Task
    {

        public MemoryItem _gameObjectKey = new MemoryItem(ItemType.STRING);

        public MemoryItem SaveKey = new MemoryItem(ItemType.STRING);

        private GameObject m_gameObject { get; set; }

        public override void OnEnter(MoonAI ai)
        {
            string gokey = _gameObjectKey.GetValue<string>(ai.Memory);
            if (!string.IsNullOrEmpty(gokey))
                m_gameObject = ai.Memory.GetValue<GameObject>(gokey);
        }

        public override TaskResult OnExecute(MoonAI ai)
        {

            string savek = SaveKey.GetValue<string>(ai.Memory);

            if (!string.IsNullOrEmpty(savek) && m_gameObject != null)
            {
                ai.Memory.SetValue(savek, m_gameObject.transform.position);

                return TaskResult.Success;
            }
            
            return TaskResult.Failure;
        }
    }
}