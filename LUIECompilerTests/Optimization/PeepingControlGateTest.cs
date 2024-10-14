using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Optimization;

namespace LUIECompilerTests.Optimization;


[TestClass, TestCategory("Optimization")]
public class PeepingControlTest
{
    
    public const string SimpleFalseGate =
    @"
        qubit a;
        qubit c;
        qif c do 
            h a;
        end
        x c;
        h a;
    ";
  
    public const string SimpleFalseGateTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "ctrl(1) @ h id1, id0;\n" +
        "x id1;\n" +
        "h id0;\n";

    public const string SimpleFalseGateOptimized =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "h id0;\n" +
        "x id1;\n";

    
    public const string SimpleTrueGate =
    @"
        qubit a;
        qubit c;
        x c;
        qif c do 
            h a;
        end
        x c;
        h a;
    ";

    public const string SimpleTrueGateTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id1;\n" +
        "ctrl(1) @ h id1, id0;\n" +
        "x id1;\n" +
        "h id0;\n";

    public const string SimpleTrueGateOptimized =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "h id0;\n" +
        "h id0;\n" +
        "x id1;\n" +
        "x id1;\n";

    
    
    [TestMethod]
    public void SimpleFalseGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleFalseGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SimpleFalseGateTranslation, code);

        QASMProgram optimized = program.Optimize(OptimizationType.PeepingControl);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(SimpleFalseGateOptimized, optimizedCode);

    } 
    
    [TestMethod]
    public void SimpleTrueGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleTrueGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SimpleTrueGateTranslation, code);

        QASMProgram optimized = program.Optimize(OptimizationType.PeepingControl);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(SimpleTrueGateOptimized, optimizedCode);

    }
}