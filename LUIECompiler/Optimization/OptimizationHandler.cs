using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Optimization.Rules;

namespace LUIECompiler.Optimization
{
    public class OptimizationHandler
    {
        public QASMProgram Program { get; set; }

        public OptimizationHandler(QASMProgram program)
        {
            Program = program.ShallowCopy();
        }

        public QASMProgram OptimizeSingleQubitNullGates()
        {
          
          
            return Program;
        }




    }
}