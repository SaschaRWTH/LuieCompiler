using LUIECompiler.Common;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration
{
    public class CodeGenerationContext
    {
        /// <summary>
        /// The constants in the program.
        /// </summary>
        public List<Constant<int>> IntegerConstants { get; } = [];

        public required CodeBlock CurrentBlock { get; set; }

        public required SymbolTable SymbolTable { get; init; }

        public CodeGenerationContext()
        {
            
        }
        public CodeGenerationContext(List<Constant<int>> integerConstants)
        {
            IntegerConstants = integerConstants;
        }
    }
}