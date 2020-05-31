using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "General" , Description = "Wait for seconds, returns SUCCESS when the wait is over, otherwise returns RUNNING")]
    public class Wait : Task
    {
        public MemoryItem Seconds = new MemoryItem(ItemType.FLOAT);

        private float Timer { get; set; }

        [HideOnDebug]
        private float WaitTime = 0;

        public override void OnEnter(MoonAI ai)
        {
            Timer = 0;
            WaitTime = Seconds.GetValue<float>(ai.Memory);
            if (WaitTime < 0) WaitTime = 0;
        }

        public override TaskResult OnExecute(MoonAI ai)
        {
            
            if (Timer >= WaitTime)
            {
                return TaskResult.Success;
            }
            else
            {
                Timer += Time.deltaTime;
                return TaskResult.Running;
            }
        }
    }
}
