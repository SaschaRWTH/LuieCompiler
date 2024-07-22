
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Gates
{
    public abstract class Gate
    {
        /// <summary>
        /// The number of parameters the gate requires.
        /// </summary>
        public int NumberOfArguments { get; init; }

        public abstract string GenerateCode(string parameters, List<GuardCode> negativeGuards, List<GuardCode> positiveGuards);
    }
}