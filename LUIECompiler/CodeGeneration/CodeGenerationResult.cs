namespace LUIECompiler.CodeGeneration
{
    public class CodeGenerationResult
    {
        public List<Instruction> Instructions = new();

        public CodeGenerationResult Append(CodeGenerationResult result){
            Instructions.AddRange(result.Instructions);
            return this;
        }
    }

    public class Instruction
    {
        public string Content = "";
    }
}