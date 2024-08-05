
namespace LUIECompiler.CodeGeneration.Gates
{
    /// <summary>
    /// Represents a controlled controlled X gate.
    /// </summary>
    public class ControlledControlledXGate : GateCode
    {
        public override string ToCode()
        {
            return "ccx";
        }
    }
}