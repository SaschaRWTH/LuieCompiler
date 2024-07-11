
using System.Diagnostics.CodeAnalysis;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Errors;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Statements
{
    public abstract class Statement : ITranslateable
    {
        public required CodeBlock ParentBlock { get; init; }

        /// <summary>
        /// Source code line of the statement.
        /// </summary>
        public required ErrorContext ErrorContext { get; init; }
        public abstract QASMProgram ToQASM(CodeGenerationContext context);
        
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

            Definition definition = ParentBlock.GetDefinition(register);
            
            return definition;
        }

        /// <summary>
        /// Translates a <paramref name="qubit"/> symbol to QASM qubit code.
        /// </summary>
        /// <param name="qubit"></param>
        /// <param name="constants"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        protected QubitCode TranslateQubit([NotNull] Qubit qubit, CodeGenerationContext context)
        {
            RegisterDefinition definition = GetDefinition(qubit) as RegisterDefinition ??
                throw new InternalException()
                {
                    Reason = "Guard is not a register definition. This should have been caught by the semantic analysis and type checking while generating."
                };

            return qubit.ToQASMCode(definition, context, ErrorContext);
        } 
    }

}