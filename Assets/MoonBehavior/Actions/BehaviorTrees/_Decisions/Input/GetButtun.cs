using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [SingleChild]
    [Info(Category = "Input", Name = "Get Buttun" , Description = "Executes it's child node if the buttun assigned are pressed")]
    public class GetButtun : Decision
    {
        public MemoryItem ButtunName = new MemoryItem(ItemType.STRING);

        public enum ButtunMode
        {
            Hold, Down, Up
        }

        public ButtunMode Mode;
        
        public override TaskResult OnExecute(MoonAI ai)
        {
            string btnName = ButtunName.GetValue<string>(ai.Memory);
            if (!string.IsNullOrEmpty(btnName) && ((Mode == ButtunMode.Hold && Input.GetButton(btnName)) || (Mode == ButtunMode.Down &&
                Input.GetButtonDown(btnName)) || (Mode == ButtunMode.Up && Input.GetButtonUp(btnName))))
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