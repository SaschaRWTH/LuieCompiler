using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Declarations
{

    public abstract class Declaration : ITranslatable
    {
        /// <summary>
        /// Identifier to be defined.
        /// </summary>
        public abstract Symbol Symbol { get; init; }

        public abstract QASMProgram ToQASM(CodeGenerationContext context);

        public override string ToString()
        {
            return $"Definition = {{Register = {Symbol}}}";
        }
    }

}