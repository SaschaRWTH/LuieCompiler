using LUIECompiler.CodeGeneration.Gates;

namespace LuieCompiler.CodeGeneration.Gates
{
    public class PhaseGate : GateCode
    {
        public required double Parameter { get; init; } 

        public override string ToCode()
        {
            return $"p(pi * {Parameter})";
        }
    }
}