using LUIECompiler.CodeGeneration;
using LUIECompiler.Common.Symbols;

namespace LUIECompilerTests;

[TestClass]
public class CodeGenerationTest
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
        "qubit id2;\n" +
        "x id1;\n" +
        "ctrl(1) @ x id1, id0;\n" +
        "ctrl(1) @ h id1, id0;\n" +
        "ctrl(1) @ h id1, id2;\n";

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

    public const string GateParamInput = 
            "qubit[3] c;\n" +
            "qubit[3] a;\n" +
            "qubit b;\n" +
            "x c[1];\n" +
            "qif c[1] do\n" +
            "x c[1];\n" +
            "cx c[0], a[2];\n" +
            "ccx c[0], b, a[1];\n" +
            "end";
            
    /// <summary>
    /// Beware: with changes to the translation code, the translations could change, while still being correct!
    /// </summary>
    public const string GateParamTranslation = 
            "qubit[3] id0;\n" +
            "qubit[3] id1;\n" +
            "qubit id2;\n" +
            "x id0[1];\n" +
            "ctrl(1) @ x id0[1], id0[1];\n" +
            "ctrl(1) @ cx id0[1], id0[0], id1[2];\n" +
            "ctrl(1) @ ccx id0[1], id0[0], id2, id1[1];\n";

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

        Assert.AreEqual(code, SimpleInputTranslation);
    }

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

    /// <summary>
    /// Tests if the scope is correctly handled in the input.
    /// </summary>
    [TestMethod]
    public void GateParamTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(GateParamInput);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(code, GateParamTranslation);
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