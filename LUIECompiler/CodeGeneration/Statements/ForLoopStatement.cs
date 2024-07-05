
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;
namespace LUIECompiler.CodeGeneration.Statements
{
    public class ForLoopStatement : Statement
    {
        public required LoopIterator Iterator { get; init; }

        public required CodeBlock Body { get; init; }

        public override QASMProgram ToQASM()
        {
            QASMProgram program = new();
            for (int i = Iterator.Start; i <= Iterator.End; i++)
            {
                program += Body.ToQASM();
            }
            return program;
        }
    }

}