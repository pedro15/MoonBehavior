using System.Collections.Generic;
using UnityEngine;
using MoonBehavior.Conditions;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [SingleChild]
    [Info(Name = "Condition" , Category = "Decorators")]
    public class BTCondition : Decision
    {


        [SerializeField,HideInInspector]
        private List<BasicCondition> conditions = new List<BasicCondition>();

        public override TaskResult OnExecute(MoonAI ai)
        {
            if (Childs.Length > 0)
            {
                if (MoonCondition.Evaluate(ai.Memory , conditions.ToArray()))
                {
                    return Childs[0].Execute(ai);
                }
            }
            return TaskResult.Failure;
        }
    }
}