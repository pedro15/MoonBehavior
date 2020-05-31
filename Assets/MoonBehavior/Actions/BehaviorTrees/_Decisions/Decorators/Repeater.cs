using UnityEngine;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [HelpURL("https://pedro15.github.io/MoonBehavior/node-types/#repeater")]
    [SingleChild]
    [Info(Category = "Decorators" , Description = "Repeats a child Task every tick.")]
    public class Repeater : Decision
    {
        private enum RepeatMode
        {
            Forever, Success, Failure
        }

        [SerializeField]
        private RepeatMode repeatUntil;
        
        public override TaskResult OnExecute(MoonAI ai)
        {
            while (Application.isPlaying)
            {
                TaskResult tres = Childs[0].Execute(ai);

                bool untilsuccess = (repeatUntil == RepeatMode.Success && tres != TaskResult.Success);

                bool untilfail = (repeatUntil == RepeatMode.Failure && tres != TaskResult.Failure);

                bool canpass = repeatUntil == RepeatMode.Forever | ((untilsuccess & !untilfail) || (!untilsuccess & untilfail));

                if (canpass)
                {
                    return TaskResult.Running;
                }
                else
                {
                    break;
                }
            }
            return TaskResult.Success;

        }
    }
}