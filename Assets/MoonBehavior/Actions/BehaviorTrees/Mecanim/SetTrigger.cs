using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "Mecanim" , Description = "Sets a Trigger parameter to the Animator component attached to the AI agent.")]
    public class SetTrigger : Task
    {
        public MemoryItem Parameter = new MemoryItem(ItemType.STRING);
        
        public override TaskResult OnExecute(MoonAI ai)
        {
            Animator anim = ai.GetComponent<Animator>();

            string _param = Parameter.GetValue<string>(ai.Memory);

            if (anim != null && !string.IsNullOrEmpty(_param))
            {
                anim.SetTrigger(_param);
                return TaskResult.Success;
            }

            return TaskResult.Failure;
        }
    }
}