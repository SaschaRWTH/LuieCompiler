using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Optimization;

namespace LUIECompilerTests.Optimization;

[TestClass, TestCategory("Optimization")]
public class NullGateOptimizationTest
{
    public const string SimpleHHNullGate =
    @"
        qubit c;
        qubit a;
        h c;
        h c;
        x a;
    ";
    public const string SimpleHHNullGateTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "h id0;\n" +
        "h id0;\n" +
        "x id1;\n";

    public const string SimpleHHNullGateOptimized =
        "qubit id1;\n" +
        "x id1;\n";

    public const string SimpleXXNullGate =
    @"
        qubit c;
        qubit a;
        x a;
        x c;
        x c;
    ";
    public const string SimpleXXNullGateTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id1;\n" +
        "x id0;\n" +
        "x id0;\n";

    public const string SimpleXXNullGateOptimized =
        "qubit id1;\n" +
        "x id1;\n";

    public const string SimpleZZNullGate =
    @"
        qubit c;
        qubit a;
        x a;
        z c;
        z c;
    ";
    public const string SimpleZZNullGateTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id1;\n" +
        "z id0;\n" +
        "z id0;\n";

    public const string SimpleZZNullGateOptimized =
        "qubit id1;\n" +
        "x id1;\n";

    public const string InterruptedHHNullGate =
    @"
        qubit c;
        qubit a;
        h c;
        x a;
        h c;
    ";
    public const string InterruptedHHNullGateTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "h id0;\n" +
        "x id1;\n" +
        "h id0;\n";

    public const string InterruptedHHNullGateOptimized =
        "qubit id1;\n" +
        "x id1;\n";


    public const string InterruptedXXNullGate =
    @"
        qubit c;
        qubit a;
        x c;
        x a;
        x c;
    ";
    public const string InterruptedXXNullGateTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id0;\n" +
        "x id1;\n" +
        "x id0;\n";

    public const string InterruptedXXNullGateOptimized =
        "qubit id1;\n" +
        "x id1;\n";


    public const string InterruptedZZNullGate =
    @"
        qubit c;
        qubit a;
        z c;
        x a;
        z c;
    ";

    public const string InterruptedZZNullGateTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "z id0;\n" +
        "x id1;\n" +
        "z id0;\n";

    public const string InterruptedZZNullGateOptimized =
        "qubit id1;\n" +
        "x id1;\n";

    public const string ControlledNullGates =
    @"
        qubit a;
        qubit b;
        qubit c;
        qubit d;
        qif a do
            h b;
            x c;
            z d;
        end
        h c;
        qif a do
            z d;
            h b;
            x c;
        end
    ";

    public const string ControlledNullGatesTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "qubit id2;\n" +
        "qubit id3;\n" +
        "ctrl(1) @ h id0, id1;\n" +
        "ctrl(1) @ x id0, id2;\n" +
        "ctrl(1) @ z id0, id3;\n" +
        "h id2;\n" +
        "ctrl(1) @ z id0, id3;\n" +
        "ctrl(1) @ h id0, id1;\n" +
        "ctrl(1) @ x id0, id2;\n";

    public const string ControlledNullGatesOptimized =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "qubit id2;\n" +
        "ctrl(1) @ h id0, id1;\n" +
        "ctrl(1) @ x id0, id2;\n" +
        "ctrl(1) @ h id0, id1;\n" + 
        "h id2;\n" +
        "ctrl(1) @ x id0, id2;\n";



    public const string NullGateImpossible =
    @"
        qubit a;
        qubit b;
        qif b do
            h a;
        end
        x b;
        qif b do
            h a;
        end
    ";
    public const string NullGateImpossibleTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "ctrl(1) @ h id1, id0;\n" +
        "x id1;\n" +
        "ctrl(1) @ h id1, id0;\n";

    public const string NullGateWithNullGateInbetween =
    @"
        qubit a;
        h a;
        x a;
        x a;
        h a;
        y a;
    ";

    public const string NullGateWithNullGateInbetweenTranslation =
        "qubit id0;\n" +
        "h id0;\n" +
        "x id0;\n" +
        "x id0;\n" +
        "h id0;\n" +
        "y id0;\n";
    
    public const string NullGateWithNullGateInbetweenOptimized =
        "qubit id0;\n" +
        "y id0;\n";

    [TestMethod]
    public void SimpleHHNullGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleHHNullGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SimpleHHNullGateTranslation, code);

        QASMProgram optimized = program.Optimize(OptimizationType.NullGate);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(SimpleHHNullGateOptimized, optimizedCode);

    }

    [TestMethod]
    public void SimpleXXNullGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleXXNullGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SimpleXXNullGateTranslation, code);

        QASMProgram optimized = program.Optimize(OptimizationType.NullGate);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(SimpleXXNullGateOptimized, optimizedCode);

    }

    [TestMethod]
    public void SimpleZZNullGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleZZNullGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SimpleZZNullGateTranslation, code);

        QASMProgram optimized = program.Optimize(OptimizationType.NullGate);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(SimpleZZNullGateOptimized, optimizedCode);

    }

    [TestMethod]
    public void InterruptedHHNullGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InterruptedHHNullGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(InterruptedHHNullGateTranslation, code);

        QASMProgram optimized = program.Optimize(OptimizationType.NullGate);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(InterruptedHHNullGateOptimized, optimizedCode);

    }

    [TestMethod]
    public void InterruptedXXNullGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InterruptedXXNullGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(InterruptedXXNullGateTranslation, code);

        QASMProgram optimized = program.Optimize(OptimizationType.NullGate);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(InterruptedXXNullGateOptimized, optimizedCode);

    }

    [TestMethod]
    public void InterruptedZZNullGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InterruptedZZNullGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(InterruptedZZNullGateTranslation, code);

        QASMProgram optimized = program.Optimize(OptimizationType.NullGate);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(InterruptedZZNullGateOptimized, optimizedCode);

    }
    
    [TestMethod]
    public void ControlledNullGatesTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(ControlledNullGates);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(ControlledNullGatesTranslation, code);

        QASMProgram optimized = program.Optimize(OptimizationType.NullGate);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(ControlledNullGatesOptimized, optimizedCode);

    }

    [TestMethod]
    public void NullGateImpossibleTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(NullGateImpossible);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(NullGateImpossibleTranslation, code);

        QASMProgram optimized = program.Optimize(OptimizationType.NullGate);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(NullGateImpossibleTranslation, optimizedCode);
    }

    
    [TestMethod]
    public void NullGateWithNullGateInbetweenTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(NullGateWithNullGateInbetween);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(NullGateWithNullGateInbetweenTranslation, code);

        QASMProgram optimized = program.Optimize(OptimizationType.NullGate);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(NullGateWithNullGateInbetweenOptimized, optimizedCode);

    }


}