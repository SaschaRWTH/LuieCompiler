using LUIECompiler.CodeGeneration.Gates;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Codes
{
    /// <summary>
    /// Class representing the QASM code of a gate application.
    /// </summary>
    public class GateApplicationCode : Code
    {
        /// <summary>
        /// List of all registers guarding the gate.
        /// </summary>
        public required List<GuardCode> Guards { get; init; }

        /// <summary>
        /// List of all positive guards, meaning they need to be true for the gate to be executed.
        /// </summary>
        public List<GuardCode> PositiveGuards { get => Guards.Where(g => !g.Negated).ToList(); }

        /// <summary>
        /// List of all negative guards, meaning they need to be false for the gate to be executed.
        /// </summary>
        public List<GuardCode> NegativeGuards { get => Guards.Where(g => g.Negated).ToList(); }

        /// <summary>
        /// Gate to be executed
        /// </summary>
        public required GateCode Gate { get; init; }

        /// <summary>
        /// List of all gate parameters. 
        /// </summary>
        public required List<QubitCode> Parameters { get; init; }

        /// <summary>
        ///  Gets the code string representation of all parameters. 
        /// </summary>
        /// <returns></returns>
        private string GetParameters()
        {
            return string.Join(", ", Parameters.Select(param => param.ToCode()));
        }

        public override string ToCode()
        {
            return Gate.GenerateCode(GetParameters(), NegativeGuards, PositiveGuards);
        }

        public override bool SemanticallyEqual(Code code)
        {
            if (code is not GateApplicationCode gateCode)
            {

                return false;
            }

            foreach(QubitCode param in Parameters)
            {
                // TODO: Check is this check is correct
                if (!gateCode.Parameters.Any(p => p.SemanticallyEqual(param)))
                {
                    return false;
                }
            }

            foreach(GuardCode guard in Guards)
            {
                if (!gateCode.Guards.Any(g => g.SemanticallyEqual(guard)))
                {
                    return false;
                }
            }

            return Gate.SemanticallyEqual(gateCode.Gate);
        }
    }
}