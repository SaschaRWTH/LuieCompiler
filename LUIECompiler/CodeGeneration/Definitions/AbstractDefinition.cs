using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Code;

namespace LUIECompiler.CodeGeneration.Definitions
{

    public abstract class AbstractDefinition : ITranslateable
    {
        public required string Identifier { get; init; }

        public abstract QASMCode ToQASM();
    }

}