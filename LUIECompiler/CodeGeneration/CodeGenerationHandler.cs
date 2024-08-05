
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
        /// Stack of currently nested code blocks.
        /// </summary>
        public Stack<CodeBlock> CodeBlocks { get; } = [];

        /// <summary>
        /// Stack of the registers guarding the if-clauses.
        /// </summary>
        public Stack<Symbol> GuardStack { get; } = [];

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
        public Symbol CurrentGuard
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
            CodeBlocks.Push(new()
            {
                Parent = CurrentBlock,
            });   
        }

        /// <summary>
        /// Pushes the main code block onto the stack.
        /// </summary>
        /// <exception cref="InternalException"></exception>
        public void PushMainCodeBlock()
        {
            if(CodeBlocks.Count > 0)
            {
                throw new InternalException()
                {
                    Reason = "Tried to push main code block onto non-empty stack.",
                };
            }

            Table.PushScope();
            CodeBlocks.Push(MainBlock);
        }

        /// <summary>
        /// Pops the current code block.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public CodeBlock PopCodeBlock()
        {
            if (CodeBlocks.Count <= 1)
            {
                throw new InternalException()
                {
                    Reason = "Tried to pop main code block from stack.",
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
        public void PushGuard([NotNull] Symbol info)
        {
            GuardStack.Push(info);
        }

        /// <summary>
        /// Pops the current guard stack.
        /// </summary>
        /// <returns></returns>
        public Symbol PopGuard()
        {
            return GuardStack.Pop();
        }

        /// <summary>
        /// Adds a register to the code generation handler. This includes adding it to the symbol table,
        /// creating a definition with a unique id, and adding the definition the the definition dictionary.
        /// <param name="identifier">Identifier of the register</param>
        /// <param name="size">Size of the register</param>
        /// <param name="context">Line of the declaration</param>
        /// <returns></returns>
        public Register AddRegister(Register register, ErrorContext context)
        {
            AddSymbol(register, context);

            RegisterDefinition definition = new(register);

            CurrentBlock.AddTranslateable(definition);

            return register;
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
        /// Adds a parameter to the symbol table.
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="context"></param>
        public void AddParameter(Parameter parameter, ErrorContext context)
        {
            AddSymbol(parameter, context);
        }

        /// <summary>
        /// Adds a composite gate to the symbol table.
        /// </summary>
        /// <param name="gate"></param>
        /// <param name="context"></param>
        public void AddCompositeGate(CompositeGate gate, ErrorContext context)
        {
            AddSymbol(gate, context);
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

            CodeGenerationContext context = new()
            {
                SymbolTable = Table,
                CurrentBlock = MainBlock,
            };

            code += MainBlock.ToQASM(context);

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