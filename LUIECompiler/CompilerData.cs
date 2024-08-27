using LUIECompiler.Optimization;

namespace LUIECompiler
{
    public class CompilerData
    {
        public required string InputPath { get; init; }
        public string OutputPath { get; init; } = "output.qasm";
        public OptimizationType Optimization { get; init; } = OptimizationType.None;
        public bool Verbose { get; init; } = false;
    }
}