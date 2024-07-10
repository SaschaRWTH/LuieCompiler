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

        public Dictionary<Register, Definition> DefinitionDictionary { get; } = [];

        public required CodeBlock? Parent { get; init; }

        /// <summary>
        /// Adds a statement to the code block.
        /// </summary>
        /// <param name="statement"></param>
        public void AddTranslateable(ITranslateable statement)
        {
            Translateables.Add(statement);
        }

        public QASMProgram ToQASM(CodeGenerationContext context)
        {
            QASMProgram code = new();

            foreach (var statement in Translateables)
            {
                code += statement.ToQASM(context);
            }

            return code;
        }

        public Definition GetDefinition(Register register)
        {
            if(DefinitionDictionary.TryGetValue(register, out var definition))
            {
                return definition;
            }
            
            if(Parent == null)
            {
                throw new CodeGenerationException()
                {
                    Error = new UndefinedError(register.ErrorContext, register.Identifier),
                };
            }
            
            return Parent.GetDefinition(register);
        }
    }

}