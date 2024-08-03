using LUIECompiler.CodeGeneration;
using LUIECompiler.Common.Symbols;

namespace LUIECompilerTests.CodeGeneration;

[TestClass, TestCategory("CodeGeneration")]
public class GeneralTest
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

    public const string QFTGate =
        @"
            gate qft(reg) do
                for i in range(sizeof(reg)) do
                    h reg[i];
                    for j in range(sizeof(reg) - (i + 1)) do
                        qif reg[j + (i + 1)] do
                            p(1/(power(2, (j + 1)))) reg[i];
                        end
                    end
                end
            end
            qubit[5] a;
            qft a;
        ";
    public const string QFTGateTranslation =
        "qubit[5] id0;\n" +
        "h id0[0];\n" +
        "ctrl(1) @ p(pi * 0.5) id0[1], id0[0];\n" +
        "ctrl(1) @ p(pi * 0.25) id0[2], id0[0];\n" +
        "ctrl(1) @ p(pi * 0.125) id0[3], id0[0];\n" +
        "ctrl(1) @ p(pi * 0.0625) id0[4], id0[0];\n" +
        "h id0[1];\n" +
        "ctrl(1) @ p(pi * 0.5) id0[2], id0[1];\n" +
        "ctrl(1) @ p(pi * 0.25) id0[3], id0[1];\n" +
        "ctrl(1) @ p(pi * 0.125) id0[4], id0[1];\n" +
        "h id0[2];\n" +
        "ctrl(1) @ p(pi * 0.5) id0[3], id0[2];\n" +
        "ctrl(1) @ p(pi * 0.25) id0[4], id0[2];\n" +
        "h id0[3];\n" +
        "ctrl(1) @ p(pi * 0.5) id0[4], id0[3];\n" +
        "h id0[4];\n";
        


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

    /// <summary>
    /// Tests the code generation for a parameterized gate.
    /// </summary>
    [TestMethod]
    public void QFTGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(QFTGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(QFTGateTranslation, code);
    }
}