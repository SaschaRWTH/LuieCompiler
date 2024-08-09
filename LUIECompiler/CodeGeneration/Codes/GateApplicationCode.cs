using System.Reflection.Metadata;
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

            CheckParameterSemanticEquality(gateCode.Parameters);

            // Guards are independent of order and amounts (i.e. ctrl(2) @ q, q = ctrl(1) @ q)
            // Therefore we only need to check mutually inclusivity of semantically equal guards
            foreach (GuardCode guard in Guards)
            {
                if (!gateCode.Guards.Any(g => g.SemanticallyEqual(guard)))
                {
                    return false;
                }
            }
            foreach (GuardCode guard in gateCode.Guards)
            {
                if (!Guards.Any(g => g.SemanticallyEqual(guard)))
                {
                    return false;
                }
            }

            return Gate.SemanticallyEqual(gateCode.Gate);
        }

        /// <summary>
        /// Checks if the parameters of the gate application are semantically equal to the <paramref name="parameter"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected bool CheckParameterSemanticEquality(List<QubitCode> parameter)
        {
            if (parameter.Count != Parameters.Count)
            {
                return false;
            }

            for (int i = 0; i < Parameters.Count; i++)
            {
                if (!Parameters[i].SemanticallyEqual(parameter[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}