using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [HelpURL("https://pedro15.github.io/MoonBehavior/node-types/#selector")]
    [Info(Category = "Compositors", Description = "Selector Executes task sequencially (like sequencer) " +
        "but it will not pass to the next task until the current task fails.")]
    public class Selector : Decision
    {
        [System.NonSerialized]
        private int index = 0;

        [SerializeField]
        private MemoryItem Shuffle = new MemoryItem(ItemType.BOOLEAN);

        [System.NonSerialized]
        private Task[] mChilds;

        public override void OnEnter(MoonAI ai)
        {           
            index = 0;
            mChilds = Childs;
            if (Shuffle.GetValue<bool>(ai.Memory))
            {
                for (int i = 0; i < Childs.Length; i++)
                {
                    Task curr = Childs[i];
                    int randomindex = Random.Range(0, Childs.Length);
                    Task randomT = Childs[randomindex];
                    mChilds[i] = randomT;
                    mChilds[randomindex] = curr;
                }
            }
        }

        public override TaskResult OnExecute(MoonAI ai)
        {

            TaskResult tRes = TaskResult.Failure;

            while (index < mChilds.Length)
            {
                tRes = mChilds[index].Execute(ai);
                if (tRes == TaskResult.Failure)
                {
                    index++;
                }
                else
                {
                    return tRes;
                }
            }

            return tRes;
        }
    }
}