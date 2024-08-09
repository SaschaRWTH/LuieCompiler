using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Expressions;

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
        public override GateCode ToGateCode(CodeGenerationContext context)
        {
            return new ParameterizedGateCode(Type, Parameter.Evaluate(context));
        }
    }
}