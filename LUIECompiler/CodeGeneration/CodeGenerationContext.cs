using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration
{
    public class CodeGenerationContext
    {
        /// <summary>
        /// The constants in the program.
        /// </summary>
        public List<Constant<int>> IntegerConstants { get; } = [];

        public CodeGenerationContext()
        {
            
        }
        public CodeGenerationContext(List<Constant<int>> integerConstants)
        {
            IntegerConstants = integerConstants;
        }
    }
}