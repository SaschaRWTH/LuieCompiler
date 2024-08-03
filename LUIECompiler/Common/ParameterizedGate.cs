using LuieCompiler.CodeGeneration.Gates;
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.CodeGeneration.Gates;

namespace LUIECompiler.Common
{
    public class ParameterizedGate : Gate
    {
        public required Expression<double> Parameter { get; init; }

        public override GateCode ToGateCode(CodeGenerationContext context)
        {
            return Type switch
            {
                GateType.P => new PhaseGate()
                {
                    Parameter = Parameter.Evaluate(context),
                },
                _ => throw new NotImplementedException(),
            };
        }
    }
}