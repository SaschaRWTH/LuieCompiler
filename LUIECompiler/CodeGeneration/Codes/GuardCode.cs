using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Codes
{
    public class GuardCode : Code
    {
        /// <summary>
        /// Identifier of the guard.
        /// </summary>
        public required QubitCode Qubit { get; init; }

        /// <summary>
        /// Indicates whether a guard is negated.
        /// </summary>
        public required bool Negated { get; init; }

        public override string ToCode()
        {
            return Qubit.ToCode();;
        }

        public override bool SemanticallyEqual(Code code)
        {
            if (code is not GuardCode guardCode)
            {
                return false;
            }

            return Qubit.SemanticallyEqual(guardCode.Qubit) && Negated == guardCode.Negated;
        }
    }
}