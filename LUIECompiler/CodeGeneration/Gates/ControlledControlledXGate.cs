
namespace LUIECompiler.CodeGeneration.Gates
{
    public class ControlledControlledXGate : Gate
    {
        public ControlledControlledXGate() 
        { 
            NumberOfArguments = 3;
        }

        public override string ToCode()
        {
            return "ccx";
        }
    }
}