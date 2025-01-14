using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class GateContextExtension
    {
        /// <summary>
        /// Get the gate from the gate context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="symbolTable"></param>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        public static IGate GetGate(this LuieParser.GateContext context, SymbolTable symbolTable)
        {
            if (context.type?.Text is string type)
            {
                return ConstantFromString(type);
            }

            if(context.parameterizedGate?.Text is string parameterizedGate)
            {
                return ParameterizedGateFromString(parameterizedGate, context.param, symbolTable);
            }

            string identifier = context.identifier.Text;
            Symbol? symbol = symbolTable.GetSymbolInfo(identifier);

            if (symbol is not CompositeGate compositeGate)
            {
                throw new CodeGenerationException()
                {
                    Error = new UndefinedError(new ErrorContext(context.Start), identifier),
                };
            }

            return compositeGate;
        }

        /// <summary>
        /// Gets a predefined gate from a string.
        /// </summary>
        /// <param name="gate"></param>
        /// <returns></returns>
        private static Gate ConstantFromString(string gate)
        {
            GateType type = GateTypeExtensions.FromString(gate);

            return new()
            {
                Type = type,
            };
        }

        /// <summary>
        /// Gets a parameterized gate from a string.
        /// </summary>
        /// <param name="gate"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static ParameterizedGate ParameterizedGateFromString(string gate, LuieParser.ExpressionContext expression, SymbolTable symbolTable)
        {
            GateType type = GateTypeExtensions.FromString(gate);
            Expression<double> parameter = expression.GetExpression<double>(symbolTable);
            parameter.PropagateSymbolInformation(symbolTable);

            return new()
            {
                Type = type,
                Parameter = parameter,
            };
        }
    }
}