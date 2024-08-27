using LUIECompiler.CLI;

namespace LUIECompiler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CompilerData? data = CommandLineParser.ParseArguments(args);
            
            if (data == null)
            {
                Compiler.PrintError("An error occured while parsing the commandline arguments. Could not continue.");
                return;
            }

            Compiler.Compile(data);
        }
    }
}