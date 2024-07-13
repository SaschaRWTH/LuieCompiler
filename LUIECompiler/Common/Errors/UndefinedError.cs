
using Antlr4.Runtime;

namespace LUIECompiler.Common.Errors
{
    public class UndefinedError : CompilationError
    {
        /// <summary>
        /// Identifier that is undefined.
        /// </summary>
        public List<string> Identifier { get; init; }

        public UndefinedError(ErrorContext context, string identifier)
        {
            Type = ErrorType.Critical;
            ErrorContext = context;
            Identifier = [identifier];
            Description = $"The identifier {identifier} does not exist in the context.";
        }

        public UndefinedError(ErrorContext context, List<string> identifiers)
        {
            Type = ErrorType.Critical;
            ErrorContext = context;
            Identifier = [.. identifiers];
            Description = $"The identifiers {string.Join(", ", identifiers)} do not exist in the context.";
        }
    }

}
