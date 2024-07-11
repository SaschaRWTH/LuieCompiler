using LUIECompiler.CodeGeneration.Statements;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.CodeGeneration
{

    public class CodeBlock : ITranslateable
    {
        /// <summary>
        /// List of statements in the code block.
        /// </summary>
        public List<ITranslateable> Translateables { get; } = [];

        public Dictionary<Definition, UniqueIdentifier> DefinitionDictionary { get; } = [];

        public required CodeBlock? Parent { get; init; }

        /// <summary>
        /// Adds a statement to the code block.
        /// </summary>
        /// <param name="statement"></param>
        public void AddTranslateable(ITranslateable statement)
        {
            Translateables.Add(statement);
        }

        /// <summary>
        /// Adds a statement to the code block.
        /// </summary>
        /// <param name="statement"></param>
        public void AddTranslateables(IEnumerable<ITranslateable> statements)
        {
            Translateables.AddRange(statements);
        }

        public QASMProgram ToQASM(CodeGenerationContext context)
        {
            context.CurrentBlock = this;
            QASMProgram code = new();
            foreach (var statement in Translateables)
            {
                if(statement is Definition definition)
                {
                    Console.WriteLine($"Adding definition {definition} to dictionary");
                    DefinitionDictionary.Add(definition, new UniqueIdentifier(context.SymbolTable));
                }
                code += statement.ToQASM(context);
            }

            return code;
        }

        public UniqueIdentifier GetUniqueIdentifier(Definition definition)
        {
            if(DefinitionDictionary.TryGetValue(definition, out var identifier))
            {
                return identifier;
            }
            
            if(Parent == null)
            {
                throw new CodeGenerationException()
                {
                    Error = new UndefinedError(definition.Register.ErrorContext, definition.Register.Identifier),
                };
            }
            
            return Parent.GetUniqueIdentifier(definition);
        }


    }

}