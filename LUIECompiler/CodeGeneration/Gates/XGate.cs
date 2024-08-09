
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Gates
{
    /// <summary>
    /// Represents an X gate.
    /// </summary>
    public class XGate : GateCode
    {
        public override bool SemanticallyEqual(Code code)
        {
            return code is XGate;
        }

        public override string ToCode()
        {
            return "x";
        }
    }
}