
using System.Diagnostics.CodeAnalysis;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.CodeGeneration.Statements;
using LUIECompiler.CodeGeneration.Code;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration
{
    public class CodeGenerationHandler
    {
        public SymbolTable Table { get; set; } = new();
        public Dictionary<RegisterInfo, AbstractDefinition> DefinitionDictionary { get; } = [];

        public List<AbstractDefinition> Definitions { get; } = [];
        public Stack<CodeBlock> CodeBlocks { get; } = [];

        /// <summary>
        /// Stack of the registers guarding the if-clauses.
        /// </summary>
        public Stack<RegisterInfo> GuardStack { get; } = [];

        /// <summary>
        ///  Main code block of the program.
        /// </summary>
        public CodeBlock MainBlock { get; } = new();

        /// <summary>
        /// Gets the current code block.
        /// </summary>
        public CodeBlock CurrentBlock { get => CodeBlocks.Peek() ?? throw new Exception("No current bock"); }

        /// <summary>
        /// Gets guard of the current if statement.
        /// </summary>
        public RegisterInfo CurrentGuard { get => GuardStack.Peek() ?? throw new Exception("Not in if clause."); }

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
        public void AddStatement([NotNull] AbstractStatement statement)
        {
            CurrentBlock.AddStatement(statement);
        }

        /// <summary>
        /// Pushes a given <paramref name="info"/> onto the guard stack.
        /// </summary>
        /// <param name="info"></param>
        public void PushGuard([NotNull] RegisterInfo info)
        {
            GuardStack.Push(info);
        }

        /// <summary>
        /// Pops the current guard stack.
        /// </summary>
        /// <returns></returns>
        public RegisterInfo PopGuard()
        {
            return GuardStack.Pop();
        }

        /// <summary>
        /// Adds a register to the code generation handler. This includes adding it to the symbol table,
        /// creating a definition with a unique id, and adding the definition the the definition dictionary.
        /// </summary>
        /// <param name="identifier"></param>
        /// <exception cref="Exception"></exception>
        public void AddRegister(string identifier)
        {
            if (Table.IsDefined(identifier))
            {
                throw new Exception("identifier already definined in context");
            }

            RegisterInfo info = new(identifier);
            string id = Table.AddSymbol(info);

            RegisterDefinition definition = new()
            {
                Identifier = id,
            };

            Definitions.Add(definition);

            DefinitionDictionary.Add(info, definition);
        }

        /// <summary>
        /// Generates the entier QASM program for the code generation handler.
        /// </summary>
        /// <returns></returns>
        public QASMCode GenerateCode()
        {
            QASMCode code = new();
            foreach(AbstractDefinition definition in Definitions)
            {
                code += definition.ToQASM();
            }

            code += MainBlock.ToQASM();

            return code;
        }
    }

}