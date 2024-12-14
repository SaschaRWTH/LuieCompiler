namespace LUIECompiler.CLI
{
    public class CLIParameterAttribute : Attribute
    {
        /// <summary>
        /// The short name of the parameter.
        /// </summary>
        public char ShortName { get; }

        /// <summary>
        /// The long name of the parameter.
        /// </summary>
        public string LongName { get; }

        /// <summary>
        /// Indicates whether the parameter should be hidden from the help message.
        /// </summary>
        public bool Hidden { get; } = false;

        public CLIParameterAttribute(char shortName, string longName, bool hidden = false)
        {
            ShortName = shortName;
            LongName = longName;
            Hidden = hidden;
        }

        public object ParseArguments(string[] args, ref int pointer)
        {
            return ShortName switch
            {
                'i' => CommandLineInterface.ParsePath(args, ref pointer),
                'o' => CommandLineInterface.ParsePath(args, ref pointer),
                'O' => CommandLineInterface.ParseOptimization(args, ref pointer),
                'v' => true,
                't' => true,
                _ => throw new ArgumentException($"Unknown parameter: {ShortName}"),
            };
        }

        public bool Matches(string arg)
        {
            return arg == $"-{ShortName}" || arg == $"--{LongName}";
        }

        public override string ToString()
        {
            return $"-{ShortName}, --{LongName}";
        }
    }
}