using LUIECompiler.CodeGeneration;
using LUIECompiler.Common.Symbols;

namespace LUIECompilerTests.CodeGeneration;

[TestClass]
public class RegisterTest
{
    public const string RegisterSizeInput =
            "qubit[3] a;\n" +
            "qubit[6] b;\n" +
            "qubit[9] c;";
            
    /// <summary>
    /// Beware: with changes to the translation code, the translations could change, while still being correct!
    /// </summary>
    public const string RegisterSizeTranslation =
            "qubit[3] id0;\n" +
            "qubit[6] id1;\n" +
            "qubit[9] id2;\n";

    public const string RegisterAccessInput =
            "qubit[3] c;\n" +
            "qubit[3] a;\n" +
            "x c[1];\n" +
            "qif c[1] do\n" +
            "qubit[3] a;\n" +
            "x c[1];\n" +
            "h a[2];\n" +
            "end";
                  
    /// <summary>
    /// Beware: with changes to the translation code, the translations could change, while still being correct!
    /// </summary>
    public const string RegisterAccessTranslation =
            "qubit[3] id0;\n" +
            "qubit[3] id1;\n" +
            "qubit[3] id2;\n" +
            "x id0[1];\n" +
            "ctrl(1) @ x id0[1], id0[1];\n" +
            "ctrl(1) @ h id0[1], id2[2];\n";

    /// <summary>
    /// Tests if the scope is correctly handled in the input.
    /// </summary>
    [TestMethod]
    public void RegisterSizeTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(RegisterSizeInput);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(code, RegisterSizeTranslation);
    }

    /// <summary>
    /// Tests if the scope is correctly handled in the input.
    /// </summary>
    [TestMethod]
    public void RegisterAccessTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(RegisterAccessInput);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(code, RegisterAccessTranslation);
    }
}