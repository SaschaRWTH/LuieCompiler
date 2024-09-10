
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

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
        public List<GuardCode> Guards { get; }

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
        public GateCode Gate { get; }

        /// <summary>
        /// List of all gate parameters. 
        /// </summary>
        public List<QubitCode> Parameters { get; }

        public GateApplicationCode(GateCode gate, List<QubitCode> parameters, List<GuardCode> guards)
        {
            Gate = gate;
            Parameters = parameters;
            Guards = guards;

            CheckUseOfGuard();
        }

        /// <summary>
        /// Checks if a parameter of the gate application is also a guard (not allowed).
        /// </summary>
        private void CheckUseOfGuard()
        {
            foreach (QubitCode parameter in Parameters)
            {
                if (Guards.Any(g => g.Qubit.SemanticallyEqual(parameter)))
                {
                    Symbol symbol = parameter.Register.Register;
                    throw new CodeGenerationException()
                    {
                        Error = new UseOfGuardError(symbol.ErrorContext, symbol.Identifier)
                    };
                }
            }
        }

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
            string parameters = GetParameters();

            if (NegativeGuards.Count == 0 && PositiveGuards.Count == 0)
            {
                return $"{Gate.ToCode()} {parameters};";
            }

            if (NegativeGuards.Count == 0)
            {
                return $"ctrl({PositiveGuards.Count}) @ {Gate.ToCode()} {string.Join(", ", PositiveGuards.Select(g => g.ToCode()))}, {parameters};";
            }

            if (PositiveGuards.Count == 0)
            {
                return $"negctrl({NegativeGuards.Count}) @ {Gate.ToCode()} {string.Join(", ", NegativeGuards.Select(g => g.ToCode()))}, {parameters};";
            }


            return $"negctrl({NegativeGuards.Count}) @ ctrl({PositiveGuards.Count}) @" +
                   $"{Gate.ToCode()} {string.Join(", ", NegativeGuards.Select(g => g.ToCode()))}," +
                   $"{string.Join(", ", PositiveGuards.Select(g => g.ToCode()))}, {parameters};";
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

        /// <summary>
        /// Checks if the gate application is independent of the <paramref name="code"/>.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool AreIndependent(GateApplicationCode code)
        {
            return IndependentGuards(code) && IndependentParameters(code);
        }

        /// <summary>
        /// Checks if the guards of the gate application are independent of the <paramref name="code"/>.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool IndependentGuards(GateApplicationCode code)
        {
            if (code.Guards.Count == 0)
            {
                return true;
            }

            if (Guards.Count == 0)
            {
                return true;
            }

            return !Guards.Any(g => code.Guards.Any(g.SemanticallyEqual));
        }

        /// <summary>
        /// Checks if the parameters of the gate application are independent of the <paramref name="code"/>.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool IndependentParameters(GateApplicationCode code)
        {
            if (code.Parameters.Count == 0)
            {
                return true;
            }

            if (Parameters.Count == 0)
            {
                return true;
            }

            return !Parameters.Any(p => code.Parameters.Any(p.SemanticallyEqual));
        }

        public override string ToString()
        {
            return $"GateApplicationCode: {Gate.ToCode()}";
        }
    }
}