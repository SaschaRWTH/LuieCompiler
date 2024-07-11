using LUIECompiler.CodeGeneration.Statements;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.CodeGeneration
{
    /// <summary>
    /// Represents a code block in the code generation.
    /// </summary>
    public class CodeBlock : ITranslateable
    {
        /// <summary>
        /// List of statements in the code block.
        /// </summary>
        public List<ITranslateable> Translateables { get; } = [];

        /// <summary>
        /// Maps definitions to their unique identifiers given while generating.
        /// </summary>
        public Dictionary<Definition, UniqueIdentifier> IdentifierMap { get; } = [];

        /// <summary>
        /// Parent code block. If null, this is the main block.
        /// </summary>
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
                    IdentifierMap.Add(definition, new UniqueIdentifier(context.SymbolTable));
                }
                code += statement.ToQASM(context);
            }

            return code;
        }

        /// <summary>
        /// Gets the unique identifier for the given definition.
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        public UniqueIdentifier GetUniqueIdentifier(Definition definition)
        {
            if(IdentifierMap.TryGetValue(definition, out var identifier))
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

        public Definition GetDefinition(Register register)
        {

            Definition? definition = null;
            try
            {
                definition = Translateables.OfType<Definition>().SingleOrDefault(def => def.Register == register);
            }
            catch (InvalidOperationException)
            {
                throw new InternalException()
                {
                    Reason = "Multiple definitions found for the same register. This should have been caught by the semantic analysis and indicates a compiler error."
                };
            }

            if(definition != null)
            {
                return definition;
            }

            if (Parent == null)
            {
                throw new CodeGenerationException()
                {
                    Error = new UndefinedError(register.ErrorContext, register.Identifier)
                };
            }
            
            return Parent.GetDefinition(register);
        }

    }

}