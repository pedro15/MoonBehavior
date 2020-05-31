using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "Mecanim" , Description = "Sets a boolean parameter to the Animator component attached to the AI agent.")]
    public class SetBool : Task
    {
        public MemoryItem Parameter = new MemoryItem(ItemType.STRING);

        public MemoryItem ParamValue = new MemoryItem(ItemType.BOOLEAN);
        
        public override TaskResult OnExecute(MoonAI ai)
        {
            Animator anim = ai.GetComponent<Animator>();

            string _param = Parameter.GetValue<string>(ai.Memory);

            bool val = ParamValue.GetValue<bool>(ai.Memory);

            if (anim != null && !string.IsNullOrEmpty(_param))
            {
                anim.SetBool(_param, val);
                return TaskResult.Success;
            }

            return TaskResult.Failure;
        }
    }
}