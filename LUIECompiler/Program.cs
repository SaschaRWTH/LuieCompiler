using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Exceptions;

namespace LUIECompiler
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string input =
            "gate qft(reg) do\n" +
            "    for i in range(sizeof(reg)) do;" +
            "    end\n" +
            "end\n" +
            "qubit a;\n" +
            "R2 a;";
            // "gate hReg(reg) do\n" +
            // "    for i in range(sizeof(reg)) do\n" +
            // "        h reg[i];\n" +
            // "    end\n" +
            // "end\n" +
            // "qubit[5] b;\n" +
            // "hReg b;";

            using(var reader = new StreamReader("examples/qft.luie"))
            {
                input = reader.ReadToEnd();
            }
            
            Console.WriteLine("--- LUIE Compiler ---");
            Console.WriteLine("Using input:");
            Console.WriteLine(input);
            Console.WriteLine("---               ---");

            try
            {
                AntlrInputStream inputStream = new AntlrInputStream(input.ToString());
                LuieLexer luieLexer = new LuieLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(luieLexer);
                LuieParser luieParser = new LuieParser(commonTokenStream);

                ParseTreeWalker walker = new();
                // var analysis = new TypeCheckListener();
                // walker.Walk(analysis, luieParser.parse());

                // var error = analysis.Error;
                // if (error.ContainsCriticalError)
                // {
                //     Console.WriteLine("Critical error occured! Cannot compile.");
                // }
                // Console.WriteLine(error.ToString());

                var codegen = new CodeGenerationListener();
                walker.Walk(codegen, luieParser.parse());

                // Console.WriteLine($" Definitions count: {codegen.CodeGen.Definitions.Count}");
                // Console.WriteLine($" main hash: {codegen.CodeGen.MainBlock.GetHashCode()}");
                // Console.WriteLine($" if block  hash: {codegen.CodeGen.MainBlock.Statements.Where(s => s is QuantumIfStatement).Cast<QuantumIfStatement>().ToList()[0].Block.GetHashCode()}");
                // Console.WriteLine($" if statements in main count: {codegen.CodeGen.MainBlock.Statements.Where(s => s is QuantumIfStatement).Count()}");

                Console.WriteLine(codegen.CodeGen.GenerateCode());
            }
            catch (InternalException e)
            {
                Console.WriteLine($"Internal error!: {e.Reason}");
                Console.WriteLine($"{e}");
            }
            catch (CodeGenerationException e)
            {
                Console.WriteLine($"Code gen exception: {e.Error}");
                Console.WriteLine($"{e}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }
    }
}