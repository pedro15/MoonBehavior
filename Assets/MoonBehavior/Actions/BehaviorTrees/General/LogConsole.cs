using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "General")]
    public class LogConsole : Task
    {
        [SerializeField]
        private MessageType Type;

        private enum MessageType
        {
            Info, Warning, Error
        }

        public MemoryItem Message = new MemoryItem();

        public override TaskResult OnExecute(MoonAI ai)
        {
            object msg = Message.GetValue<object>(ai.Memory);

            if (Type == MessageType.Info)
            {
                Debug.Log(msg);

            }
            else if (Type == MessageType.Warning)
            {
                Debug.LogWarning(msg);
            }
            else if (Type == MessageType.Error)
            {
                Debug.LogError(msg);
            }

            return TaskResult.Success;
        }

        public LogConsole() { }
    }
}
