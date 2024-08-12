using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs;

namespace LUIECompiler
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string input = 
            @"
                qubit[5] q;
                for i in range(sizeof(q)) do
                    h q[i];
                end
            ";
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


                QASMProgram program = codegen.CodeGen.GenerateCode();
                Console.WriteLine("Generated program:");
                Console.WriteLine(program);

                Console.WriteLine("Creating Graph:");
                CircuitGraph graph = new(program);
                
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