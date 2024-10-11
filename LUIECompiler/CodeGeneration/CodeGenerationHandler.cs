
using System.Diagnostics.CodeAnalysis;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.CodeGeneration.Statements;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Symbols;
using System.Numerics;

namespace LUIECompiler.CodeGeneration
{
    public class CodeGenerationHandler
    {
        /// <summary>
        /// The symbol table of the program.
        /// </summary>
        public SymbolTable Table { get; set; } = new();

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
            get
            {
                return Table.CurrentScope.CodeBlock ?? throw new InternalException()
                {
                    Reason = "Tried to access empty code block.",
                };
            }
        }

        /// <summary>
        /// Pushes a new code block onto the stack.
        /// </summary>
        public void PushScope()
        {
            Table.PushScope(new CodeBlock()
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
            Table.PushScope(MainBlock);
        }

        /// <summary>
        /// Pops the current code block.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public Scope PopCodeBlock()
        {
            return Table.PopScope();
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
        public void PushGuard(Symbol info)
        {
            Table.PushGuard(info);
        }

        /// <summary>
        /// Pops the current guard stack.
        /// </summary>
        /// <returns></returns>
        public Symbol? PopGuard()
        {
            return Table.PopGuard();
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
        /// Adds a constant to the symbol table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constant"></param>
        /// <param name="context"></param>
        public void AddConstant(Symbol constant, ErrorContext context)
        {
            AddSymbol(constant, context);
        }

        /// <summary>
        /// Adds a symbol to the symbol table.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="context"></param>
        /// <exception cref="CodeGenerationException"></exception>
        protected void AddSymbol(Symbol symbol, ErrorContext context)
        {
            if (Table.IsDefined(symbol.Identifier))
            {
                throw new CodeGenerationException()
                {
                    Error = new RedefineError(context, symbol.Identifier),
                };
            }
            Table.AddSymbol(symbol);
        }

        /// <summary>
        /// Generates the entire QASM program for the code generation handler.
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