using MoonBehavior.Memory;
using UnityEngine;

namespace MoonBehavior.Conditions.Operations
{
    /// <summary>
    /// String operation mode
    /// </summary>
    [System.Serializable]
    public class StringOperation : IOperation
    {
        /// <summary>
        /// Stored string constant
        /// </summary>
        [SerializeField]
        private string BValue;

        /// <summary>
        /// Current Operation Mode
        /// </summary>
        [SerializeField]
        private ObjectOperation.OperationMode Mode; 

        public bool Evaluate(string AKey, string BKey, bool BConstant, MoonMemory Memory)
        {
            string a = Memory.GetValue<string>(AKey);
            string b = BConstant ? BValue : Memory.GetValue<string>(BKey);

            switch(Mode)
            {
                case ObjectOperation.OperationMode.EQUALS:
                    return string.Equals(a, b);
                case ObjectOperation.OperationMode.NOT_EQUALS:
                    return !string.Equals(a, b);
            }

            return false; 
        }
    }
}
