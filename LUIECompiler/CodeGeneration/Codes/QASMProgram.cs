
using LUIECompiler.Optimization;

namespace LUIECompiler.CodeGeneration.Codes
{
    public class QASMProgram
    {
        /// <summary>
        /// List of all code lines/entries in the program. 
        /// </summary>
        public List<Code> Code { get; set; } = [];

        /// <summary>
        /// Creates an empty instance of <see cref="QASMProgram"/>.
        /// </summary>
        public QASMProgram() { }

        /// <summary>
        /// Creates an instance of <see cref="QASMProgram"/> with a single <paramref name="command"/>.
        /// </summary>
        /// <param name="command"></param>
        public QASMProgram(Code command)
        {
            Code.Add(command);
        }

        /// <summary>
        /// Creates an instance of <see cref="QASMProgram"/> with multiple <paramref name="commands"/>.
        /// </summary>
        /// <param name="commands"></param>
        public QASMProgram(params Code[] commands)
        {
            Code.AddRange(commands);
        }

        /// <summary>
        /// Creates an instance of <see cref="QASMProgram"/> with multiple <paramref name="commands"/>.
        /// </summary>
        /// <param name="commands"></param>
        public QASMProgram(List<Code> commands)
        {
            Code.AddRange(commands);
        }

        /// <summary>
        /// Adds together two programs by appending the <paramref name="second"/> to the <paramref name="first"/>.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static QASMProgram operator +(QASMProgram first, QASMProgram second)
        {
            return new()
            {
                Code = [.. first.Code, .. second.Code],
            };
        }

        /// <summary>
        /// Returns a control version of the given <see cref="QASMProgram"/>.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="negated"></param>
        /// <returns></returns>
        public QASMProgram AddGuard(QubitCode qubit, bool negated = false)
        {
            QASMProgram code = new();

            foreach (Code line in Code)
            {
                if (line is not GateApplicationCode gate)
                {
                    // TODO: Change to shallow copy?
                    code.Code.Add(line);
                    continue;
                }

                GuardCode guard = new GuardCode()
                {
                    Qubit = qubit,
                    Negated = negated,
                };

                code.Code.Add(new GateApplicationCode(
                    guards:
                    [
                        guard,
                         .. gate.Guards
                    ],
                    gate: gate.Gate,
                    parameters:
                    [
                        .. gate.Parameters
                    ]
                ));
            }

            return code;
        }

        public override string ToString()
        {
            string code = "";
            foreach (Code line in Code)
            {
                code += $"{line.ToCode()}\n";
            }
            return code;
        }

        public QASMProgram ShallowCopy()
        {
            return new()
            {
                Code = [.. Code],
            };
        }

        /// <summary>
        /// Optimizes the program and returns the number by which the gate count was reduced.
        /// </summary>
        /// <returns></returns>
        public QASMProgram Optimize(OptimizationType optimization = OptimizationType.All)
        {
            int gateCount = Code.Count(c => c is GateApplicationCode);

            OptimizationHandler handler = new(this);

            QASMProgram program = handler.OptimizeSingleQubitNullGates(optimization);

            return program;
        }
    }
}