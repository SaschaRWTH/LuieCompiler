using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Statements;
using LUIECompiler.SemanticAnalysis;

namespace LUIECompiler
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string input =
                "qubit[5] c;\n" +
                "qubit[5] d;\n" +
                "qif c[0] do\n" +
                "    h c[0];\n" +
                "    qubit[2] c;\n" +
                "    h c[0];\n" +
                "end";
                // "qubit[5] c;\n" +
                // "qubit a;\n" +
                // "for i in 0..5 do\n" +
                // "    h c[i];\n" +
                // "    cx c[i], a;\n" +
                // "    for j in 0..99 do\n" +
                // "end";

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
            }
            catch (CodeGenerationException e)
            {
                Console.WriteLine($"Code gen exception: {e.Error}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }
    }
}