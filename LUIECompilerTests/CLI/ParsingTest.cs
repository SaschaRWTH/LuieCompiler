using LUIECompiler;
using LUIECompiler.CLI;
using LUIECompiler.Optimization;

namespace LUIECompilerTests.CLI;

[TestClass, TestCategory("CLI"), TestCategory("Parsing")]
public class ParsingTest
{
    [TestMethod]
    public void TestParseArguments()
    {
        string inputPath = "input.qasm";
        string outputPath = "output.qasm";
        OptimizationType optimization = OptimizationType.None;
        string[] args =
        [
            "-i", inputPath,
            "-o", outputPath,
            "-O", optimization.ToCommandLineInput(),
            "-v"
        ];
        CompilerData? data = CommandLineInterface.ParseArguments(args);
        Assert.IsNotNull(data);
        Assert.AreEqual(inputPath, data.InputPath);
        Assert.AreEqual(outputPath, data.OutputPath);
        Assert.AreEqual(optimization, data.Optimization);
        Assert.IsTrue(data.Verbose);
    }
}