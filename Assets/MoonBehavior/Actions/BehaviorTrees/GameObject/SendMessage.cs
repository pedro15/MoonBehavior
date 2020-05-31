using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "GameObject" , 
        Description = "Calls the specific method in every monobehavior on the target GameObject" , Name = "Send Message")]
    public class SendMessage : Task
    {
        public MemoryItem _MethodName = new MemoryItem(ItemType.STRING);
        
        public MemoryItem TargetGameObjectKey = new MemoryItem(ItemType.STRING);

        public SendMessageOptions Options;

        private string TargetGoKey;
        private string MethodName;

        public override void OnEnter(MoonAI ai)
        {
            MethodName = _MethodName.GetValue<string>(ai.Memory);
            TargetGoKey = TargetGameObjectKey.GetValue<string>(ai.Memory);
        }

        public override TaskResult OnExecute(MoonAI ai)
        {
            if (!string.IsNullOrEmpty(MethodName) & !string.IsNullOrEmpty(TargetGoKey))
            {
                GameObject target = ai.Memory.GetValue<GameObject>(TargetGoKey);
                if (target != null)
                {
                    target.SendMessage(MethodName, Options);
                }
            }
            return TaskResult.Failure;
        }
    }
}