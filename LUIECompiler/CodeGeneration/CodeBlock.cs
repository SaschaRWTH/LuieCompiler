using LUIECompiler.CodeGeneration.Statements;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;
using LUIECompiler.CodeGeneration.Definitions;

namespace LUIECompiler.CodeGeneration
{

    public class CodeBlock : ITranslateable
    {
        /// <summary>
        /// List of statements in the code block.
        /// </summary>
        public List<ITranslateable> Translateables { get; } = [];

        /// <summary>
        /// Adds a statement to the code block.
        /// </summary>
        /// <param name="statement"></param>
        public void AddTranslateable(ITranslateable statement)
        {
            Translateables.Add(statement);
        }

        public QASMProgram ToQASM(List<Constant<int>> constants)
        {
            QASMProgram code = new();

            foreach (var statement in Translateables)
            {
                code += statement.ToQASM(constants);
            }

            return code;
        }
    }

}