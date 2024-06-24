
using System.Diagnostics.CodeAnalysis;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.CodeGeneration.Statements
{
    public abstract class Statement : ITranslateable
    {
        /// <summary>
        /// Maps any <see cref="RegisterInfo"/> to the corresponding <see cref="Definition"/>.
        /// </summary>
        public required Dictionary<RegisterInfo, Definition> DefinitionDictionary { get; init; }
        public required int Line { get; init; }
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
                throw new CodeGenerationException()
                {
                    Error = new UndefinedError(Line, register.Identifier),
                };
            }
            return definition.Identifier;
        }

        protected Definition GetDefinition([NotNull] RegisterInfo register)
        {
            if (!DefinitionDictionary.TryGetValue(register, out var definition))
            {
                throw new CodeGenerationException()
                {
                    Error = new UndefinedError(Line, register.Identifier),
                };
            }
            return definition;
        }
    }

}