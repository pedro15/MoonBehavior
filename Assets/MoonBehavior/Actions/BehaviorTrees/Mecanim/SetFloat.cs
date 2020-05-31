using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "Mecanim" , Description = "Sets a float parameter to the Animator component attached to the AI agent." )]
    public class SetFloat : Task
    {
        public MemoryItem Parameter = new MemoryItem(ItemType.STRING);

        public MemoryItem ParamValue = new MemoryItem(ItemType.FLOAT);
        
        public override TaskResult OnExecute(MoonAI ai)
        {
            Animator anim = ai.GetComponent<Animator>();

            string _param = Parameter.GetValue<string>(ai.Memory);

            float val = ParamValue.GetValue<float>(ai.Memory);

            if (anim != null && !string.IsNullOrEmpty(_param))
            {
                anim.SetFloat(_param, val);
                return TaskResult.Success;
            }

            return TaskResult.Failure;
        }
    }
}