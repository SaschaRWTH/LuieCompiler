
using Antlr4.Runtime;

namespace LUIECompiler.Common.Errors
{
    public class UndefinedError : CompilationError
    {
        public string Identifier { get; init; }

        public UndefinedError(int line, string identifier)
        {
            Type = ErrorType.Critical;
            Line = line;
            Identifier = identifier;
            Description = $"The identifier {identifier} does not exist in the context.";
        }
    }

}
