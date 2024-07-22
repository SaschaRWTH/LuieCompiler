
namespace LUIECompiler.CodeGeneration.Gates
{
    public class HGate : Gate
    {
        public HGate() 
        { 
            NumberOfArguments = 1;
        }

        public override string ToCode()
        {
            return "h";
        }
    }
}