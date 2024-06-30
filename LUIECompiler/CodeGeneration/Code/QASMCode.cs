using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Codes
{
    public class QASMProgram
    {
        /// <summary>
        /// List of all code lines/entries in the program. 
        /// </summary>
        public List<Code> Code { get; init; } = [];

        public QASMProgram() { }

        public QASMProgram(Code command)
        {
            Code.Add(command);
        }

        public QASMProgram(params Code[] commands)
        {
            Code.AddRange(commands);
        }

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
        public QASMProgram AddControl(string identifier, bool negated = false)
        {
            QASMProgram code = new();

            foreach (Code line in Code)
            {
                if (line is not GateCode gate)
                {
                    continue;
                }

                GateGuard guard = new GateGuard()
                {
                    Identifier = identifier,
                    Negated = negated,
                };

                code.Code.Add(new GateCode()
                {
                    Guards =
                    [
                        guard,
                         .. gate.Guards
                    ],
                    Gate = gate.Gate,
                    Register = gate.Register,
                    Index = gate.Index,
                });
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
    }
}