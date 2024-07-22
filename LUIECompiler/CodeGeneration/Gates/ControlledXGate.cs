
namespace LUIECompiler.CodeGeneration.Gates
{
    public class ControlledXGate : PredefinedGate
    {
        public ControlledXGate() 
        { 
            NumberOfArguments = 2;
        }

        protected override string ToCode()
        {
            return "cx";
        }
    }
}