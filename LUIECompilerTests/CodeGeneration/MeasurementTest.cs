using LUIECompiler.CodeGeneration;

namespace LUIECompilerTests.CodeGeneration;

[TestClass]
public class MeasurementTest
{
        public const string SimpleQubitInput =
        "qubit c;\n" +
        "qubit a;\n" +
        "x a;\n" +
        "qif a do\n" +
        "   qubit t;" +
        "   x c;\n" +
        "   h c;\n" +
        "   h t;\n" +
        "end";

    /// <summary>
    /// Beware: with changes to the translation code, the translations could change, while still being correct!
    /// </summary>
    public const string SimpleQubitInputTranslation =
        "OPENQASM 3.0;\n" +
        "include \"stdgates.inc\";\n" +
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id1;\n" +
        "qubit id2;\n" +
        "ctrl(1) @ x id1, id0;\n" +
        "ctrl(1) @ h id1, id0;\n" +
        "ctrl(1) @ h id1, id2;\n" +
        "bit id0_measurement;\n" +
        "measure id0 -> id0_measurement;\n" +
        "bit id1_measurement;\n" +
        "measure id1 -> id1_measurement;\n" +
        "bit id2_measurement;\n" +
        "measure id2 -> id2_measurement;\n";

    
    public const string SimpleRegisterInput =
        @"
        gate h_reg(reg) do
            for i in range(sizeof(reg)) do
                h reg[i];
            end
        end
        "+
        "qubit[4] a;\n" +
        "qubit c;\n" +
        "h_reg a;\n" +
        "qif c do\n" +
        "   qubit[3] t;" +
        "   h_reg t;\n" +
        "end";    

    public const string SimpleRegisterInputTranslation =
        "OPENQASM 3.0;\n" +
        "include \"stdgates.inc\";\n" +
        "qubit id0;\n" +
        "qubit[4] id1;\n" +
        "h id1[0];\n" +
        "h id1[1];\n" +
        "h id1[2];\n" +
        "h id1[3];\n" +
        "qubit id2;\n" +
        "ctrl(1) @ h id1, id2[0];\n" +
        "ctrl(1) @ h id1, id2[1];\n" +
        "ctrl(1) @ h id1, id2[2];\n" +
        "bit id0_measurement;\n" +
        "measure id0 -> id0_measurement;\n" +
        "bit[4] id1_measurement;\n" +
        "measure id1 -> id1_measurement;\n" +
        "bit[3] id2_measurement;\n" +
        "measure id2 -> id2_measurement;\n";

            
    /// <summary>
    /// Tests the code generation for the simple input.
    /// </summary>
    [TestMethod]
    public void SimpleQubitInputTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleQubitInput);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.PrintProgram();
        Assert.IsNotNull(code);

        Assert.AreEqual(code, SimpleQubitInputTranslation);
    }
            
    /// <summary>
    /// Tests the code generation for the simple input.
    /// </summary>
    [TestMethod]
    public void SimpleRegisterInputTranslationTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleRegisterInput);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.PrintProgram();
        Assert.IsNotNull(code);

        Assert.AreEqual(code, SimpleRegisterInputTranslation);
    }
}