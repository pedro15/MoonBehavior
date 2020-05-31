using MoonBehavior.Memory;
using UnityEngine;

namespace MoonBehavior.Conditions.Operations
{
    /// <summary>
    /// Numeric codition
    /// </summary>
    [System.Serializable]
    public class NumericOperation : IOperation
    {

        public enum NumericOperationMode : int
        {
            EQUALS = 0, // ==
            NOT_EQUALS = 1, // !=
            MAYOR = 2, // >
            MINOR = 3, // <
            MAYOR_EQUALS = 4, // >=
            MINOR_EQUALS = 5 // <= 
        }

        /// <summary>
        /// Stored constant float
        /// </summary>
        [SerializeField]
        private float BValue;
        
        /// <summary>
        /// Current operation mode
        /// </summary>
        [SerializeField]
        private NumericOperationMode Mode;

        public bool Evaluate(string AKey , string BKey , bool BConstant , MoonMemory Memory)
        {
            float AVal = Memory.GetValue<float>(AKey , (string k) => Memory.GetValue<int>(k));
            float BVal = BConstant ? BValue : Memory.GetValue<float>(BKey , (string k) => Memory.GetValue<int>(k));
            
            switch (Mode)
            {
                case NumericOperationMode.EQUALS:
                    return AVal == BVal;
                case NumericOperationMode.NOT_EQUALS:
                    return AVal != BVal;
                case NumericOperationMode.MAYOR:
                    return AVal > BVal;
                case NumericOperationMode.MINOR:
                    return AVal < BVal;
                case NumericOperationMode.MAYOR_EQUALS:
                    return AVal >= BVal;
                case NumericOperationMode.MINOR_EQUALS:
                    return AVal <= BVal;
            }

            return false;
        }
    }
}
