
using System.Diagnostics.CodeAnalysis;
using LUIECompiler.CodeGeneration.Declarations;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Errors;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Statements
{
    /// <summary>
    /// Represents an abstract statement in the code generation.
    /// </summary>
    public abstract class Statement : ITranslateable
    {
        /// <summary>
        /// Source code line of the statement.
        /// </summary>
        public required ErrorContext ErrorContext { get; init; }

        public abstract QASMProgram ToQASM(CodeGenerationContext context);
        
        /// <summary>
        /// Returns the <see cref="Declaration"/> of a given <paramref name="register"/>
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        protected Declaration GetDefinition([NotNull] Register register, CodeGenerationContext context)
        {
            if (register is RegisterAccess access)
            {
                register = access.Register;
            }

            Declaration definition = context.CurrentBlock.GetDefinition(register);
            
            return definition;
        }

        /// <summary>
        /// Translates a <paramref name="qubit"/> symbol to QASM qubit code.
        /// </summary>
        /// <param name="qubit"></param>
        /// <param name="constants"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        protected QubitCode TranslateQubit([NotNull] Symbol symbol, CodeGenerationContext context)
        {
            if(symbol is Parameter parameter)
            {
                symbol = parameter.ToRegister(context);
            }

            Qubit? qubit = symbol as Qubit;
            if (qubit is null)
            {
                Compiler.LogError($"Could not translate the symbol '{symbol.Identifier}'. Symbol is not a qubit.");
                throw new CodeGenerationException()
                {
                    Error = new TypeError(ErrorContext, symbol.Identifier, typeof(Qubit), symbol.GetType())
                };
            }

            RegisterDeclaration definition = GetDefinition(qubit, context) as RegisterDeclaration ??
                throw new InternalException()
                {
                    Reason = "Guard is not a register definition. This should have been caught by the semantic analysis and type checking while generating."
                };

            return qubit.ToQASMCode(definition, context, ErrorContext);
        } 

    }

}