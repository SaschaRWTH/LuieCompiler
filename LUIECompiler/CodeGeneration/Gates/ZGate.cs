
namespace LUIECompiler.CodeGeneration.Gates
{
    public class ZGate : PredefinedGate
    {
        public ZGate() 
        { 
            NumberOfArguments = 1;
        }

        protected override string ToCode()
        {
            return "z";
        }
    }
}