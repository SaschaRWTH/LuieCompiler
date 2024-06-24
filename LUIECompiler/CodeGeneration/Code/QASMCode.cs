using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Codes
{
    public class QASMCode
    {
        public List<Code> Code { get; init; } = [];

        public QASMCode() { }

        public QASMCode(Code command)
        {
            Code.Add(command);
        }

        public QASMCode(params Code[] commands)
        {
            Code.AddRange(commands);
        }

        public QASMCode(List<Code> commands)
        {
            Code.AddRange(commands);
        }

        public static QASMCode operator +(QASMCode first, QASMCode second)
        {
            return new()
            {
                Code = [.. first.Code, .. second.Code],
            };
        }

        /// <summary>
        /// Returns a control version of the given <see cref="QASMCode"/>.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="negated"></param>
        /// <returns></returns>
        public QASMCode AddControl(string identifier, bool negated = false)
        {
            QASMCode code = new();

            string control = negated ? "negctrl" : "ctrl";


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