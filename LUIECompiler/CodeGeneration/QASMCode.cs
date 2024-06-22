using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration
{
    public class QASMCode 
    {

        public List<string> Code { get; init; } = [];

        public QASMCode() {}        

        public QASMCode(string command){
            Code.Add(command);
        }

        public QASMCode(params string[] commands){
            Code.AddRange(commands);
        }

        public QASMCode(List<string> commands){
            Code.AddRange(commands);
        }

        public static QASMCode operator +(QASMCode first, QASMCode second){
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


            foreach(string line in Code)
            {
                // WARNING: This is not correct and needs to be adjusted, only placeholder
                code.Code.Add($"{control} @ ${identifier} {line}");
            }

            return code;
        }

    }
}