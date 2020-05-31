using MoonBehavior.BehaviorTrees;
using UnityEngine;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [HelpURL("https://pedro15.github.io/MoonBehavior/node-types/#parallel")]
    [Info(Category = "Compositors", Description = "Runs all childs every step" )]
    public class Parallel : Decision
    {
        public override TaskResult OnExecute(MoonAI ai)
        {
            for (int i = 0; i < Childs.Length; i++)
            {
                Childs[i].Execute(ai);
            }
            return TaskResult.Running;
        }
    }
}