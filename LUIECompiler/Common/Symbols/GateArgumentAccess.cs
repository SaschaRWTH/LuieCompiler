using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
    /// <summary>
    /// Represents an access to a parameter in the code generation.
    /// </summary>
    public class GateArgumentAccess : GateArgument
    {
        /// <summary>
        /// Expression that evaluates to the index of the qubit in the <see cref="Register"/>.
        /// </summary>
        public Expression<int> IndexExpression { get; }

        /// <summary>
        /// The parameter being accessed.
        /// </summary>
        public GateArgument Argument { get; }

        /// <summary>
        /// Creates a new parameter access.
        /// </summary>
        /// <param name="parameter">Parameter being accessed.</param>
        /// <param name="indexExpression">Expression that evaluates to the index of the qubit in the <see cref="Register"/>.</param>
        /// <param name="errorContext">Context of the parameter access.</param>
        public GateArgumentAccess(GateArgument parameter, Expression<int> indexExpression, ErrorContext errorContext) : base(parameter.Identifier, errorContext) 
        { 
            Argument = parameter;
            IndexExpression = indexExpression;
        }

        public override RegisterAccess ToRegister(CodeGenerationContext context)
        {
            Symbol symbol = Argument.GetSymbol(context);

            if (symbol is not Register register)
            {
                Compiler.LogError($"Could not convert the parameter '{Identifier}' to a register. The symbol is not a register.");
                throw new CodeGenerationException()
                {
                    Error = new TypeError(ErrorContext, Identifier, typeof(Register), symbol.GetType()),
                };
            }

            return new RegisterAccess(register, IndexExpression, ErrorContext);
        }

        public override Symbol GetSymbol(CodeGenerationContext context)
        {
            Register? register = Argument.GetSymbol(context) as Register;
            if (register is null)
            {
                Compiler.LogError($"Could not get the symbol of the parameter '{Identifier}'");
                throw new CodeGenerationException()
                {
                    Error = new TypeError(ErrorContext, Identifier, typeof(Register), Argument.GetSymbol(context).GetType()),
                };
            }
            return register.ToRegisterAccess(IndexExpression, ErrorContext);
             
        }

        public override string ToString()
        {
            return $"Argument = {{ id={Identifier}, index={IndexExpression}, Hash={GetHashCode()}, Parameter={Argument} }}";
        }
    }
}