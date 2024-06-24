
using System.Diagnostics.CodeAnalysis;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Errors;
using LUIECompiler.CodeGeneration.Exceptions;

namespace LUIECompiler.CodeGeneration.Statements
{
    public abstract class Statement : ITranslateable
    {
        /// <summary>
        /// Maps any <see cref="RegisterInfo"/> to the corresponding <see cref="Definition"/>.
        /// </summary>
        public required Dictionary<RegisterInfo, Definition> DefinitionDictionary { get; init; }

        /// <summary>
        /// Source code line of the statement.
        /// </summary>
        public required int Line { get; init; }
        public abstract QASMProgram ToQASM();

        /// <summary>
        /// Returns the QASM identifier of a given <paramref name="register"/>.
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
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

        /// <summary>
        /// Returns the <see cref="Definition"/> of a given <paramref name="register"/>
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
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