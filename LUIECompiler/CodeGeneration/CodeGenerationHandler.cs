
using System.Diagnostics.CodeAnalysis;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.CodeGeneration.Statements;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration
{
    public class CodeGenerationHandler
    {
        /// <summary>
        /// The symbol table of the program.
        /// </summary>
        public SymbolTable Table { get; set; } = new();

        /// <summary>
        /// Dictionary mapping all registers to a definition.
        /// </summary>
        public Dictionary<Register, Definition> DefinitionDictionary { get; } = [];

        /// <summary>
        /// List of all definitions. 
        /// </summary>
        public List<Definition> Definitions { get; } = [];

        /// <summary>
        /// Stack of currently nested code blocks.
        /// </summary>
        public Stack<CodeBlock> CodeBlocks { get; } = [];

        /// <summary>
        /// Stack of the registers guarding the if-clauses.
        /// </summary>
        public Stack<Qubit> GuardStack { get; } = [];

        /// <summary>
        ///  Main code block of the program.
        /// </summary>
        public CodeBlock MainBlock { get; } = new();

        /// <summary>
        /// Gets the current code block.
        /// </summary>
        public CodeBlock CurrentBlock
        {
            get => CodeBlocks.Peek()
                ?? throw new InternalException()
                {
                    Reason = "Tried to peek empty code block stack.",
                };
        }

        /// <summary>
        /// Gets guard of the current if statement.
        /// </summary>
        public Qubit CurrentGuard
        {
            get => GuardStack.Peek()
                ?? throw new InternalException()
                {
                    Reason = "Tried to peek empty guard stack.",
                };
        }

        /// <summary>
        /// Pushes a new code block onto the stack.
        /// </summary>
        public void PushCodeBlock()
        {
            Table.PushScope();
            if (CodeBlocks.Count == 0)
            {
                CodeBlocks.Push(MainBlock);
            }
            else
            {
                CodeBlocks.Push(new());
            }
        }

        /// <summary>
        /// Pops the current code block.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public CodeBlock PopCodeBlock()
        {
            if (CodeBlocks.Count <= 0)
            {
                throw new InternalException()
                {
                    Reason = "Tried to pop empty code block stack.",
                };
            }

            Table.PopScope();
            return CodeBlocks.Pop();
        }

        /// <summary>
        /// Adds the <paramref name="statement"/> to the current <see cref="CodeBlock"/>.
        /// </summary>
        /// <param name="statement"></param>
        public void AddStatement([NotNull] Statement statement)
        {
            CurrentBlock.AddStatement(statement);
        }

        /// <summary>
        /// Pushes a given <paramref name="info"/> onto the guard stack.
        /// </summary>
        /// <param name="info"></param>
        public void PushGuard([NotNull] Qubit info)
        {
            GuardStack.Push(info);
        }

        /// <summary>
        /// Pops the current guard stack.
        /// </summary>
        /// <returns></returns>
        public Qubit PopGuard()
        {
            return GuardStack.Pop();
        }

        public Qubit AddQubit(string identifier, int line)
        {            
            if (Table.IsDefinedInCurrentScop(identifier))
            {
                throw new CodeGenerationException()
                {
                    Error = new RedefineError(line, identifier),
                };
            }

            Qubit info = new(identifier);
            string id = Table.AddSymbol(info);

            RegisterDefinition definition = new()
            {
                Identifier = id,
                Size = 1,
            };

            Definitions.Add(definition);

            DefinitionDictionary.Add(info, definition);

            return info;
        }

        /// <summary>
        /// Adds a register to the code generation handler. This includes adding it to the symbol table,
        /// creating a definition with a unique id, and adding the definition the the definition dictionary.
        /// </summary>
        /// <param name="identifier"></param>
        /// <exception cref="RedefineError"></exception>
        public Register AddRegister(string identifier, int size, int line)
        {
            if (Table.IsDefinedInCurrentScop(identifier))
            {
                throw new CodeGenerationException()
                {
                    Error = new RedefineError(line, identifier),
                };
            }

            Register info = new(identifier, size);
            string id = Table.AddSymbol(info);

            RegisterDefinition definition = new()
            {
                Identifier = id,
                Size = size,
            };

            Definitions.Add(definition);

            DefinitionDictionary.Add(info, definition);

            return info;
        }

        /// <summary>
        /// Generates the entier QASM program for the code generation handler.
        /// </summary>
        /// <returns></returns>
        public QASMProgram GenerateCode()
        {
            QASMProgram code = new();
            foreach (Definition definition in Definitions)
            {
                code += definition.ToQASM();
            }

            code += MainBlock.ToQASM();

            return code;
        }


        /// <summary>
        /// Gets the symbol information based on an identifier.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Symbol GetSymbolInfo(string identifier, int line)
        {
            return Table.GetSymbolInfo(identifier) ?? throw new CodeGenerationException()
            {
                Error = new UndefinedError(line, identifier)
            };

        }
    }

}