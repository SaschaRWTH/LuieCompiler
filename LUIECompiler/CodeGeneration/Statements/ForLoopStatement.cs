
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;
namespace LUIECompiler.CodeGeneration.Statements
{
    /// <summary>
    /// Represents a for loop.
    /// </summary>
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
            int start = Iterator.Start.Evaluate(context);
            int end = Iterator.End.Evaluate(context);

            // Create temporary scope where the iterator is defined.
            // Very hacky way to add the iterator to the symbol table.
            context.SymbolTable.PushScope(new CodeBlock()
            {
                Parent = Body,
            });

            // Add symbol only if it is not already defined.
            // e.g. it is not defined for a for loop in a composite gate.
            if (!context.SymbolTable.IsDefined(Iterator.Identifier))
            {
                context.SymbolTable.AddSymbol(Iterator);
            }

            for (Iterator.CurrentValue = start; Iterator.CurrentValue <= end; Iterator.CurrentValue++)
            {
                CodeBlock block = new()
                {
                    Parent = context.CurrentBlock,
                    Translateables = Body.Translateables,
                    // Dont pass on the identifier map, causes tests to fail.
                    // IdentifierMap = Body.IdentifierMap,
                };
                // block.AddTranslateables(Body.Translateables);

                program += block.ToQASM(new(context.ParameterMap)
                {
                    SymbolTable = context.SymbolTable,
                    CurrentBlock = block,
                });
            }

            // Remove temporary scope.
            context.SymbolTable.PopScope();

            return program;
        }
    }

}