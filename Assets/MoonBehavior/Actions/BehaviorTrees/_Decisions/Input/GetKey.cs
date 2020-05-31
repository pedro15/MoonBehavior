using UnityEngine;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [SingleChild]
    [Info(Category = "Input", Name = "Get Key" , Description = "Executes it's child node if the key assigned are pressed")]
    public class GetKey : Decision
    {
        public KeyCode key;

        public GetButtun.ButtunMode Mode;

        public override TaskResult OnExecute(MoonAI ai)
        {
            if ((Mode == GetButtun.ButtunMode.Hold && Input.GetKey(key)) ||
                (Mode == GetButtun.ButtunMode.Down && Input.GetKeyDown(key)) || (Mode == GetButtun.ButtunMode.Up && Input.GetKeyUp(key)))
            {
                Task child = Childs[0];
                if (child != null)
                {
                    return child.Execute(ai);
                }
            }

            return TaskResult.Failure;
        }
    }
}