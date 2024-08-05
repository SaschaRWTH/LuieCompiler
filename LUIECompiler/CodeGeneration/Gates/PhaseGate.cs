using System.Globalization;
using LUIECompiler.CodeGeneration.Gates;

namespace LuieCompiler.CodeGeneration.Gates
{
    /// <summary>
    /// Represents a phase gate.
    /// </summary>
    public class PhaseGate : GateCode
    {
        public required double Parameter { get; init; } 

        public override string ToCode()
        {
            return $"p(pi * {Parameter.ToString(CultureInfo.InvariantCulture)})";
        }
    }
}