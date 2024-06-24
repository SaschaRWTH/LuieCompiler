
namespace LUIECompiler.CodeGeneration.Codes
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
        /// <summary>
        /// Type of the gate.
        /// </summary>
        public GateType Type { get; init; }

        /// <summary>
        /// Create a gate from the <paramref name="context"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public Gate(LuieParser.GateapplicationContext context)
        {
            string gate = context.GATE().GetText();
            Type = gate switch 
            {
                "x" => GateType.X,
                "z" => GateType.Z,
                "h" => GateType.H,
                _ => throw new NotImplementedException()
            };
        }

        public override string ToString()
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