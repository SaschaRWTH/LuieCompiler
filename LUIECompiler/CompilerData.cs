using LUIECompiler.CLI;
using LUIECompiler.Optimization;

namespace LUIECompiler
{
    public class CompilerData
    {
        [CLIParameter('i', "input")]
        [CLIDescription("Path to the input file.")]
        public string InputPath { get; set; } = string.Empty;
        
        [CLIParameter('o', "output")]
        [CLIDescription("Path to the output file.")]
        public string OutputPath { get; set; } = "output.qasm";
        
        [CLIParameter('O', "optimization")]
        [CLIDescription("The type of optimization to apply.")]
        public OptimizationType Optimization { get; set; } = OptimizationType.None;
        
        [CLIParameter('v', "verbose")]
        [CLIDescription("Path to the input file.")]
        public bool Verbose { get; set; } = false;
    }
}