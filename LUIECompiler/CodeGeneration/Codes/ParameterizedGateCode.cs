using System.Globalization;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Codes
{
    public class ParameterizedGateCode : GateCode
    {
        public double Parameter { get; }

        public ParameterizedGateCode(GateType type, double parameter) : base(type)
        {
            Parameter = parameter;
        }

        public override string ToCode()
        {
            return $"{GateType.ToCode()}(pi * {Parameter.ToString(CultureInfo.InvariantCulture)})";
        }

        public override bool SemanticallyEqual(Code code)
        {
            return base.SemanticallyEqual(code) 
                && code is ParameterizedGateCode parameterizedGateCode 
                && Parameter == parameterizedGateCode.Parameter;
        }
    }
}