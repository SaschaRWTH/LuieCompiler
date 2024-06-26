using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.Common.Errors;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Statements
{
    public class GateApplicationStatement : Statement
    {
        /// <summary>
        /// Gate that is applied in the statement.
        /// </summary>
        public required Gate Gate { get; init; }

        /// <summary>
        /// Register to which the gate is applied to.
        /// </summary>
        public required Register Register { get; init; }

        /// <summary>
        /// Returns the QASM code for the statement.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        public override QASMProgram ToQASM()
        {
            return new(new GateCode()
            {
                Gate = Gate,
                Guards = [],
                Register = GetDefinition(Register) as RegisterDefinition
                        ?? throw new CodeGenerationException()
                        {
                            Error = new TypeError(Line, Register.Identifier),
                        },
            });
        }
    }

}