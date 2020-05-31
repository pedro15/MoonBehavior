using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "Physics" , Description = "Apply force to rigidbody" )]
    public class ApplyForce : Task
    {
        public MemoryItem _rigidbodyKey = new MemoryItem(ItemType.STRING);

        public MemoryItem force = new MemoryItem(ItemType.VECTOR3);

        public MemoryItem useDeltaTime = new MemoryItem(ItemType.BOOLEAN);

        public ForceMode forceMode;

        private Rigidbody rb;
        
        public override void OnEnter(MoonAI ai)
        {
            string rbkey = _rigidbodyKey.GetValue<string>(ai.Memory);
            if (!string.IsNullOrEmpty(rbkey))
                rb = ai.Memory.GetValue<Rigidbody>(rbkey);   
        }
        
        public override TaskResult OnExecute(MoonAI ai)
        {
            if (rb != null)
            {
                Vector3 f = force.GetValue<Vector3>(ai.Memory);
                float delta = (useDeltaTime.GetValue<bool>(ai.Memory)) ? Time.deltaTime : 1;
                rb.AddForce(f * delta , forceMode);
            }
            return TaskResult.Failure;
        }
    }
}