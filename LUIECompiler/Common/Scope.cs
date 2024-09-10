using LUIECompiler.CodeGeneration;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common
{
    public class Scope
    {
        /// <summary>
        /// Maps an identifier to the corresponding <see cref="Symbol"/>.
        /// </summary>
        public Dictionary<string, Symbol> IdentifierMap { get; } = [];

        /// <summary>
        /// The code block corresponding to the scope.
        /// </summary>
        public required CodeBlock CodeBlock { get; init; }

        /// <summary>
        /// The symbol that guards the execution of the scope.
        /// </summary>
        public Symbol? Guard { get; init; }

        /// <summary>
        /// Gets all parameters from the scope.
        /// </summary>
        /// <returns></returns>
        public List<Parameter> GetParameters()
        {
            List<Parameter> parameters = [];
            foreach (var symbol in IdentifierMap.Values)
            {
                if (symbol is Parameter parameter)
                {
                    parameters.Add(parameter);
                }
            }
            return parameters;
        }
    }
}