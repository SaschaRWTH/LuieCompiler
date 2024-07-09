using LUIECompiler.CodeGeneration;
using LUIECompiler.Common.Symbols;

namespace LUIECompilerTests.CodeGeneration;

[TestClass, TestCategory("CodeGeneration"), TestCategory("Gates")]
public class GateArgumentTest
{

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
}