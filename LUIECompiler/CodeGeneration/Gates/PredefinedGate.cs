
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Gates
{
    public abstract class PredefinedGate : Gate
    {
        protected abstract string ToCode();

        public override string GenerateCode(string parameters, List<GuardCode> negativeGuards, List<GuardCode> positiveGuards)
        {
            
            if (negativeGuards.Count == 0 && positiveGuards.Count == 0)
            {
                return $"{ToCode()} {parameters};";
            }

            if (negativeGuards.Count == 0)
            {
                return $"ctrl({positiveGuards.Count}) @ {ToCode()} {string.Join(", ", positiveGuards.Select(g => g.ToCode()))}, {parameters};";
            }

            if (positiveGuards.Count == 0)
            {
                return $"negctrl({negativeGuards.Count}) @ {ToCode()} {string.Join(", ", negativeGuards.Select(g => g.ToCode()))}, {parameters};";
            }


            return $"negctrl({negativeGuards.Count}) @ ctrl({positiveGuards.Count}) @" +
                   $"{ToCode()} {string.Join(", ", negativeGuards.Select(g => g.ToCode()))}," +
                   $"{string.Join(", ", positiveGuards.Select(g => g.ToCode()))}, {parameters};";
        }
    }
}