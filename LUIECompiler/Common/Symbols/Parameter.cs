
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
        public Parameter(string identifier, ErrorContext errorContext) : base(identifier, errorContext) { }

        /// <summary>
        /// Mapps the parameter to the register in the given context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        public virtual Register ToRegister(CodeGenerationContext context)
        {
            return GetSymbol(context) as Register ?? throw new CodeGenerationException()
            {
                Error = new TypeError(ErrorContext, Identifier, typeof(Register), GetSymbol(context).GetType()),
            };
        }

        /// <summary>
        /// Gets the symbol the parameter is mapped to in the given context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        public Symbol GetSymbol(CodeGenerationContext context)
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
    }
}