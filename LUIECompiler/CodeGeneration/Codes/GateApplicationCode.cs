
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
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
        public GateCode Gate { get; private set; }

        /// <summary>
        /// List of all gate arguments. 
        /// </summary>
        public List<QubitCode> Arguments { get; }

        public GateApplicationCode(GateCode gate, List<QubitCode> arguments, List<GuardCode> guards)
        {
            Gate = gate;
            Arguments = arguments;
            Guards = guards;

            ReplaceControlledGates();
            CheckUseOfGuard();
        }

        private void ReplaceControlledGates()
        {
            if (Gate.GateType == GateType.CX)
            { 
                Gate = new(GateType.X);
                if (Arguments.Count != 2)
                {
                    throw new InternalException()
                    {
                        Reason = "CX gate must have 2 arguments"
                    };
                }
                QubitCode guard = Arguments[0];
                Arguments.Remove(guard);

                Guards.Add(new GuardCode()
                {
                    Qubit = guard,
                    Negated = false
                });

                return;
            }
            if (Gate.GateType == GateType.CCX)
            {
                Gate = new(GateType.X);
                if (Arguments.Count != 3)
                {
                    throw new InternalException()
                    {
                        Reason = "CCX gate must have 3 arguments"
                    };
                }
                QubitCode firstGuard = Arguments[0];
                QubitCode secGuard = Arguments[1];
                Arguments.Remove(firstGuard);
                Arguments.Remove(secGuard);

                Guards.Add(new GuardCode()
                {
                    Qubit = firstGuard,
                    Negated = false
                });
                Guards.Add(new GuardCode()
                {
                    Qubit = secGuard,
                    Negated = false
                });
            }
        }

        /// <summary>
        /// Checks if an argument of the gate application is also a guard (not allowed).
        /// </summary>
        private void CheckUseOfGuard()
        {
            foreach (QubitCode arg in Arguments)
            {
                if (Guards.Any(g => g.Qubit.SemanticallyEqual(arg)))
                {
                    Symbol symbol = arg.Register;
                    throw new CodeGenerationException()
                    {
                        Error = new UseOfGuardError(symbol.ErrorContext, symbol.Identifier)
                    };
                }
            }
        }

        /// <summary>
        ///  Gets the code string representation of all arguments. 
        /// </summary>
        /// <returns></returns>
        private string GetArguments()
        {
            return string.Join(", ", Arguments.Select(param => param.ToCode()));
        }

        public override string ToCode()
        {
            string arguments = GetArguments();

            if (NegativeGuards.Count == 0 && PositiveGuards.Count == 0)
            {
                return $"{Gate.ToCode()} {arguments};";
            }

            if (NegativeGuards.Count == 0)
            {
                return $"ctrl({PositiveGuards.Count}) @ {Gate.ToCode()} {string.Join(", ", PositiveGuards.Select(g => g.ToCode()))}, {arguments};";
            }

            if (PositiveGuards.Count == 0)
            {
                return $"negctrl({NegativeGuards.Count}) @ {Gate.ToCode()} {string.Join(", ", NegativeGuards.Select(g => g.ToCode()))}, {arguments};";
            }


            return $"negctrl({NegativeGuards.Count}) @ ctrl({PositiveGuards.Count}) @" +
                   $"{Gate.ToCode()} {string.Join(", ", NegativeGuards.Select(g => g.ToCode()))}," +
                   $"{string.Join(", ", PositiveGuards.Select(g => g.ToCode()))}, {arguments};";
        }

        public override bool SemanticallyEqual(Code code)
        {
            if (code is not GateApplicationCode gateCode)
            {

                return false;
            }

            CheckArgumentSemanticEquality(gateCode.Arguments);

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
        /// Checks if the arguments of the gate application are semantically equal to the <paramref name="args"/>.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected bool CheckArgumentSemanticEquality(List<QubitCode> args)
        {
            if (args.Count != Arguments.Count)
            {
                return false;
            }

            for (int i = 0; i < Arguments.Count; i++)
            {
                if (!Arguments[i].SemanticallyEqual(args[i]))
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
            return IndependentGuards(code) && IndependentArguments(code);
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
        /// Checks if the argument of the gate application are independent of the <paramref name="code"/>.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool IndependentArguments(GateApplicationCode code)
        {
            if (code.Arguments.Count == 0)
            {
                return true;
            }

            if (Arguments.Count == 0)
            {
                return true;
            }

            return !Arguments.Any(p => code.Arguments.Any(p.SemanticallyEqual));
        }

        public override string ToString()
        {
            return $"GateApplicationCode: {Gate.ToCode()}";
        }
    }
}