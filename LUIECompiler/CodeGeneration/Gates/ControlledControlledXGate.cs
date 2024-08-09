
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Gates
{
    /// <summary>
    /// Represents a controlled controlled X gate.
    /// </summary>
    public class ControlledControlledXGate : GateCode
    {
        public override bool SemanticallyEqual(Code code)
        {
            return code is ControlledControlledXGate;
        }

        public override string ToCode()
        {
            return "ccx";
        }
    }
}