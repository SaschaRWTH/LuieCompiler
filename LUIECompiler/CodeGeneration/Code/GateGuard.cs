using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Codes
{
    public class GateGuard
    {
        public required string Identifier { get; init; }
        public required bool Negated { get; init; }

        public override string ToString()
        {
            return Identifier;
        }
    }
}