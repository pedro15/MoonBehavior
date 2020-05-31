using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "General")]
    public class SubTree : Task
    {
        public MemoryItem BehaviorTree = new MemoryItem(ItemType.OBJECT);

        [System.NonSerialized]
        private MoonBT Bt = null;

        public override void OnEnter(MoonAI ai)
        {
            MoonBT loadedbt = BehaviorTree.GetValue<MoonBT>(ai.Memory);
            if (loadedbt != null && (loadedbt != ai.BehaviorTree) && loadedbt.Root != null)
            {
                Bt = MoonBT.CopyBT(loadedbt);           
            }
        }

        public override TaskResult OnExecute(MoonAI ai)
        {
            if (Bt != null)
            {
                return Bt.Root.Execute(ai);
            }
            return TaskResult.Failure;
        }
    }

}