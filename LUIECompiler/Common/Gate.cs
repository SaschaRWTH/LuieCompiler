namespace LUIECompiler.Common 
{
    public enum GateType
    {
        X,
        Z,
        Y,
        H,
        // ...
    }


    public class Gate 
    {
        public GateType Type { get; init; } 
    }
}