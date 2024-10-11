using LUIECompiler.CodeGeneration;
using LUIECompiler.Common.Symbols;

namespace LUIECompilerTests.CodeGeneration;

[TestClass, TestCategory("CodeGeneration"), TestCategory("Scope")]
public class ScopeTest
{
    public const string ScopeInput =
        "qubit c;\n" +
        "qubit a;\n" +
        "x a;\n" +
        "qif a do\n" +
        "   qubit b;\n" +
        "   qubit t;" +
        "   x c;\n" +
        "   h c;\n" +
        "   h t;\n" +
        "   h b;\n" +
        "end";

    /// <summary>
    /// Beware: with changes to the translation code, the translations could change, while still being correct!
    /// </summary>
    public const string ScopeInputTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id1;\n" +
        "qubit id2;\n" +
        "qubit id3;\n" +
        "ctrl(1) @ x id1, id0;\n" +
        "ctrl(1) @ h id1, id0;\n" +
        "ctrl(1) @ h id1, id3;\n" +
        "ctrl(1) @ h id1, id2;\n";

    public const string ChangeUsedInScopeInput =                 
        "qubit[5] c;\n" +
        "qubit[5] d;\n" +
        "qif c[0] do\n" +
        "    h c[1];\n" +
        "    qubit[2] e;\n" +
        "    h e[0];\n" +
        "end";

    /// <summary>
    /// Beware: with changes to the translation code, the translations could change, while still being correct!
    /// </summary>
    public const string ChangeUsedInScopeInputTranslation =
        "qubit[5] id0;\n" +
        "qubit[5] id1;\n" +
        "ctrl(1) @ h id0[0], id0[1];\n" +
        "qubit[2] id2;\n" +
        "ctrl(1) @ h id0[0], id2[0];\n";

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

        Assert.AreEqual(ScopeInputTranslation, code);
    }    

    /// <summary>
    /// Tests the translation of the input where an identifier is change mid scope (after beding used).
    /// </summary>
    [TestMethod]
    public void ChangeUsedInScopeTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(ChangeUsedInScopeInput);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(ChangeUsedInScopeInputTranslation, code);
    }        
}