using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
    /// <summary>
    /// The parameter is a symbol used in the definition of a composite gate. It represents a parameter of the composite gate.
    /// </summary>
    public class Parameter : Symbol
    {
        public Parameter(string identifier, ErrorContext errorContext) : base(identifier, errorContext) { }
    }
}