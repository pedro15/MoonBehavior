using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "Vector" , Description = "Math operations for vectors and saves the result in the Memory component" , 
        Name = "Vector Math")]
    public class VectorMath : Task
    {
        public MemoryItem A = new MemoryItem(ItemType.VECTOR3);

        public MemoryItem B = new MemoryItem(ItemType.VECTOR3);

        public MemoryItem SaveKey = new MemoryItem(ItemType.STRING);
        
        public enum VectorMathMode
        {
            Add,Substract,Multiply,Cross
        }

        public VectorMathMode Mode;

        public override TaskResult OnExecute(MoonAI ai)
        {

            string savek = SaveKey.GetValue<string>(ai.Memory);

            if (!string.IsNullOrEmpty(savek))
            {
                Vector3 a = A.GetValue<Vector3>(ai.Memory);

                Vector3 b = B.GetValue<Vector3>(ai.Memory);

                Vector3 result = Vector3.zero;

                switch (Mode)
                {
                    case VectorMathMode.Add:

                        result = a + b;

                        break;
                    case VectorMathMode.Substract:

                        result = a - b;

                        break;
                    case VectorMathMode.Multiply:

                        result = Vector3.Scale(a, b);

                        break;

                    case VectorMathMode.Cross:

                        result = Vector3.Cross(a, b);

                        break;
                }

                ai.Memory.SetValue(savek, result);

                return TaskResult.Success;
            }
            
            return TaskResult.Failure;
        }
    }
}