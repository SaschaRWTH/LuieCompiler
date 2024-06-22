using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Definitions
{

    public abstract class AbstractDefinition : ITranslateable
    {
        public required string Identifier { get; init; }

        public abstract string ToQASM();
    }

}