using System;
using Antlr4.Runtime;

namespace LUIECompiler
{
    internal class Program
    {
        static void Main(string[] args)
        {    
            string input = "qubit c; x q; qubit q; x q;";
                
            try
            {
                AntlrInputStream inputStream = new AntlrInputStream(input.ToString());
                LuieLexer luieLexer = new LuieLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(luieLexer);
                LuieParser luieParser = new LuieParser(commonTokenStream);

                LuieParser.ParseContext parseContext = luieParser.parse();
                
                LuieParser.BlockContext blockContext = parseContext.block();
                
                Console.WriteLine("Statements:");
                foreach(var statement in blockContext.statement()){
                    Console.WriteLine(statement);
                }
                Console.WriteLine("definitions:");
                foreach(var def in blockContext.definition()){
                    Console.WriteLine(def);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);                
            }
        }
    }
}