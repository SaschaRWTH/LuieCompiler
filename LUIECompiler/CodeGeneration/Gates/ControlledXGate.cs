
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Gates
{
    /// <summary>
    /// Represents a controlled X gate.
    /// </summary>
    public class ControlledXGate : GateCode
    {
        public override bool SemanticallyEqual(Code code)
        {
            return code is ControlledXGate;
        }
        public override string ToCode()
        {
            return "cx";
        }
    }
}