using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
    /// <summary>
    /// Represents an access to a argument in the code generation.
    /// </summary>
    public class GateArgumentAccess : GateArgument
    {
        /// <summary>
        /// Expression that evaluates to the index of the qubit in the <see cref="Register"/>.
        /// </summary>
        public Expression<int> IndexExpression { get; }

        /// <summary>
        /// The argument being accessed.
        /// </summary>
        public GateArgument Argument { get; }

        /// <summary>
        /// Creates a new argument access.
        /// </summary>
        /// <param name="argument">argument being accessed.</param>
        /// <param name="indexExpression">Expression that evaluates to the index of the qubit in the <see cref="Register"/>.</param>
        /// <param name="errorContext">Context of the argument access.</param>
        public GateArgumentAccess(GateArgument argument, Expression<int> indexExpression, ErrorContext errorContext) : base(argument.Identifier, errorContext) 
        { 
            Argument = argument;
            IndexExpression = indexExpression;
        }

        public override RegisterAccess ToRegister(CodeGenerationContext context)
        {
            Symbol symbol = Argument.GetSymbol(context);

            if (symbol is not Register register)
            {
                Compiler.LogError($"Could not convert the argument '{Identifier}' to a register. The symbol is not a register.");
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
                Compiler.LogError($"Could not get the symbol of the argument '{Identifier}'");
                throw new CodeGenerationException()
                {
                    Error = new TypeError(ErrorContext, Identifier, typeof(Register), Argument.GetSymbol(context).GetType()),
                };
            }
            return register.ToRegisterAccess(IndexExpression, ErrorContext);
             
        }

        public override string ToString()
        {
            return $"Argument = {{ id={Identifier}, index={IndexExpression}, Hash={GetHashCode()}, argument={Argument} }}";
        }
    }
}