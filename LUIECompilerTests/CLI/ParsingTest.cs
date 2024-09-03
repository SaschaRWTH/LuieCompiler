using LUIECompiler;
using LUIECompiler.CLI;
using LUIECompiler.Optimization;

namespace LUIECompilerTests.CLI;

[TestClass, TestCategory("CLI"), TestCategory("Parsing")]
public class ParsingTest
{
    public static List<string?> Output { get; } = [];

    [TestInitialize]
    public void Initialize()
    {
        Output.Clear();
        Compiler.Printer = Output.Add;
    }

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

    [TestMethod]
    public void FailWithoutInput()
    {
        string outputPath = "output.qasm";
        OptimizationType optimization = OptimizationType.None;
        string[] args =
        [
            "-o", outputPath,
            "-O", optimization.ToCommandLineInput(),
            "-v"
        ];
        CompilerData? data = CommandLineInterface.ParseArguments(args);
        Assert.IsNull(data);

        string? output = Output.Last();
        Assert.IsNotNull(output);
        Assert.AreEqual("Missing input path argument.", output);
    }
}