using System.Numerics;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Extensions;

namespace LUIECompiler.CodeGeneration.Expressions
{

    public class PowerFunctionExpression<T> : FunctionExpression<T> where T : INumber<T>
    {
        public List<Expression<T>> Parameters { get; init; }

        public PowerFunctionExpression(List<Expression<T>> parameters)
        {
            Parameters = parameters;
        }

        public PowerFunctionExpression(LuieParser.FunctionParameterContext context)
        {
            // TODO: Add error handling for wrong parameters
            Parameters = context.expression()?.Select(expression => expression.GetExpression<T>()).ToList() ?? throw new NotImplementedException();
        }

        public override T Evaluate(CodeGenerationContext context)
        {
            if (Parameters.Count != 2)
            {
                throw new CodeGenerationException()
                {
                    Error = new InvalidFunctionArguments(new ErrorContext(), "Power", 2, Parameters.Count),
                };
            }

            Expression<double> @base = Parameters[0] as Expression<double> ?? throw new InternalException()
            {
                Reason = "The base parameter is not a double expression.",
            };
            Expression<double> exponent = Parameters[0] as Expression<double> ?? throw new InternalException()
            {
                Reason = "The exponent parameter is not a double expression.",
            };

            return T.CreateChecked(Math.Pow(@base.Evaluate(context), exponent.Evaluate(context)));
        }

        public override List<string> UndefinedIdentifiers(SymbolTable table)
        {
            throw new NotImplementedException();
        }
    }
}