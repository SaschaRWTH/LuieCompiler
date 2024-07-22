
namespace LUIECompiler.CodeGeneration.Gates
{
    public class YGate : PredefinedGate
    {
        public YGate() 
        { 
            NumberOfArguments = 1;
        }

        protected override string ToCode()
        {
            return "y";
        }
    }
}