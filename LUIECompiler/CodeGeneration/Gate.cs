
namespace LUIECompiler.CodeGeneration 
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

        public QASMCode ToQASM()
        {
            return Type switch
            {
                GateType.X => new("x"),
                GateType.Z => new("z"),
                GateType.Y => new("y"),
                GateType.H => new("h"),
                _ => throw new NotImplementedException(),
            };
        }
    }
}