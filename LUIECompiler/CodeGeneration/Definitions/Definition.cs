using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Definitions
{

    public abstract class Definition : ITranslateable
    {
        public required string Identifier { get; init; }

        public abstract QASMCode ToQASM();
    }

}