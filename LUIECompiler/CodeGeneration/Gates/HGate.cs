
namespace LUIECompiler.CodeGeneration.Gates
{
    public class HGate : PredefinedGate
    {
        public HGate() 
        { 
            NumberOfArguments = 1;
        }

        protected override string ToCode()
        {
            return "h";
        }
    }
}