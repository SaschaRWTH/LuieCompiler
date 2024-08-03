using LUIECompiler.CodeGeneration;
using LUIECompiler.Common.Symbols;

namespace LUIECompilerTests.CodeGeneration;

[TestClass, TestCategory("CodeGeneration")]
public class BasicTest
{
    public const string SimpleInput =
        "qubit c;\n" +
        "qubit a;\n" +
        "x a;\n" +
        "qif a do\n" +
        "qubit t;" +
        "x c;\n" +
        "h c;\n" +
        "h t;\n" +
        "end";

    /// <summary>
    /// Beware: with changes to the translation code, the translations could change, while still being correct!
    /// </summary>
    public const string SimpleInputTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id1;\n" +
        "qubit id2;\n" +
        "ctrl(1) @ x id1, id0;\n" +
        "ctrl(1) @ h id1, id0;\n" +
        "ctrl(1) @ h id1, id2;\n";

    
    public const string ParameterizedGate = 
        "qubit a;\n" +
        "p(1/8) a;";

    public const string ParameterizedGateTranslation = 
        "qubit id0;\n" +
        "p(pi * 0.125) id0;\n";

    /// <summary>
    /// Tests the code generation for the simple input.
    /// </summary>
    [TestMethod]
    public void SimpleInputTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleInput);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SimpleInputTranslation, code);
    }

    /// <summary>
    /// Tests the code generation for a parameterized gate.
    /// </summary>
    [TestMethod]
    public void ParameterizedGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(ParameterizedGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(ParameterizedGateTranslation, code);
    }
}