using MoonBehavior.Memory;
using UnityEngine;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [HelpURL("https://pedro15.github.io/MoonBehavior/node-types/#iterator")]
    [SingleChild]
    [Info(Category = "Decorators" , Description = "Repeats a child task every tick (like Repeater) but with a limited repeat count.")]
    public class Iterator : Decision
    {
        public MemoryItem RepeatCount = new MemoryItem(ItemType.INT);

        private int count;
        private int MaxCount;
        
        public override void OnEnter(MoonAI ai)
        {
            count = 0;
            MaxCount = RepeatCount.GetValue<int>(ai.Memory);
        }

        public override TaskResult OnExecute(MoonAI ai)
        {
            while (count < MaxCount)
            {
                TaskResult tres = Childs[0].Execute(ai);
                if (tres == TaskResult.Failure || tres == TaskResult.Success)
                    count++;

                return TaskResult.Running;
            }
            return TaskResult.Success;
        }
    }
}