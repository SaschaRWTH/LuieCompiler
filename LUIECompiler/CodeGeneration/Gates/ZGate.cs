
namespace LUIECompiler.CodeGeneration.Gates
{
    /// <summary>
    /// Represents a Z gate.
    /// </summary>
    public class ZGate : GateCode
    {
        public override string ToCode()
        {
            return "z";
        }
    }
}