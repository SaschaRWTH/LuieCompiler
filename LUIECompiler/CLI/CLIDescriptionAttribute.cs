namespace LUIECompiler.CLI
{
    public class CLIDescriptionAttribute : Attribute
    {
        public string Description { get; }

        public CLIDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}