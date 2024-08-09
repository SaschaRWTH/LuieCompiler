
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Gates
{
    /// <summary>
    /// Represents a Z gate.
    /// </summary>
    public class ZGate : GateCode
    {
        public override bool SemanticallyEqual(Code code)
        {
            return code is ZGate;
        }

        public override string ToCode()
        {
            return "z";
        }
    }
}