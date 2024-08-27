using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CLI
{
    public static class IOHandler
    {
       public static string GetInputCode(CompilerData data)
       {
           if (string.IsNullOrEmpty(data.InputPath))
           {
               throw new ArgumentException("Input file path is required.");
           }

           if (!File.Exists(data.InputPath))
           {
               throw new ArgumentException("Input file does not exist.");
           }

           return File.ReadAllText(data.InputPath);
       }

       public static void WriteOutputCode(CompilerData data, QASMProgram program)
       {
           File.WriteAllText(data.OutputPath, program.ToString());
       }
    }
}