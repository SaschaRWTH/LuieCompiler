using LUIECompiler.CodeGeneration.Statements;
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration
{

    public class CodeBlock : ITranslateable
    {
        public List<Statement> Statements { get; } = [];


        public void AddStatement(Statement statement)
        {
            Statements.Add(statement);
        }

        public QASMCode ToQASM()
        {
            QASMCode code = new();
            
            foreach(var statement in Statements)
            {
                code += statement.ToQASM();
            }

            return code;
        }
    }

}