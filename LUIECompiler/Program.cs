using LUIECompiler.CLI;

namespace LUIECompiler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CompilerData? data = CommandLineInterface.ParseArguments(args);
            
            if (data == null)
            {
                return;
            }

            Compiler.Compile(data);
        }
    }
}