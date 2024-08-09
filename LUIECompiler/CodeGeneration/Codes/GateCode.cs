using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Codes
{
    /// <summary>
    /// Represents the code for an abstract gate.
    /// </summary>
    public class GateCode : Code
    {
        public GateType GateType { get; }

        public GateCode(GateType gateType)
        {
            GateType = gateType;
        } 

        public override string ToCode()
        {
            return GateType.ToCode();
        }

        public override bool SemanticallyEqual(Code code)
        {
            if (code is not GateCode gateCode)
            {
                return false;
            }

            return GateType == gateCode.GateType;
        }
    }
}