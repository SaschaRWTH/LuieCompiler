
namespace LUIECompiler.CodeGeneration.Gates
{
    /// <summary>
    /// Represents a controlled X gate.
    /// </summary>
    public class ControlledXGate : GateCode
    {
        public override string ToCode()
        {
            return "cx";
        }
    }
}