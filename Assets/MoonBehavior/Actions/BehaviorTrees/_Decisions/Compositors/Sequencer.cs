using UnityEngine;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [HelpURL("https://pedro15.github.io/MoonBehavior/node-types/#sequencer")]
    [Info(Category = "Compositors" , 
        Description = "Sequencer like his name says executes sequencially every task by order from minor to mayor.")]
    public class Sequencer : Decision
    {
        private int index;

        public override void OnEnter(MoonAI ai)
        {
            index = 0;
        }

        public override TaskResult OnExecute(MoonAI ai)
        {
            while (index < Childs.Length)
            {
                TaskResult tres = Childs[index].Execute(ai);

                if (tres == TaskResult.Failure || tres == TaskResult.Running)
                    return tres;

                index++;
            }

            return TaskResult.Success;
        }
    }
}