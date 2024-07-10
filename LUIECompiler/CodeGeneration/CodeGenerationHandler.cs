
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
        [Obsolete("Use ITranslateable List instread instead.")]
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
        public CodeBlock MainBlock { get; } = new()
        {
            Parent = null,
        };

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
                CodeBlocks.Push(new()
                {
                    Parent = CurrentBlock,
                });
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
            CurrentBlock.AddTranslateable(statement);
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

        /// <summary>
        /// Adds a qubit to the code generation handler. This includes adding it to the symbol table,
        /// creating a definition with a unique id, and adding the definition the the definition dictionary.
        /// <param name="identifier">Identifier of the qubit</param>
        /// <param name="context">Line of the declaration</param>
        /// <returns></returns>
        public Qubit AddQubit(string identifier, ErrorContext context)
        {
            Qubit info = new(identifier, context);

            AddSymbol(info, context);
            string id = Table.UniqueIdentifier;

            RegisterDefinition definition = new()
            {
                Identifier = id,
                Size = 1,
            };

            CurrentBlock.AddTranslateable(definition);

            DefinitionDictionary.Add(info, definition);

            return info;
        }

        /// <summary>
        /// Adds a register to the code generation handler. This includes adding it to the symbol table,
        /// creating a definition with a unique id, and adding the definition the the definition dictionary.
        /// <param name="identifier">Identifier of the register</param>
        /// <param name="size">Size of the register</param>
        /// <param name="context">Line of the declaration</param>
        /// <returns></returns>
        public Register AddRegister(string identifier, int size, ErrorContext context)
        {
            Register info = new(identifier, size, context);

            AddSymbol(info, context);
            string id = Table.UniqueIdentifier;

            RegisterDefinition definition = new()
            {
                Identifier = id,
                Size = size,
            };

            CurrentBlock.AddTranslateable(definition);

            DefinitionDictionary.Add(info, definition);

            return info;
        }

        /// <summary>
        /// Adds an iterator to the symbol table.
        /// </summary>
        /// <param name="iterator"></param>
        /// <param name="context"></param>
        public void AddIterator(LoopIterator iterator, ErrorContext context)
        {
            AddSymbol(iterator, context);
        }

        /// <summary>
        /// Adds a symbol to the symbol table.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="context"></param>
        /// <exception cref="CodeGenerationException"></exception>
        protected void AddSymbol(Symbol symbol, ErrorContext context)
        {
            if (Table.IsDefinedInCurrentScop(symbol.Identifier))
            {
                throw new CodeGenerationException()
                {
                    Error = new RedefineError(context, symbol.Identifier),
                };
            }
            Table.AddSymbol(symbol);
        }

        /// <summary>
        /// Generates the entier QASM program for the code generation handler.
        /// </summary>
        /// <returns></returns>
        public QASMProgram GenerateCode()
        {
            QASMProgram code = new();

            code += MainBlock.ToQASM(new());

            return code;
        }


        /// <summary>
        /// Gets the symbol information based on an identifier.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Symbol GetSymbolInfo(string identifier, ErrorContext context)
        {
            return Table.GetSymbolInfo(identifier) ?? throw new CodeGenerationException()
            {
                Error = new UndefinedError(context, identifier)
            };

        }
    }

}