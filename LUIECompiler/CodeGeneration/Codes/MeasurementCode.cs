using LUIECompiler.CodeGeneration.Definitions;

namespace LUIECompiler.CodeGeneration.Codes
{
    public class MeasurementCode : Code
    {
        public required UniqueIdentifier Target { get; init; }
        public required UniqueIdentifier Storage { get; init; }

        public override bool SemanticallyEqual(Code code)
        {
            if (code is not MeasurementCode measurementCode)
            {
                return false;
            }

            return Target == measurementCode.Target && Storage == measurementCode.Storage;
        }

        public override string ToCode()
        {
            return $"measure {Target.Identifier} -> {Storage.Identifier};";
        }
    }
}