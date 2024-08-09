
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Gates
{
    /// <summary>
    /// Represents an H gate.
    /// </summary>
    public class HGate : GateCode
    {
        public override string ToCode()
        {
            return "h";
        }

        public override bool SemanticallyEqual(Code code)
        {
            return code is HGate;
        }
    }
}