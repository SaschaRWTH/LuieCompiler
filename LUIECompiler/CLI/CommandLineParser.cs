using System.Reflection;
using LUIECompiler.Optimization;

namespace LUIECompiler.CLI
{
    public static class CommandLineParser
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

            if (string.IsNullOrEmpty(data.InputPath))
            {
                Compiler.PrintError("Missing input path argument.");
                return null;
            }

            return data;
        }

        private static CompilerData InnerParseArguments(string[] args)
        {
            CompilerData data = new();
            IEnumerable<(PropertyInfo, CLIParameterAttribute)> properties = GetPropertiesWithAttribute<CLIParameterAttribute>(typeof(CompilerData));

            for (int i = 0; i < args.Length; i++)
            {
                foreach (var (prop, attr) in properties)
                {
                    if (attr.Matches(args[i]))
                    {
                        object value = attr.ParseArguments(args, ref i);
                        prop.SetValue(data, value);
                        break;
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

            var optimizations = arg.Split('|');
            OptimizationType type = OptimizationType.None;

            foreach (var opt in optimizations)
            {
                type |= opt switch
                {
                    "nullgate" => OptimizationType.NullGate,
                    "peepingcontrol" => OptimizationType.PeepingControl,
                    "hsandwich" => OptimizationType.HSandwichReduction,
                    "controlreversal" => OptimizationType.ControlReversal,
                    _ => throw new ArgumentException($"Unknown optimization: {opt}"),
                };
            }

            return type;
        }
    }
}