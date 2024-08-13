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

    public const string InterruptedHHNullGateOptimizated =
        "qubit id0;\n" +
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

    public const string InterruptedXXNullGateOptimizated =
        "qubit id0;\n" +
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

    public const string InterruptedZZNullGateOptimizated =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id1;\n";


    [TestMethod]
    public void SimpleHHNullGateTest()
    {
        // var walker = Utils.GetWalker();
        // var parser = Utils.GetParser(SimpleHHNullGate);

        // var codegen = new CodeGenerationListener();
        // walker.Walk(codegen, parser.parse());

        // QASMProgram program = codegen.CodeGen.GenerateCode();
        // Assert.IsNotNull(program);

        // string code = program.ToString();
        // Assert.IsNotNull(code);

        // Assert.AreEqual(SimpleHHNullGateTranslation, code);

        // QASMProgram optimized = program.Optimize();

        // string optimizedCode = optimized.ToString();
        // Assert.IsNotNull(optimizedCode);

        // Assert.AreEqual(SimpleHHNullGateOptimizated, optimizedCode);

    }

    [TestMethod]
    public void SimpleXXNullGateTest()
    {
        // var walker = Utils.GetWalker();
        // var parser = Utils.GetParser(SimpleXXNullGate);

        // var codegen = new CodeGenerationListener();
        // walker.Walk(codegen, parser.parse());

        // QASMProgram program = codegen.CodeGen.GenerateCode();
        // Assert.IsNotNull(program);

        // string code = program.ToString();
        // Assert.IsNotNull(code);

        // Assert.AreEqual(SimpleXXNullGateTranslation, code);

        // QASMProgram optimized = program.Optimize();

        // string optimizedCode = optimized.ToString();
        // Assert.IsNotNull(optimizedCode);

        // Assert.AreEqual(SimpleXXNullGateOptimizated, optimizedCode);

    }

    [TestMethod]
    public void SimpleZZNullGateTest()
    {
        // var walker = Utils.GetWalker();
        // var parser = Utils.GetParser(SimpleZZNullGate);

        // var codegen = new CodeGenerationListener();
        // walker.Walk(codegen, parser.parse());

        // QASMProgram program = codegen.CodeGen.GenerateCode();
        // Assert.IsNotNull(program);

        // string code = program.ToString();
        // Assert.IsNotNull(code);

        // Assert.AreEqual(SimpleZZNullGateTranslation, code);

        // QASMProgram optimized = program.Optimize();

        // string optimizedCode = optimized.ToString();
        // Assert.IsNotNull(optimizedCode);

        // Assert.AreEqual(SimpleZZNullGateOptimizated, optimizedCode);

    }

    [TestMethod]
    public void InterruptedHHNullGateTest()
    {
        // var walker = Utils.GetWalker();
        // var parser = Utils.GetParser(InterruptedHHNullGate);

        // var codegen = new CodeGenerationListener();
        // walker.Walk(codegen, parser.parse());

        // QASMProgram program = codegen.CodeGen.GenerateCode();
        // Assert.IsNotNull(program);

        // string code = program.ToString();
        // Assert.IsNotNull(code);

        // Assert.AreEqual(InterruptedHHNullGateTranslation, code);

        // QASMProgram optimized = program.Optimize();

        // string optimizedCode = optimized.ToString();
        // Assert.IsNotNull(optimizedCode);

        // Assert.AreEqual(InterruptedHHNullGateOptimizated, optimizedCode);

    }

    [TestMethod]
    public void InterruptedXXNullGateTest()
    {
        // var walker = Utils.GetWalker();
        // var parser = Utils.GetParser(InterruptedXXNullGate);

        // var codegen = new CodeGenerationListener();
        // walker.Walk(codegen, parser.parse());

        // QASMProgram program = codegen.CodeGen.GenerateCode();
        // Assert.IsNotNull(program);

        // string code = program.ToString();
        // Assert.IsNotNull(code);

        // Assert.AreEqual(InterruptedXXNullGateTranslation, code);

        // QASMProgram optimized = program.Optimize();

        // string optimizedCode = optimized.ToString();
        // Assert.IsNotNull(optimizedCode);

        // Assert.AreEqual(InterruptedXXNullGateOptimizated, optimizedCode);

    }

    [TestMethod]
    public void InterruptedZZNullGateTest()
    {
        // var walker = Utils.GetWalker();
        // var parser = Utils.GetParser(InterruptedZZNullGate);

        // var codegen = new CodeGenerationListener();
        // walker.Walk(codegen, parser.parse());

        // QASMProgram program = codegen.CodeGen.GenerateCode();
        // Assert.IsNotNull(program);

        // string code = program.ToString();
        // Assert.IsNotNull(code);

        // Assert.AreEqual(InterruptedZZNullGateTranslation, code);

        // QASMProgram optimized = program.Optimize();

        // string optimizedCode = optimized.ToString();
        // Assert.IsNotNull(optimizedCode);

        // Assert.AreEqual(InterruptedZZNullGateOptimizated, optimizedCode);

    }


}