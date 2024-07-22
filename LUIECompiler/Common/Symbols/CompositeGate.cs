using LUIECompiler.CodeGeneration;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
    public class CompositeGate : Symbol
    {
        public CodeBlock Body { get; init; }

        public List<Parameter> Parameters { get; init; }

        public CompositeGate(string identifier, CodeBlock body, List<Parameter> parameters, ErrorContext errorContext) : base(identifier, errorContext)
        {
            Body = body;
            Parameters = parameters;
        }

    }
}