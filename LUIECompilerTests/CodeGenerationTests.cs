using LUIECompiler.CodeGeneration;
using LUIECompiler.Common.Symbols;

namespace LUIECompilerTests;

[TestClass]
public class CodeGenerationTests
{
    public const string SimpleInput =
        "qubit c;\n" +
        "qubit y;\n" +
        "x y;\n" +
        "qif y do\n" +
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
        "qubit id2;\n" +
        "x id1;\n" +
        "ctrl(1) @ x id1, id0;\n" +
        "ctrl(1) @ h id1, id0;\n" +
        "ctrl(1) @ h id1, id2;\n";

    [TestMethod]
    public void SimpleInputTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleInput);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(code, SimpleInputTranslation);
    }


    [TestMethod]
    public void ScopeCorrectInfoTest()
    {
        CodeGenerationHandler handler = new();

        handler.PushCodeBlock();
        Register firstA = handler.AddRegister("A", 1);

        handler.PushCodeBlock();
        Register secondA = handler.AddRegister("A", 2);

        Register? secondScopeA = handler.GetSymbolInfo("A", 3) as Register;
        Assert.IsNotNull(secondScopeA);
        Assert.AreNotEqual(firstA, secondScopeA);
        Assert.AreEqual(secondA, secondScopeA);

        handler.PopCodeBlock();
        Register? firstScopeA = handler.GetSymbolInfo("A", 4) as Register;
        Assert.IsNotNull(secondScopeA);
        Assert.AreNotEqual(secondA, firstScopeA);
        Assert.AreEqual(firstA, firstScopeA);
    }
}