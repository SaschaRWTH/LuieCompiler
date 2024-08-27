namespace LUIECompiler.CLI
{
    public class CLIParameterAttribute : Attribute
    {
        public char ShortName { get; }
        public string LongName { get; }

        public CLIParameterAttribute(char shortName, string longName)
        {
            ShortName = shortName;
            LongName = longName;
        }

        public object ParseArguments(string[] args, ref int pointer)
        {
            return ShortName switch
            {
                'i' => CommandLineInterface.ParsePath(args, ref pointer),
                'o' => CommandLineInterface.ParsePath(args, ref pointer),
                'O' => CommandLineInterface.ParseOptimization(args, ref pointer),
                'v' => true,
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