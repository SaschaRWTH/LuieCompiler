using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using LUIECompiler.SemanticAnalysis;

namespace LUIECompiler
{
    internal class Program
    {
        static void Main(string[] args)
        {    
            string input = 
            "qubit c;\n"+
            "qubit b;\n"+
            "x y;\n"+
            "qubit c;\n"+
            "qif y do\n"+
            "skip;\n"+
            "end";
                
            try
            {
                AntlrInputStream inputStream = new AntlrInputStream(input.ToString());
                LuieLexer luieLexer = new LuieLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(luieLexer);
                LuieParser luieParser = new LuieParser(commonTokenStream);

                ParseTreeWalker walker = new();
                walker.Walk(new DeclarationAnalysisListener(), luieParser.parse());
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);                
            }
        }
    }
}