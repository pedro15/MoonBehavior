using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "Memory", Description = "Saves a value to the MoonMemory component")]
    public class SetValue : Task
    {
        public MemoryItem Key = new MemoryItem(ItemType.STRING);

        public MemoryItem Value = new MemoryItem(true);

        public override TaskResult OnExecute(MoonAI ai)
        {
            string _key = Key.GetValue<string>(ai.Memory);

            if (!string.IsNullOrEmpty(_key))
            {
                object val = Value.GetValue<object>(null);
                ai.Memory.SetValue(_key, val);
                return TaskResult.Success;
            }
            return TaskResult.Failure;
        }
    }
}