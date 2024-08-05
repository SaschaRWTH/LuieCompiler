using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Gates
{
    /// <summary>
    /// Represents the code for an abstract gate.
    /// </summary>
    public abstract class GateCode : Code
    {
        /// <summary>
        /// Return the code string representation of the gate.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="negativeGuards"></param>
        /// <param name="positiveGuards"></param>
        /// <returns></returns>
        public string GenerateCode(string parameters, List<GuardCode> negativeGuards, List<GuardCode> positiveGuards)
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