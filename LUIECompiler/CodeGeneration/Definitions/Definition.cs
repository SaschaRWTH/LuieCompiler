using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Definitions
{

    public abstract class Definition : ITranslateable
    {
        /// <summary>
        /// Identifier to be defined.
        /// </summary>
        public abstract Symbol Register { get; init; }

        public abstract QASMProgram ToQASM(CodeGenerationContext context);

        public override string ToString()
        {
            return $"Definition = {{Register = {Register}}}";
        }
    }

}