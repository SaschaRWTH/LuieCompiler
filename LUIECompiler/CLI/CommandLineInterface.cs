using System.Reflection;
using LUIECompiler.Optimization;

namespace LUIECompiler.CLI
{
    public static class CommandLineInterface
    {
        /// <summary>
        /// Parse the commandline arguments and returns the compiler data, if the parsing was successful.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static CompilerData? ParseArguments(string[] args)
        {
            CompilerData? data;
            try
            {
                data = InnerParseArguments(args);
            }
            catch (ArgumentException e)
            {
                Compiler.PrintError($"Error parsing the commandline arguments: {e.Message}");
                return null;
            }

            if(data == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(data.InputPath))
            {
                Compiler.PrintError("Missing input path argument.");
                return null;
            }

            return data;
        }

        private static CompilerData? InnerParseArguments(string[] args)
        {
            CompilerData data = new();
            IEnumerable<(PropertyInfo, CLIParameterAttribute)> properties = GetPropertiesWithAttribute<CLIParameterAttribute>(typeof(CompilerData));

            for (int i = 0; i < args.Length; i++)
            {
                bool matched = false;
                foreach (var (prop, attr) in properties)
                {
                    if (attr.Matches(args[i]))
                    {
                        matched = true;
                        object value = attr.ParseArguments(args, ref i);
                        prop.SetValue(data, value);
                        break;
                    }
                }
                if(!matched)
                {
                    if(args[i] == "-h" || args[i] == "--help")
                    {
                        PrintHelp();
                        return null;
                    }
                    else
                    {
                        Compiler.PrintError($"Unknown argument: {args[i]}");
                        PrintHelp();
                        return null;
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// Get all properties with the specified attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerable<(PropertyInfo, T)> GetPropertiesWithAttribute<T>(Type type)
        {
            return from prop in type.GetProperties()
                   where prop.IsDefined(typeof(T), false)
                   select (prop, (T)prop.GetCustomAttributes(typeof(T), false).First());
        }

        /// <summary>
        /// Parse the path argument.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="pointer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string ParsePath(string[] args, ref int pointer)
        {
            if (pointer + 1 >= args.Length)
            {
                throw new ArgumentException("Missing path argument.");
            }

            return args[++pointer];
        }

        /// <summary>
        /// Parse the optimization argument.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="pointer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static OptimizationType ParseOptimization(string[] args, ref int pointer)
        {
            if (pointer + 1 >= args.Length)
            {
                throw new ArgumentException("Missing optimization argument.");
            }

            string arg = args[++pointer];
            if (arg == "all")
            {
                return OptimizationType.All;
            }

            var optimizations = arg.Split('+');
            OptimizationType type = OptimizationType.None;

            foreach (var opt in optimizations)
            {
                type |= OptimizationTypeExtension.FromCommandLineInput(opt);
            }

            return type;
        }

        /// <summary>
        /// Print the help message, describing the available options.
        /// </summary>
        public static void PrintHelp()
        {
            IEnumerable<(PropertyInfo, CLIParameterAttribute)> parameters = GetPropertiesWithAttribute<CLIParameterAttribute>(typeof(CompilerData));
            IEnumerable<(PropertyInfo, CLIDescriptionAttribute)> descriptions = GetPropertiesWithAttribute<CLIDescriptionAttribute>(typeof(CompilerData));
            Compiler.Print("Usage: LUIECompiler [options]");
            Compiler.Print("Options:");
            string help = "-h, --help";
            string helpDescription = "Print help.";  
            Compiler.Print(HelpOptionString(help, helpDescription));
            foreach (var (prop, attr) in parameters)
            {
                if(attr?.Hidden == true)
                {
                    continue;
                }

                string? description = descriptions.FirstOrDefault(x => x.Item1.Name == prop.Name).Item2.Description;
                if(string.IsNullOrEmpty(description))
                {
                    description = "No description available.";
                }
                Compiler.Print(HelpOptionString(attr.ToString(), description));
            }
        }

        public static string HelpOptionString(string option, string description)
        {
            return $"  {option,-24} {description}";
        }
    }
}