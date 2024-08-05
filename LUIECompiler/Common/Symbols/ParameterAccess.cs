using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
    /// <summary>
    /// Represents an access to a parameter in the code generation.
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

        /// <summary>
        /// Creates a new parameter access.
        /// </summary>
        /// <param name="parameter">Parameter being accessed.</param>
        /// <param name="indexExpression">Expression that evaluates to the index of the qubit in the <see cref="Register"/>.</param>
        /// <param name="errorContext">Context of the parameter access.</param>
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

        public override Symbol GetSymbol(CodeGenerationContext context)
        {
            Register register = Parameter.GetSymbol(context) as Register ?? throw new CodeGenerationException()
            {
                Error = new TypeError(ErrorContext, Identifier, typeof(Register), Parameter.GetSymbol(context).GetType()),
            };

            return register.ToRegisterAccess(IndexExpression, ErrorContext);
             
        }

        public override string ToString()
        {
            return $"Parameter = {{ id={Identifier}, index={IndexExpression}, Hash={GetHashCode()}, Parameter={Parameter} }}";
        }
    }
}