
namespace LUIECompiler.CodeGeneration.Gates
{
    public class XGate : PredefinedGate
    {
        public XGate() 
        { 
            NumberOfArguments = 1;
        }

        protected override string ToCode()
        {
            return "x";
        }
    }
}