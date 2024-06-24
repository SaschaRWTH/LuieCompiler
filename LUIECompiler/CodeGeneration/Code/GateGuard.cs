using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Codes
{
    public class GateGuard
    {
        /// <summary>
        /// Identifier of the guard.
        /// </summary>
        public required string Identifier { get; init; }

        /// <summary>
        /// Indicates whether a guard is negated.
        /// </summary>
        public required bool Negated { get; init; }

        public override string ToString()
        {
            return Identifier;
        }
    }
}