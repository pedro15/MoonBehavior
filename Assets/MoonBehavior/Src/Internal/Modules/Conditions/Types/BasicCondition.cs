using UnityEngine;
using MoonBehavior.Conditions.Operations;
using MoonBehavior.Memory;

namespace MoonBehavior.Conditions
{
    /// <summary>
    /// Basic condition class that hanldes basic operations
    /// </summary>
    [System.Serializable]
    public class BasicCondition : MoonCondition
    {
        /// <summary>
        /// A item Memory Key
        /// </summary>
        [SerializeField]
        private string AKey;

        /// <summary>
        /// B item Memory Key
        /// </summary>
        [SerializeField]
        private string BKey;

        /// <summary>
        /// Is B item a Constant ?
        /// </summary>
        [SerializeField]
        private bool BConstant;
        
        /// <summary>
        /// Condition types to determinate what operation use
        /// </summary>
        public enum ConditionType : int 
        {
            Numeric = 0,
            String = 1,
            Boolean = 2,
            Object = 3
        }

        /// <summary>
        /// Cuurent condition type
        /// </summary>
        [SerializeField]
        private ConditionType Type;
        
        /// <summary>
        /// Stored Numeric operation
        /// </summary>
        [SerializeField]
        private NumericOperation op_numeric;
        /// <summary>
        /// Stored String Operation
        /// </summary>
        [SerializeField]
        private StringOperation op_string;
        /// <summary>
        /// Stored boolean operation
        /// </summary>
        [SerializeField]
        private BooleanOperation op_bool;
        /// <summary>
        /// Stored Object operation
        /// </summary>
        [SerializeField]
        private ObjectOperation op_obj;

        public override bool Evaluate(object context)
        {
            MoonMemory mem = context as MoonMemory;
            
            switch(Type)
            {
                case ConditionType.Boolean:
                    return op_bool.Evaluate(AKey, BKey, BConstant, mem);
                case ConditionType.Numeric:
                    return op_numeric.Evaluate(AKey, BKey, BConstant, mem);
                case ConditionType.Object:
                    return op_obj.Evaluate(AKey, BKey, BConstant, mem);
                case ConditionType.String:
                    return op_string.Evaluate(AKey, BKey, BConstant, mem);
            }

            return false; 
        }
    }
}