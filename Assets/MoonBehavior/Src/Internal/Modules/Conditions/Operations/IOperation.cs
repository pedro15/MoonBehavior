using MoonBehavior.Memory;

namespace MoonBehavior.Conditions.Operations
{
    /// <summary>
    /// Operation interface for Conditions
    /// </summary>
    public interface IOperation
    {
        bool Evaluate(string AKey , string BKey , bool BConstant , MoonMemory Memory);
    }
}
