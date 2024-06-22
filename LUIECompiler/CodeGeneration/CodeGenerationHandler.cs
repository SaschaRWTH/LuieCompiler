
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.CodeGeneration.Statements;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration
{
    public class CodeGenerationHandler
    {
        public SymbolTable Table { get; set; } = new();
        public List<AbstractDefinition> Definitions { get; } = [];
        public Stack<CodeBlock> CodeBlocks { get; } = [];

        /// <summary>
        ///  Main code block of the program.
        /// </summary>
        public CodeBlock MainBlock { get; } = new();

        /// <summary>
        /// Gets the current code block.
        /// </summary>
        public CodeBlock CurrentBlock { get => CodeBlocks.Peek(); }

        public CodeGenerationHandler()
        {
            CodeBlocks.Push(MainBlock);
        }

        /// <summary>
        /// Pushes a new code block onto the stack.
        /// </summary>
        public void PushCodeBlock()
        {
            CodeBlocks.Push(new());
        }

        /// <summary>
        /// Pops the current code block.
        /// </summary>
        /// <returns></returns>
        public CodeBlock PopCodeBlock()
        {
            if (CodeBlocks.Count <= 1)
            {
                // TODO: Error Handling
            }

            return CodeBlocks.Pop();
        }
        
        /// <summary>
        /// Adds the <paramref name="statement"/> to the current <see cref="CodeBlock"/>.
        /// </summary>
        /// <param name="statement"></param>
        public void AddStatement(AbstractStatement statement)
        {
            CurrentBlock.AddStatement(statement);
        }
    }

}