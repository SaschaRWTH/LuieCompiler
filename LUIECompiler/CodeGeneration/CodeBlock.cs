using LUIECompiler.CodeGeneration.Statements;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration
{

    public class CodeBlock : ITranslateable
    {
        /// <summary>
        /// List of statements in the code block.
        /// </summary>
        public List<Statement> Statements { get; } = [];


        /// <summary>
        /// Adds a statement to the code block.
        /// </summary>
        /// <param name="statement"></param>
        public void AddStatement(Statement statement)
        {
            Statements.Add(statement);
        }

        public QASMProgram ToQASM(List<Constant<int>> constants)
        {
            QASMProgram code = new();

            foreach (var statement in Statements)
            {
                code += statement.ToQASM(constants);
            }

            return code;
        }
    }

}