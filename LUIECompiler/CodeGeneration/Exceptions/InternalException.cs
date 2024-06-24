namespace LUIECompiler
{ 
    public class InternalException : Exception
    {
        public required string Reason { get; init; }
    }

}