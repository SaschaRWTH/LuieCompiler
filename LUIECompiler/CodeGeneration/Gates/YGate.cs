
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Gates
{
    /// <summary>
    /// Represents a Y gate.
    /// </summary>
    public class YGate : GateCode
    {
        public override bool SemanticallyEqual(Code code)
        {
            return code is YGate;
        }

        public override string ToCode()
        {
            return "y";
        }
    }
}