using LuieCompiler.CodeGeneration.Gates;
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.CodeGeneration.Gates;

namespace LUIECompiler.Common
{
    /// <summary>
    /// Represents a parameterized gate.
    /// </summary>
    public class ParameterizedGate : Gate
    {
        /// <summary>
        /// Parameter of the gate.
        /// </summary>
        public required Expression<double> Parameter { get; init; }

        /// <summary>
        /// Creates a new parameterized gate.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
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