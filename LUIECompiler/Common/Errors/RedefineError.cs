
using Antlr4.Runtime;

namespace LUIECompiler.Common.Errors
{
    public class RedefineError : CompilationError
    {
        /// <summary>
        /// Identifier that is already defined.
        /// </summary>
        public string Identifier { get; init; }

        public RedefineError(ErrorContext context, string identifier)
        {
            Type = ErrorType.Critical;
            ErrorContext = context;
            Identifier = identifier;
            Description = $"The register {identifier} is already defined in the context.";
        }
    }

}
