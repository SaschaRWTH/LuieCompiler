
namespace LUIECompiler.CodeGeneration.Gates
{
    public class XGate : Gate
    {
        public XGate() 
        { 
            NumberOfArguments = 1;
        }

        public override string ToCode()
        {
            return "x";
        }
    }
}