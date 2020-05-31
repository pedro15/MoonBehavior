using UnityEngine;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [HelpURL("https://pedro15.github.io/MoonBehavior/node-types/#random-selector")]
    [Info(Category = "Compositors" , Description = "Select a child node based on higher to lower priority and return it's result")]
    public class RandomSelector : Decision
    {
        Task ChooseOne(MoonAI ai)
        {
            if (Childs.Length <= 0) return null;

            float total = 0;

            foreach (Task t in Childs)
            {
                total += t.GetPriority(ai);
            }

            float randomPoint = Random.value * total;

            for (int i = 0; i < Childs.Length; i++)
            {
                if (randomPoint < (Childs[i].GetPriority(ai)))
                {
                    return Childs[i];
                }
                else
                {
                    randomPoint -= Childs[i].GetPriority(ai);
                }
            }
            return Childs[Childs.Length - 1];
        }

        public override TaskResult OnExecute(MoonAI ai)
        {
            Task choosed = ChooseOne(ai);

            if (choosed != null)
            {
                return choosed.Execute(ai);
            }

            return TaskResult.Failure;
        }
    }
}