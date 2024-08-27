using LUIECompiler.Optimization;

namespace LUIECompiler.CLI
{
    public static class CommandLineParser
    {
        public static CompilerData ParseArguments(string[] args)
        {
            string input = string.Empty;
            string output = string.Empty;
            OptimizationType optimization = OptimizationType.None;
            bool verbose = false;


            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-i":
                    case "--input":
                        input = args[++i];
                        break;
                    case "-o":
                    case "--output":
                        output = args[++i];
                        break;
                    case "-O":
                    case "--optimization":
                        optimization = ParseOptimization(args[++i]);
                        break;
                    case "-v":
                    case "--verbose":
                        verbose = true;
                        break;
                    default:
                        throw new ArgumentException($"Unknown argument: {args[i]}");
                }
            }

            return new CompilerData()
            {
                InputPath = input,
                OutputPath = output,
                Optimization = optimization,
                Verbose = verbose
            };
        }

        private static OptimizationType ParseOptimization(string arg)
        {
            if (arg == "all")
            {
                return OptimizationType.All;
            }

            var optimizations = arg.Split('|');
            OptimizationType type = OptimizationType.None;

            foreach (var opt in optimizations)
            {
                switch (opt)
                {
                    case "nullgate":
                        type |= OptimizationType.NullGate;
                        break;
                    case "peepingcontrol":
                        type |= OptimizationType.PeepingControl;
                        break;
                    case "hsandwich":
                        type |= OptimizationType.HSandwichReduction;
                        break;
                    case "controlreversal":
                        type |= OptimizationType.ControlReversal;
                        break;
                    default:
                        throw new ArgumentException($"Unknown optimization: {opt}");
                }
            }

            return type;
        }
    }
}