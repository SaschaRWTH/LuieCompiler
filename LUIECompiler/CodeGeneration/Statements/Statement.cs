
using System.Diagnostics.CodeAnalysis;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Errors;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Statements
{
    public abstract class Statement : ITranslateable
    {
        /// <summary>
        /// Maps any <see cref="Qubit"/> to the corresponding <see cref="Definition"/>.
        /// </summary>
        public required Dictionary<Register, Definition> DefinitionDictionary { get; init; }

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
        protected string GetIdentifier([NotNull] Register register)
        {
            if (register is RegisterAccess access)
            {
                register = access.Register;
            }

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
        protected Definition GetDefinition([NotNull] Register register)
        {
            if (register is RegisterAccess access)
            {
                register = access.Register;
            }

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