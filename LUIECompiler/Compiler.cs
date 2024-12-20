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
        public static Action<string?> Printer { get; set; } = Console.WriteLine;

        public static bool Verbose { get; set; }

        private static CompilerTimer? _timer = null;

        public static void Compile(CompilerData data)
        {
            Verbose = data.Verbose;
            if (data.Timed)
            {
                _timer = new();
            }

            string input = IOHandler.GetInputCode(data);

            LuieParser.ParseContext parseContext = GetParseContext(input);
            ParseTreeWalker walker = GetParseTreeWalker();

            _timer?.StartStage("Semantic analysis");
            bool @continue = SemanticallyAnalyseCode(walker, parseContext);
            _timer?.StopStage();
            if (!@continue)
            {
                return;
            }

            _timer?.StartStage("Code Generation");
            QASMProgram? program = GenerateCode(walker, parseContext);
            _timer?.StopStage();

            if (program == null)
            {
                return;
            }

            _timer?.StartStage("Code Optimization");
            program = program.Optimize(data.Optimization);
            _timer?.StopStage();

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
            else
            {
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
            Log($"Error: {message}", membName, lineNumber);
        }

        public static void LogWarning(string? message,
            [System.Runtime.CompilerServices.CallerMemberName] string membName = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            Log($"Warning: {message}", membName, lineNumber);
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
            Printer(message);
        }


        private class CompilerTimer
        {
            private readonly System.Diagnostics.Stopwatch _stopwatch = new();
            private string _stage = string.Empty;

            public void StartStage(string stage)
            {
                _stage = stage;
                _stopwatch.Start();
            }

            public void StopStage()
            {
                _stopwatch.Stop();
                Print($"Compilation stage \"{_stage}\" took {_stopwatch.ElapsedMilliseconds}ms!");

            }
        }

    }

}