using MoonBehavior.Memory;
using UnityEngine;

namespace MoonBehavior.Conditions.Operations
{
    /// <summary>
    /// Boolean condition operation
    /// </summary>
    [System.Serializable]
    public class BooleanOperation : IOperation
    {
        /// <summary>
        /// Stored constant boolean
        /// </summary>
        [SerializeField]
        private bool BValue;

        /// <summary>
        /// operation mode
        /// </summary>
        [SerializeField]
        private ObjectOperation.OperationMode Mode; 
        
        public bool Evaluate(string AKey, string BKey, bool BConstant, MoonMemory Memory)
        {

            bool a = Memory.GetValue<bool>(AKey);

            bool b = BConstant ? BValue : Memory.GetValue<bool>(BKey);

            switch(Mode)
            {
                case ObjectOperation.OperationMode.EQUALS:
                    return a == b;
                case ObjectOperation.OperationMode.NOT_EQUALS:
                    return a != b; 
            }

            return false; 
        }
    }
}
