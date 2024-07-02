
namespace LUIECompiler.CodeGeneration.Codes
{
    public enum GateType
    {
        X,
        Z,
        Y,
        H,
        CX,
        CCX,
        // ...
    }


    public class Gate
    {
        /// <summary>
        /// Type of the gate.
        /// </summary>
        public GateType Type { get; init; }

        /// <summary>
        /// Returns the number of parameters the gate requires.
        /// </summary>
        public int NumberOfParameters { get; init; }

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
                "y" => GateType.Y,
                "h" => GateType.H,
                "cx" => GateType.CX,
                "ccx" => GateType.CCX,
                _ => throw new NotImplementedException()
            };

            NumberOfParameters = Type switch
            {
                GateType.CX => 2,
                GateType.CCX => 3,
                _ => 1,
            };
        }

        public override string ToString()
        {
            return Type switch
            {
                GateType.X => "x",
                GateType.CX => "cx",
                GateType.CCX => "ccx",
                GateType.Z => "z",
                GateType.Y => "y",
                GateType.H => "h",
                _ => throw new NotImplementedException(),
            };
        }
    }
}