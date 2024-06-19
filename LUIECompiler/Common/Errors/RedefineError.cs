
using Antlr4.Runtime;

namespace LUIECompiler.Common.Errors
{
    public class RedefineError : CompilationError
    {
        public string Identifier { get; init; }

        public RedefineError(int line, string identifier)
        {
            Type = ErrorType.Critical;
            Line = line;
            Identifier = identifier;
            Description = $"The register {identifier} is already defined in the context.";
        }
    }

}
