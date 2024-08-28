using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using LUIECompiler.CLI;
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.SemanticAnalysis;

namespace LUIECompiler
{
    public static class Compiler
    {
        public static bool Verbose { get; private set; }

        public static void Compile(CompilerData data)
        {
            Verbose = data.Verbose;

            string input = IOHandler.GetInputCode(data);

            LuieParser.ParseContext parseContext = GetParseContext(input);
            ParseTreeWalker walker = GetParseTreeWalker();

            bool @continue = SemanticallyAnalyseCode(walker, parseContext);
            if (!@continue)
            {
                return;
            }

            QASMProgram? program = GenerateCode(walker, parseContext);

            if (program == null)
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

        public static bool SemanticallyAnalyseCode(ParseTreeWalker walker, LuieParser.ParseContext parseContext)
        {
            ErrorHandler declarationErrors = DeclarationAnalysis(walker, parseContext);
            ErrorHandler typeCheckingErrors = TypeCheckingAnalysis(walker, parseContext);

            declarationErrors.Warnings.ForEach(Compiler.PrintWarning);
            declarationErrors.CriticalErrors.ForEach(Compiler.PrintError);
            typeCheckingErrors.Warnings.ForEach(Compiler.PrintWarning);
            typeCheckingErrors.CriticalErrors.ForEach(Compiler.PrintError);

            if (!declarationErrors.ContainsCriticalError && !typeCheckingErrors.ContainsCriticalError)
            {
                return true;
            }
            else{
                Compiler.PrintError("Critical errors found in the code. Compilation aborted.");
                return false;
            }
        }

        private static ErrorHandler DeclarationAnalysis(ParseTreeWalker walker, LuieParser.ParseContext parseContext)
        {
            DeclarationAnalysisListener declarationAnalysisListener = new();
            try
            {
                walker.Walk(declarationAnalysisListener, parseContext);
            }
            catch (InternalException e)
            {
                PrintError($"Internal error!: {e.Reason}");
                PrintError($"{e}");
            }
            return declarationAnalysisListener.Error;
        }

        private static ErrorHandler TypeCheckingAnalysis(ParseTreeWalker walker, LuieParser.ParseContext parseContext)
        {
            TypeCheckListener typeCheckListener = new();
            try
            {
                walker.Walk(typeCheckListener, parseContext);
            }
            catch (InternalException e)
            {
                PrintError($"Internal error!: {e.Reason}");
                PrintError($"{e}");
            }
            return typeCheckListener.Error;
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
                PrintError($"Internal error!: {e.Reason}");
                PrintError($"{e}");
            }
            catch (CodeGenerationException e)
            {
                PrintError($"Code gen exception: {e.Error}");
                PrintError($"{e}");
            }
            catch (Exception ex)
            {
                PrintError($"Error: {ex}");
            }
            return program;
        }



        public static void PrintError(object message)
        {
            PrintError(message.ToString());
        }

        public static void PrintError(string? message)
        {
            if (message is null)
            {
                return;
            }
            Print(message);
        }

        public static void PrintWarning(object message)
        {
            PrintWarning(message.ToString());
        }

        public static void PrintWarning(string? message)
        {
            if (message is null)
            {
                return;
            }
            Print(message);
        }

        public static void LogInfo(object message,
            [System.Runtime.CompilerServices.CallerMemberName] string membName = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            Log(message.ToString(), membName, lineNumber);
        }

        public static void LogInfo(string? message,
            [System.Runtime.CompilerServices.CallerMemberName] string membName = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            Log(message, membName, lineNumber);
        }

        public static void LogError(object message,
            [System.Runtime.CompilerServices.CallerMemberName] string membName = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            LogError(message.ToString(), membName, lineNumber);
        }

        public static void LogError(string? message,
            [System.Runtime.CompilerServices.CallerMemberName] string membName = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            Log(message, membName, lineNumber);
        }

        public static void Log(string? message,
            [System.Runtime.CompilerServices.CallerMemberName] string membName = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            if (!Verbose)
            {
                return;
            }

            Print($"{membName}:{lineNumber} >> {message}");
        }
        /// <summary>
        /// Print a message to the console.
        /// </summary>
        /// <param name="message"></param>
        public static void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}