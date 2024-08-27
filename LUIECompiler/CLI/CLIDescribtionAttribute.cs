namespace LUIECompiler.CLI
{
    public class CLIDescribtionAttribute : Attribute
    {
        public string Description { get; }

        public CLIDescribtionAttribute(string description)
        {
            Description = description;
        }
    }
}