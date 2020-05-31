using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "Mecanim" , Description = "Sets new animator speed" )]
    public class SetAnimatorSpeed : Task
    {
        public MemoryItem Speed = new MemoryItem(ItemType.FLOAT);

        private Animator anim;

        public override void OnEnter(MoonAI ai)
        {
            anim = ai.GetComponent<Animator>();
        }

        public override TaskResult OnExecute(MoonAI ai)
        {
            
            if (anim != null)
            {
                anim.speed = Speed.GetValue<float>(ai.Memory);
                return TaskResult.Success;
            }

            return TaskResult.Failure;
        }
    }
}