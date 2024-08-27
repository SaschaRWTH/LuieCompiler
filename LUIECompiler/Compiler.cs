using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using LUIECompiler.CLI;
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.SemanticAnalysis;

namespace LUIECompiler
{
    public static class Compiler
    {
        public static void Compile(CompilerData data)
        {

            string input = IOHandler.GetInputCode(data);

            LuieParser.ParseContext parseContext = GetParseContext(input);
            ParseTreeWalker walker = GetParseTreeWalker();

            SemanticallyAnalyseCode(walker, parseContext);
            QASMProgram? program = GenerateCode(walker, parseContext);

            if(program == null)
            {
                return;
            }

            program.Optimize(data.Optimization);

            IOHandler.WriteOutputCode(data, program);
        }

        public static LuieParser.ParseContext GetParseContext(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            LuieLexer luieLexer = new LuieLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(luieLexer);
            LuieParser luieParser = new LuieParser(commonTokenStream);

            return luieParser.parse();
        }

        public static ParseTreeWalker GetParseTreeWalker()
        {
            return new ParseTreeWalker();
        }

        public static void SemanticallyAnalyseCode(ParseTreeWalker walker, LuieParser.ParseContext parseContext)
        {
            DeclarationAnalysis(walker, parseContext);            
            TypeCheckingAnalysis(walker, parseContext);            
        }

        private static void DeclarationAnalysis(ParseTreeWalker walker, LuieParser.ParseContext parseContext)
        {
            DeclarationAnalysisListener declarationAnalysisListener = new();
            walker.Walk(declarationAnalysisListener, parseContext);
        }

        private static void TypeCheckingAnalysis(ParseTreeWalker walker, LuieParser.ParseContext parseContext)
        {
            TypeCheckListener typeCheckListener = new();
            walker.Walk(typeCheckListener, parseContext);
        }

        public static QASMProgram? GenerateCode(ParseTreeWalker walker, LuieParser.ParseContext parseContext)
        {
            QASMProgram? program = null;
            try
            {
                CodeGenerationListener codeGenerationListener = new();
                walker.Walk(codeGenerationListener, parseContext);
                program = codeGenerationListener.CodeGen.GenerateCode();
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
            return program;
        }
    }
}