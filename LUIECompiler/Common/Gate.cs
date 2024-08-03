
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Gates;

namespace LUIECompiler.Common
{
    public enum GateType
    {
        X,
        Y,
        Z,
        H,
        CX,
        CCX,

        P,

        Composite,
    }

    public static class GateTypeExtensions
    {
        public static GateType FromString(string gate)
        {
            return gate switch
            {
                "x" => GateType.X,
                "y" => GateType.Y,
                "z" => GateType.Z,
                "h" => GateType.H,
                "cx" => GateType.CX,
                "ccx" => GateType.CCX,
                "p" => GateType.P,
                _ => GateType.Composite,
            };
        }

        public static int GetNumberOfArguments(this GateType type)
        {
            return type switch
            {
                GateType.X => 1,
                GateType.Y => 1,
                GateType.Z => 1,
                GateType.H => 1,
                GateType.CX => 2,
                GateType.CCX => 3,
                GateType.P => 1,
                _ => 0,
            };
        }
    }

    public class Gate : IGate
    {
        /// <summary>
        /// The number of parameters the gate requires.
        /// </summary>
        public required int NumberOfArguments { get; init; }

        public required GateType Type { get; init; }

        public virtual GateCode ToGateCode(CodeGenerationContext context)
        {
            return Type switch
            {
                GateType.X => new XGate(),
                GateType.Y => new YGate(),
                GateType.Z => new ZGate(),
                GateType.H => new HGate(),
                GateType.CX => new ControlledXGate(),
                GateType.CCX => new ControlledControlledXGate(),
                _ => throw new NotImplementedException()
            };
        }
    }

    public interface IGate
    {
        /// <summary>
        /// The number of parameters the gate requires.
        /// </summary>
        public int NumberOfArguments { get; }

        public GateType Type { get; }
    }
}