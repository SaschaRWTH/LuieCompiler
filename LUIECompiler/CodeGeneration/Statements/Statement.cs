
using System.Diagnostics.CodeAnalysis;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Errors;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Symbols;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Statements
{
    public abstract class Statement : ITranslateable
    {
        
        /// <summary>
        /// Maps any <see cref="Qubit"/> to the corresponding <see cref="Definition"/>.
        /// </summary>
        public required Dictionary<Register, Definition> DefinitionDictionary { get; init; }

        public required SymbolTable SymbolTable { get; init; }

        /// <summary>
        /// Source code line of the statement.
        /// </summary>
        public required ErrorContext ErrorContext { get; init; }
        public abstract QASMProgram ToQASM(List<Constant<int>> constants);

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
                    Error = new UndefinedError(ErrorContext, register.Identifier),
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
                    Error = new UndefinedError(ErrorContext, register.Identifier),
                };
            }
            return definition;
        }

        /// <summary>
        /// Translates a <paramref name="qubit"/> symbol to QASM qubit code.
        /// </summary>
        /// <param name="qubit"></param>
        /// <param name="constants"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        protected QubitCode TranslateQubit([NotNull] Qubit qubit, List<Constant<int>> constants)
        {
            RegisterDefinition definition = GetDefinition(qubit) as RegisterDefinition ??
                throw new InternalException()
                {
                    Reason = "Guard is not a register definition. This should have been caught by the semantic analysis and type checking while generating."
                };

            return qubit.ToQASMCode(definition, constants, ErrorContext);
        } 
    }

}