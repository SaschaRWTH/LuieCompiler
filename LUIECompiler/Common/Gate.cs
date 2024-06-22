using System.Security.Cryptography;

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


    public class Gate : ITranslateable
    {
        public GateType Type { get; init; }

        public string ToQASM()
        {
            return Type switch
            {
                GateType.X => "x",
                GateType.Z => "z",
                GateType.Y => "y",
                GateType.H => "h",
                _ => throw new NotImplementedException(),
            };
        }
    }
}