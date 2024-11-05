
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.Common
{
    /// <summary>
    /// Gate types.
    /// </summary>
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
        /// <summary>
        /// Converts a string to the corresponding gate type.
        /// </summary>
        /// <param name="gate"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the number of arguments a gate type requires.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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

        public static string ToCode(this GateType type)
        {
            return type switch
            {
                GateType.X => "x",
                GateType.Y => "y",
                GateType.Z => "z",
                GateType.H => "h",
                GateType.CX => "cx",
                GateType.CCX => "ccx",
                GateType.P => "p",
                _ => throw new NotImplementedException(),
            };
        }
    }

    /// <summary>
    /// Represents a gate.
    /// </summary>
    public class Gate : IGate
    {
        /// <summary>
        /// The number of arguments the gate requires.
        /// </summary>
        public int NumberOfArguments { get => Type.GetNumberOfArguments(); }

        /// <summary>
        /// Type of the gate.
        /// </summary>
        public required GateType Type { get; init; }

        /// <summary>
        /// Converts the gate to gate code.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual GateCode ToGateCode(CodeGenerationContext context)
        {
            return new GateCode(Type);
        }
    }

    /// <summary>
    /// Gate interface.
    /// </summary>
    public interface IGate
    {
        /// <summary>
        /// The number of arguments the gate requires.
        /// </summary>
        public int NumberOfArguments { get; }

        public GateType Type { get; }
    }
}