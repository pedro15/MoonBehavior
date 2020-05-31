using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "Memory" , Description = "Do a math operation and saves it's result to the MoonMemory component")]
    public class Math : Task
    {
        public enum MathMode : int
        {
            Addiction = 0,
            Substraction = 1,
            Division = 2 ,
            Sqrt = 3,
            Sin = 4,
            Cosin = 5,
            Tan = 6,
            Pow = 7,
            Round = 8
        }

        public MathMode Mode;

        [HideInInspector]
        public MemoryItem AValue = new MemoryItem(ItemType.FLOAT);
        [HideInInspector]
        public MemoryItem BValue = new MemoryItem(ItemType.FLOAT);
        [HideInInspector]
        public MemoryItem SaveKey = new MemoryItem(ItemType.STRING);
        
        // Called when the task is executed
        public override TaskResult OnExecute(MoonAI ai)
        {

            float Aval = AValue.GetValue<float>(ai.Memory);

            float BVal = BValue.GetValue<float>(ai.Memory);

            string mSaveKey = SaveKey.GetValue<string>(ai.Memory);

            ai.Memory.SetValue(mSaveKey, Compute(Aval, BVal));
            
            return TaskResult.Success;
        }

        private float Compute(float a , float b)
        {
            switch (Mode)
            {
                case MathMode.Addiction:
                    return (a + b);
                case MathMode.Substraction:
                    return (a - b);
                case MathMode.Division:
                    return (a / (b != 0 ? b : 1));
                case MathMode.Pow:
                    return Mathf.Pow(a, b);
                case MathMode.Sin:
                    return Mathf.Sin(a);
                case MathMode.Cosin:
                    return Mathf.Cos(a);
                case MathMode.Tan:
                    return Mathf.Tan(a);
                case MathMode.Sqrt:
                    return Mathf.Sqrt(a);
                case MathMode.Round:
                    return Mathf.Round(a);
            }
            
            return 0;
        }
    }
}