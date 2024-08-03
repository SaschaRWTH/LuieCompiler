
namespace LUIECompiler.CodeGeneration.Gates
{
    public class ControlledControlledXGate : PredefinedGate
    {
        public ControlledControlledXGate() 
        { 
            NumberOfArguments = 3;
        }

        protected override string ToCode()
        {
            return "ccx";
        }
    }
}