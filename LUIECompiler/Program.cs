using LUIECompiler.CLI;

namespace LUIECompiler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CompilerData data = CommandLineParser.ParseArguments(args);
            
            Compiler.Compile(data);
        }
    }
}