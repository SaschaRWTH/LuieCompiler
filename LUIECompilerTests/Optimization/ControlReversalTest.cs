using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Optimization;

namespace LUIECompilerTests.Optimization;

[TestClass]
public class ControlReversalTest
{

    public static readonly string SimpleControlReversal = @"
        qubit a;
        qubit c;
        
        x c;
        h a;
        h c;
        cx a, c;
        h c;
        h a;
        y a;
    ";

    public static readonly string SimpleControlReversalTranslated =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id1;\n" +
        "h id0;\n" +
        "h id1;\n" +
        "ctrl(1) @ x id0, id1;\n" +
        "h id1;\n" +
        "h id0;\n" +
        "y id0;\n";

    public static readonly string SimpleControlReversalOptimized =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id1;\n" +
        "ctrl(1) @ x id1, id0;\n" +
        "y id0;\n";

    public static readonly string MissingFirstHGate = @"
        qubit a;
        qubit c;
        
        h c;
        cx a, c;
        h c;
        h a;
    ";

    public static readonly string MissingFirstHGateTranslated =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "h id1;\n" +
        "ctrl(1) @ x id0, id1;\n" +
        "h id1;\n" +
        "h id0;\n";

    public static readonly string MissingSecondHGate = @"
        qubit a;
        qubit c;
        
        h a;
        cx a, c;
        h a;
        h c;
    ";

    public static readonly string MissingSecondHGateTranslated =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "h id0;\n" +
        "ctrl(1) @ x id0, id1;\n" +
        "h id0;\n" +
        "h id1;\n";

    public static readonly string MissingThirdHGate = @"
        qubit a;
        qubit c;
        
        h a;
        h c;
        cx a, c;
        h a;
    ";

    public static readonly string MissingThirdHGateTranslated =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "h id0;\n" +
        "h id1;\n" +
        "ctrl(1) @ x id0, id1;\n" +
        "h id0;\n";
    public static readonly string MissingFourthHGate = @"
        qubit a;
        qubit c;
        
        h a;
        h c;
        cx a, c;
        h c;
    ";

    public static readonly string MissingFourthHGateTranslated =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "h id0;\n" +
        "h id1;\n" +
        "ctrl(1) @ x id0, id1;\n" +
        "h id1;\n";
    public static readonly string SimpleGuardReversal = @"
        qubit a;
        qubit c;
        
        x c;
        h a;
        h c;
        qif a do
            x c;
        end
        h c;
        h a;
        y a;
    ";

    public static readonly string SimpleGuardReversalTranslated =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id1;\n" +
        "h id0;\n" +
        "h id1;\n" +
        "ctrl(1) @ x id0, id1;\n" +
        "h id1;\n" +
        "h id0;\n" +
        "y id0;\n";

    public static readonly string SimpleGuardReversalOptimized =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id1;\n" +
        "ctrl(1) @ x id1, id0;\n" +
        "y id0;\n";

    public static readonly string MissingFirstHGateGuard = @"
        qubit a;
        qubit c;
        
        h c;
        qif a do
            x c;
        end
        h c;
        h a;
    ";

    public static readonly string MissingFirstHGateGuardTranslated =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "h id1;\n" +
        "ctrl(1) @ x id0, id1;\n" +
        "h id1;\n" +
        "h id0;\n";

    public static readonly string MissingSecondHGateGuard = @"
        qubit a;
        qubit c;
        
        h a;
        qif a do
            x c;
        end
        h a;
        h c;
    ";

    public static readonly string MissingSecondHGateGuardTranslated =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "h id0;\n" +
        "ctrl(1) @ x id0, id1;\n" +
        "h id0;\n" +
        "h id1;\n";

    public static readonly string MissingThirdHGateGuard = @"
        qubit a;
        qubit c;
        
        h a;
        h c;
        qif a do
            x c;
        end
        h a;
    ";

    public static readonly string MissingThirdHGateGuardTranslated =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "h id0;\n" +
        "h id1;\n" +
        "ctrl(1) @ x id0, id1;\n" +
        "h id0;\n";
    public static readonly string MissingFourthHGateGuard = @"
        qubit a;
        qubit c;
        
        h a;
        h c;
        qif a do
            x c;
        end
        h c;
    ";

    public static readonly string MissingFourthHGateGuardTranslated =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "h id0;\n" +
        "h id1;\n" +
        "ctrl(1) @ x id0, id1;\n" +
        "h id1;\n";

    [TestMethod]    
    public void SimpleControlReversalTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleControlReversal);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SimpleControlReversalTranslated, code);

        QASMProgram optimized = program.Optimize(OptimizationType.ControlReversal);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(SimpleControlReversalOptimized, optimizedCode);
    }

    [TestMethod]    
    public void MissingFirstHGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(MissingFirstHGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(MissingFirstHGateTranslated, code);

        QASMProgram optimized = program.Optimize(OptimizationType.ControlReversal);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(MissingFirstHGateTranslated, optimizedCode);
    }
    
    [TestMethod]    
    public void MissingSecondHGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(MissingSecondHGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(MissingSecondHGateTranslated, code);

        QASMProgram optimized = program.Optimize(OptimizationType.ControlReversal);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(MissingSecondHGateTranslated, optimizedCode);
    }

    [TestMethod]    
    public void MissingThirdHGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(MissingThirdHGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(MissingThirdHGateTranslated, code);

        QASMProgram optimized = program.Optimize(OptimizationType.ControlReversal);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(MissingThirdHGateTranslated, optimizedCode);
    }

    [TestMethod]    
    public void MissingFourthHGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(MissingFourthHGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(MissingFourthHGateTranslated, code);

        QASMProgram optimized = program.Optimize(OptimizationType.ControlReversal);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(MissingFourthHGateTranslated, optimizedCode);
    }

    [TestMethod]    
    public void SimpleGuardReversalTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleGuardReversal);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SimpleGuardReversalTranslated, code);

        QASMProgram optimized = program.Optimize(OptimizationType.ControlReversal);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(SimpleGuardReversalOptimized, optimizedCode);
    }

    [TestMethod]    
    public void MissingFirstHGateGuardTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(MissingFirstHGateGuard);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(MissingFirstHGateGuardTranslated, code);

        QASMProgram optimized = program.Optimize(OptimizationType.ControlReversal);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(MissingFirstHGateGuardTranslated, optimizedCode);
    }
    
    [TestMethod]    
    public void MissingSecondHGateGuardTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(MissingSecondHGateGuard);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(MissingSecondHGateGuardTranslated, code);

        QASMProgram optimized = program.Optimize(OptimizationType.ControlReversal);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(MissingSecondHGateGuardTranslated, optimizedCode);
    }

    [TestMethod]    
    public void MissingThirdHGateGuardTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(MissingThirdHGateGuard);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(MissingThirdHGateGuardTranslated, code);

        QASMProgram optimized = program.Optimize(OptimizationType.ControlReversal);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(MissingThirdHGateGuardTranslated, optimizedCode);
    }

    [TestMethod]    
    public void MissingFourthHGateGuardTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(MissingFourthHGateGuard);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(MissingFourthHGateGuardTranslated, code);

        QASMProgram optimized = program.Optimize(OptimizationType.ControlReversal);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(MissingFourthHGateGuardTranslated, optimizedCode);
    }
}