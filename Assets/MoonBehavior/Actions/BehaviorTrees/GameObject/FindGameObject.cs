using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "GameObject" , Description = "Finds a GameObject by name in the scene and saves it to the memory component")]
    public class FindGameObject : Task
    {

        public MemoryItem GameObjectName = new MemoryItem(ItemType.STRING);

        public MemoryItem SaveKey = new MemoryItem(ItemType.STRING);
        
        public override TaskResult OnExecute(MoonAI ai)
        {
            string _name = GameObjectName.GetValue<string>(ai.Memory);
            string _key = SaveKey.GetValue<string>(ai.Memory);

            if (!string.IsNullOrEmpty(_name) & !string.IsNullOrEmpty(_key))
            {
                GameObject found = GameObject.Find(_name);

                ai.Memory.SetValue(_key, found);
                
                return TaskResult.Success;
            }
            return TaskResult.Failure;
        }
    }
}