using MoonBehavior.Memory;
using UnityEngine;

namespace MoonBehavior.Conditions.Operations
{
    /// <summary>
    /// Object Conition
    /// </summary>
    [System.Serializable]
    public class ObjectOperation : IOperation
    {
        /// <summary>
        /// Operation mode
        /// </summary>
        // in object we only check for equals and not equals
        public enum OperationMode : int 
        {
            EQUALS = 0, // == 
            NOT_EQUALS = 1 // != 
        }

        /// <summary>
        /// Current Operation Mode
        /// </summary>
        [SerializeField]
        private OperationMode Mode;

        public bool Evaluate(string AKey, string BKey, bool BConstant, MoonMemory Memory)
        {

            Object a = Memory.GetValue<Object>(AKey);

            Object b = Memory.GetValue<Object>(BKey);
            
            switch(Mode)
            {
                case OperationMode.EQUALS:
                    return a == b; 
                case OperationMode.NOT_EQUALS:
                    return a != b; 
            }

            return false; 
        }
    }
}
