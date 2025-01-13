
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
    /// <summary>
    /// The argument is a symbol used in the definition of a composite gate. It represents an argument of the composite gate.
    /// </summary>
    public class GateArgument : Symbol
    {
        /// <summary>
        /// Creates a new argument.
        /// </summary>
        /// <param name="identifier">Identifier of the argument.</param>
        /// <param name="errorContext">Context of the argument definition.</param>
        public GateArgument(string identifier, ErrorContext errorContext) : base(identifier, errorContext) { }

        /// <summary>
        /// Maps the argument to the register in the given context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        public virtual Register ToRegister(CodeGenerationContext context)
        {
            Register? register = GetSymbol(context) as Register;
            if(register is null)
            {
                Compiler.LogError($"Could not convert the argument '{Identifier}' to a register.");
                throw new CodeGenerationException()
                {
                    Error = new TypeError(ErrorContext, Identifier, typeof(Register), GetSymbol(context).GetType()),
                };
            }
            return register;
        }

        /// <summary>
        /// Gets the symbol the argument is mapped to in the given context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        public virtual Symbol GetSymbol(CodeGenerationContext context)
        {
            if (!context.ArgumentMap.TryGetValue(this, out Symbol? symbol))
            {
                throw new CodeGenerationException()
                {
                    Error = new UndefinedError(this.ErrorContext, this.Identifier),
                };
            }

            if (symbol is GateArgument arg)
            {
                return arg.GetSymbol(context);
            }

            return symbol;
        }

        public override string ToString()
        {
            return $"Argument = {{ id={Identifier}, Hash={GetHashCode()} }}";
        }
    }
}