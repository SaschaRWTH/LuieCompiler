using LUIECompiler.CLI;
using LUIECompiler.Optimization;

namespace LUIECompiler
{
    public class CompilerData
    {
        [CLIParameter('i', "input")]
        [CLIDescribtion("Path to the input file.")]
        public string InputPath { get; set; } = string.Empty;
        
        [CLIParameter('o', "output")]
        [CLIDescribtion("Path to the output file.")]
        public string OutputPath { get; set; } = "output.qasm";
        
        [CLIParameter('O', "optimization")]
        [CLIDescribtion("The type of optimization to apply.")]
        public OptimizationType Optimization { get; set; } = OptimizationType.None;
        
        [CLIParameter('v', "verbose")]
        [CLIDescribtion("Path to the input file.")]
        public bool Verbose { get; set; } = false;
    }
}