
using System.Diagnostics.CodeAnalysis;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Statements
{
    public abstract class AbstractStatement : ITranslateable
    {
        /// <summary>
        /// Maps any <see cref="RegisterInfo"/> to the corresponding <see cref="AbstractDefinition"/>.
        /// </summary>
        public required Dictionary<RegisterInfo, AbstractDefinition> DefinitionDictionary;
        public abstract QASMCode ToQASM();

        /// <summary>
        /// Returns the QASM identifier of a given <paramref name="register"/>.
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        protected string GetIdentifier([NotNull] RegisterInfo register)
        {
            if (!DefinitionDictionary.TryGetValue(register, out var definition))
            {
                // TODO: improve error handling
                return "error";
            }
            return definition.Identifier;
        }
    }

}