
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;
namespace LUIECompiler.CodeGeneration.Statements
{
    public class ForLoopStatement : Statement
    {
        /// <summary>
        /// The iterator of the loop.
        /// </summary>
        public required LoopIterator Iterator { get; init; }

        /// <summary>
        /// The body of the loop.
        /// </summary>
        public required CodeBlock Body { get; init; }

        public override QASMProgram ToQASM(CodeGenerationContext context)
        {
            QASMProgram program = new();
            for (int i = Iterator.Start; i <= Iterator.End; i++)
            {
                if (context.IntegerConstants.Exists(c => c.Identifier == Iterator.Identifier))
                {
                    throw new NotImplementedException($"The constant {Iterator.Identifier} is already defined in the current scope.");
                }

                // TODO: Create a new body for each iteration.
                CodeBlock block = new() {
                    Parent = Body.Parent,
                };
                block.AddTranslateables(Body.Translateables);

                Constant<int> constant = new Constant<int>(Iterator.Identifier, i, ErrorContext);

                program += block.ToQASM(new([.. context.IntegerConstants, constant])
                {
                    SymbolTable = context.SymbolTable,
                    CurrentBlock = Body,
                });
            }
            return program;
        }
    }

}