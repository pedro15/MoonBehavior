using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "Mecanim" , Description = "Sets a int parameter to the Animator component attached to the AI agent.")]
    public class SetInt : Task
    {
        public MemoryItem Parameter = new MemoryItem(ItemType.STRING);

        public MemoryItem ParamValue = new MemoryItem(ItemType.INT);
        

        public override TaskResult OnExecute(MoonAI ai)
        {
            Animator anim = ai.GetComponent<Animator>();

            string _param = Parameter.GetValue<string>(ai.Memory);

            int val = ParamValue.GetValue<int>(ai.Memory);

            if (anim != null && !string.IsNullOrEmpty(_param))
            {
                anim.SetInteger(_param, val);
                return TaskResult.Success;
            }

            return TaskResult.Failure;
        }
    }
}