using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
    /// <summary>
    /// Represents a parameter in the code generation.
    /// </summary>
    public class ParameterAccess : Parameter
    {
        /// <summary>
        /// Expression that evaluates to the index of the qubit in the <see cref="Register"/>.
        /// </summary>
        public Expression<int> IndexExpression { get; }

        /// <summary>
        /// The parameter being accessed.
        /// </summary>
        public Parameter Parameter { get; }

        public ParameterAccess(Parameter parameter, Expression<int> indexExpression, ErrorContext errorContext) : base(parameter.Identifier, errorContext) 
        { 
            Parameter = parameter;
            IndexExpression = indexExpression;
        }

        public override RegisterAccess ToRegister(CodeGenerationContext context)
        {
            Symbol symbol = Parameter.GetSymbol(context);

            if (symbol is not Register register)
            {
                throw new CodeGenerationException()
                {
                    Error = new TypeError(ErrorContext, Identifier, typeof(Register), symbol.GetType()),
                };
            }

            return new RegisterAccess(register, IndexExpression, ErrorContext);
        }

        public override string ToString()
        {
            return $"Parameter: {{ id={Identifier}, index={IndexExpression} }}";
        }
    }
}