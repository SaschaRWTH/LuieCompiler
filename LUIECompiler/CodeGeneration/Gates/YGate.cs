
namespace LUIECompiler.CodeGeneration.Gates
{
    public class YGate : Gate
    {
        public YGate() 
        { 
            NumberOfArguments = 1;
        }

        public override string ToCode()
        {
            return "y";
        }
    }
}