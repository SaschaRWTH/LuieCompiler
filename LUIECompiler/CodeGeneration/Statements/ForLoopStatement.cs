
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

        public override QASMProgram ToQASM(List<Constant<int>> constants)
        {
            QASMProgram program = new();
            for (int i = Iterator.Start; i <= Iterator.End; i++)
            {
                if(constants.Exists(c => c.Identifier == Iterator.Identifier))
                {
                    throw new NotImplementedException($"The constant {Iterator.Identifier} is already defined in the current scope.");
                }

                constants.Add(new Constant<int>(Iterator.Identifier, i));

                program += Body.ToQASM(constants);
            }
            return program;
        }
    }

}