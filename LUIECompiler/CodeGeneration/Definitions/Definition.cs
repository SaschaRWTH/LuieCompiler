using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Definitions
{

    public abstract class Definition : ITranslateable
    {
        /// <summary>
        /// Identifier to be defined.
        /// </summary>
        public required string Identifier { get; init; }

        public abstract QASMProgram ToQASM();
    }

}