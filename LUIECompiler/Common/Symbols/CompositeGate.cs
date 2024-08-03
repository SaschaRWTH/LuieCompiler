using LUIECompiler.CodeGeneration;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
    public class CompositeGate : Symbol, IGate
    {
        public CodeBlock Body { get; init; }

        public List<Parameter> Parameters { get; init; }
        
        public int NumberOfArguments { get => Parameters.Count; }
        public GateType Type { get => GateType.Composite; }

        public CompositeGate(string identifier, CodeBlock body, List<Parameter> parameters, ErrorContext errorContext) : base(identifier, errorContext)
        {
            Body = body;
            Parameters = parameters;
        }

    }
}