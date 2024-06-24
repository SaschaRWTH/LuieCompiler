using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Codes
{
    public class GateCode : Code
    {
        public required List<GateGuard> Guards { get; init; }
        public List<GateGuard> PositiveGuards { get => Guards.Where(g => !g.Negated).ToList(); }
        public List<GateGuard> NegativeGuards { get => Guards.Where(g => g.Negated).ToList(); }
        public required Gate Gate { get; init; }
        // TODO: extend to multiple params
        public required RegisterDefinition Register { get; init; }

        public override string ToCode()
        {
            if (Guards.Count == 0)
            {
                return $"{Gate} {Register.Identifier};";
            }

            if (NegativeGuards.Count == 0)
            {
                return $"ctrl({PositiveGuards.Count}) @ {Gate} {string.Join(", ", PositiveGuards)}, {Register.Identifier};";
            }

            if (PositiveGuards.Count == 0)
            {
                return $"negctrl({NegativeGuards.Count}) @ {Gate} {string.Join(", ", NegativeGuards)}, {Register.Identifier};";
            }


            return $"negctrl({NegativeGuards.Count}) @ ctrl({PositiveGuards.Count}) @" +
                   $"{Gate} {string.Join(", ", NegativeGuards)}," +
                   $"{string.Join(", ", PositiveGuards)}, {Register.Identifier};";
        }
    }
}