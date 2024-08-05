
namespace LUIECompiler.CodeGeneration.Gates
{
    /// <summary>
    /// Represents a Y gate.
    /// </summary>
    public class YGate : GateCode
    {
        public override string ToCode()
        {
            return "y";
        }
    }
}