using LUIECompiler.CodeGeneration;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
    /// <summary>
    /// Represents a composite gate.
    /// </summary>
    public class CompositeGate : Symbol, IGate
    {
        /// <summary>
        /// The body of the composite gate.
        /// </summary>
        public CodeBlock Body { get; init; }

        /// <summary>
        /// List of arguments of the composite gate.
        /// </summary>
        public List<GateArgument> Arguments { get; init; }
        
        /// <summary>
        /// Number of arguments the gate takes.
        /// </summary>
        public int NumberOfArguments { get => Arguments.Count; }

        /// <summary>
        /// Type of the gate.
        /// </summary>
        public GateType Type { get => GateType.Composite; }

        /// <summary>
        /// Creates a new composite gate.
        /// </summary>
        /// <param name="identifier">Identifier of the gate.</param>
        /// <param name="body">Body of the gate.</param>
        /// <param name="arguments">List of arguments of the gate.</param>
        /// <param name="errorContext">Context of the gate definition.</param>
        public CompositeGate(string identifier, CodeBlock body, List<GateArgument> arguments, ErrorContext errorContext) : base(identifier, errorContext)
        {
            Body = body;
            Arguments = arguments;
        }

        public override string ToString()
        {
            return $"CompositeGate: {{ Identifier = {Identifier}, Args = {Arguments} }}";
        }
    }
}