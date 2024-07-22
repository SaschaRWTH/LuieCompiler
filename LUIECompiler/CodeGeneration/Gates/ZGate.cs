
namespace LUIECompiler.CodeGeneration.Gates
{
    public class ZGate : Gate
    {
        public ZGate() 
        { 
            NumberOfArguments = 1;
        }

        public override string ToCode()
        {
            return "z";
        }
    }
}