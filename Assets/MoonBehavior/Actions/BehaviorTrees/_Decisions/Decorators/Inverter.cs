using UnityEngine;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [HelpURL("https://pedro15.github.io/MoonBehavior/node-types/#inverter")]
    [SingleChild]
    [Info(Category = "Decorators", Description = "Like the ! Operator, returns the inverter result of it's child nodes")]
    public class Inverter : Decision
    {
        public override TaskResult OnExecute(MoonAI ai)
        {
            while (Childs.Length > 0)
            {
                TaskResult tres = Childs[0].Execute(ai);
                if (tres == TaskResult.Failure)
                    return TaskResult.Success;
                if (tres == TaskResult.Success)
                    return TaskResult.Failure;
                else return TaskResult.Running;
            }
            return TaskResult.Failure;
        }
    }
}