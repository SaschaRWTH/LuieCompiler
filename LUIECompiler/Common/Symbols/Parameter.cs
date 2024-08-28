
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
    /// <summary>
    /// The parameter is a symbol used in the definition of a composite gate. It represents a parameter of the composite gate.
    /// </summary>
    public class Parameter : Symbol
    {
        /// <summary>
        /// Creates a new parameter.
        /// </summary>
        /// <param name="identifier">Identifier of the parameter.</param>
        /// <param name="errorContext">Context of the parameter definition.</param>
        public Parameter(string identifier, ErrorContext errorContext) : base(identifier, errorContext) { }

        /// <summary>
        /// Maps the parameter to the register in the given context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        public virtual Register ToRegister(CodeGenerationContext context)
        {
            Register? register = GetSymbol(context) as Register;
            if(register is null)
            {
                Compiler.PrintLog($"Could convert the parameter '{Identifier}' to a register.");
                throw new CodeGenerationException()
                {
                    Error = new TypeError(ErrorContext, Identifier, typeof(Register), GetSymbol(context).GetType()),
                };
            }
            return register;
        }

        /// <summary>
        /// Gets the symbol the parameter is mapped to in the given context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        public virtual Symbol GetSymbol(CodeGenerationContext context)
        {
            if (!context.ParameterMap.TryGetValue(this, out Symbol? symbol))
            {
                throw new CodeGenerationException()
                {
                    Error = new UndefinedError(this.ErrorContext, this.Identifier),
                };
            }

            // TODO: Is a cycle possible? E.g. a -> b, b -> a
            if (symbol is Parameter parameter1)
            {
                return parameter1.GetSymbol(context);
            }

            return symbol;
        }

        public override string ToString()
        {
            return $"Parameter = {{ id={Identifier}, Hash={GetHashCode()} }}";
        }
    }
}