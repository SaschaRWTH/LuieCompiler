using LUIECompiler.CodeGeneration.Statements;

namespace LUIECompiler.CodeGeneration
{

    public class CodeBlock : ITranslateable
    {
        public List<AbstractStatement> Statements { get; } = [];


        public void AddStatement(AbstractStatement statement)
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