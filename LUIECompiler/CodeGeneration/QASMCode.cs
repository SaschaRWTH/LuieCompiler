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

    }
}