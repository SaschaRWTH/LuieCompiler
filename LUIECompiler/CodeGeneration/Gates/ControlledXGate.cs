
namespace LUIECompiler.CodeGeneration.Gates
{
    public class ControlledXGate : Gate
    {
        public ControlledXGate() 
        { 
            NumberOfArguments = 2;
        }

        public override string ToCode()
        {
            return "cx";
        }
    }
}