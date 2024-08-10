using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Codes;

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

    public const string SimpleHHNullGateOptimizated =
        "qubit id0;\n" +
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

    public const string SimpleXXNullGateOptimizated =
        "qubit id0;\n" +
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

    public const string SimpleZZNullGateOptimizated =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id1;\n";


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

        QASMProgram optimized = program.Optimize();

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);
        
        Assert.AreEqual(SimpleHHNullGateOptimizated, optimizedCode);

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

        QASMProgram optimized = program.Optimize();

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);
        
        Assert.AreEqual(SimpleXXNullGateOptimizated, optimizedCode);

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

        QASMProgram optimized = program.Optimize();

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);
        
        Assert.AreEqual(SimpleZZNullGateOptimizated, optimizedCode);

    }


}