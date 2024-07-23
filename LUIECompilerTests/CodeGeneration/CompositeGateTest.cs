using LUIECompiler.CodeGeneration;

namespace LUIECompilerTests.CodeGeneration;

[TestClass, TestCategory("CodeGeneration"), TestCategory("CompositeGate")]
public class CompositeGateTest
{
    public const string SwapDefinition =
        "gate swap(a, b) do\n" +
        "    cx a, b;\n" +
        "    cx b, a;\n" +
        "    cx a, b;\n" +
        "end\n" +
        "\n" +
        "qubit b;\n" +
        "qubit c;\n" +
        "x c;\n" +
        "swap b, c;";
    public const string SwapDefinitionTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id1;\n" +
        "cx id0, id1;\n" +
        "cx id1, id0;\n" +
        "cx id0, id1;\n";

    /// <summary>
    /// Tests the code generation for the simple input.
    /// </summary>
    [TestMethod]
    public void SimpleInputTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SwapDefinition);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SwapDefinitionTranslation, code);
    }
}