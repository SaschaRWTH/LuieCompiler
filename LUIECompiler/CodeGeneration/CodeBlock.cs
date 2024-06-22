using LUIECompiler.CodeGeneration.Statements;

namespace LUIECompiler.CodeGeneration
{

    public class CodeBlock
    {
        public List<AbstractStatement> Definitions { get; } = [];


        public void AddStatement(AbstractStatement statement)
        {
            Definitions.Add(statement);
        }
    }

}