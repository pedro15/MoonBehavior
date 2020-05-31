
namespace MoonBehavior.Conditions
{
    /// <summary>
    /// Conditions base class
    /// </summary>
    public abstract class MoonCondition
    {
        /// <summary>
        /// Condition operator
        /// </summary>
        public enum ConditionOperator : int
        {
            /// <summary>
            /// && Operator
            /// </summary>
            AND = 0,
            /// <summary>
            /// || Operator
            /// </summary>
            OR = 1
        }

        /// <summary>
        /// Working Operator
        /// </summary>
        public ConditionOperator Operator;

        /// <summary>
        /// Evaluate conditions array
        /// </summary>
        /// <param name="context">Context object</param>
        /// <param name="Conditions">Conditions array</param>
        /// <returns>Condition result</returns>
        public static bool Evaluate(object context , params MoonCondition[] Conditions)
        {
            if (Conditions == null || Conditions.Length == 0) return false; 
            bool status = true;
            for (int i = 0; i < Conditions.Length; i++)
            {
                bool result = Conditions[i].Evaluate(context);
                switch (Conditions[i].Operator)
                {
                    case ConditionOperator.AND:
                        status = (status && result);
                        break;
                    case ConditionOperator.OR:
                        status = (status || result);
                        break;
                       
                }
            }
            return status;
        }

        public abstract bool Evaluate(object context);
    }
}
