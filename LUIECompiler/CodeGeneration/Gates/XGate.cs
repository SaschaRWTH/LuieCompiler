
namespace LUIECompiler.CodeGeneration.Gates
{
    /// <summary>
    /// Represents an X gate.
    /// </summary>
    public class XGate : GateCode
    {

        public override string ToCode()
        {
            return "x";
        }
    }
}