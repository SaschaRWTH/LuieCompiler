using LUIECompiler.CodeGeneration;
using LUIECompiler.Common.Symbols;

namespace LUIECompilerTests.CodeGeneration;

[TestClass]
public class ScopeTest
{
    public const string ScopeInput =
            "qubit c;\n" +
            "qubit a;\n" +
            "x a;\n" +
            "qif a do\n" +
            "qubit a;\n" +
            "qubit t;" +
            "x c;\n" +
            "h c;\n" +
            "h t;\n" +
            "h a;\n" +
            "end";

    /// <summary>
    /// Beware: with changes to the translation code, the translations could change, while still being correct!
    /// </summary>
    public const string ScopeInputTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "qubit id2;\n" +
        "qubit id3;\n" +
        "x id1;\n" +
        "ctrl(1) @ x id1, id0;\n" +
        "ctrl(1) @ h id1, id0;\n" +
        "ctrl(1) @ h id1, id3;\n" +
        "ctrl(1) @ h id1, id2;\n";


    /// <summary>
    /// Tests if the scope is correctly handled in the input.
    /// </summary>
    [TestMethod]
    public void ScopeInputTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(ScopeInput);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(code, ScopeInputTranslation);
    }    

    /// <summary>
    /// Tests if the scope is correctly handled.
    /// </summary>
    [TestMethod]
    public void ScopeCorrectInfoTest()
    {
        CodeGenerationHandler handler = new();

        handler.PushCodeBlock();
        Qubit firstA = handler.AddQubit("A", 1);

        handler.PushCodeBlock();
        Qubit secondA = handler.AddQubit("A", 2);

        Qubit? secondScopeA = handler.GetSymbolInfo("A", 3) as Qubit;
        Assert.IsNotNull(secondScopeA);
        Assert.AreNotEqual(firstA, secondScopeA);
        Assert.AreEqual(secondA, secondScopeA);

        handler.PopCodeBlock();
        Qubit? firstScopeA = handler.GetSymbolInfo("A", 4) as Qubit;
        Assert.IsNotNull(secondScopeA);
        Assert.AreNotEqual(secondA, firstScopeA);
        Assert.AreEqual(firstA, firstScopeA);
    }
}