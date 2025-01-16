namespace LUIECompilerTests.Optimization;

using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Optimization;

[TestClass, TestCategory("Optimization")]
public class HSandwichTest
{
    public static readonly string SimpleHZHSandwich = @"
        qubit a;
        
        h a;
        z a;
        h a;
        y a;
    ";

    public static readonly string SimpleHZHSandwichTranslated =
        "qubit id0;\n" +
        "h id0;\n" +
        "z id0;\n" +
        "h id0;\n" +
        "y id0;\n";

    public static readonly string SimpleHZHSandwichOptimized =
        "qubit id0;\n" +
        "x id0;\n" +
        "y id0;\n";

    public static readonly string SimpleHXHSandwich = @"
        qubit a;
        
        h a;
        x a;
        h a;
        y a;
    ";

    public static readonly string SimpleHXHSandwichTranslated =
        "qubit id0;\n" +
        "h id0;\n" +
        "x id0;\n" +
        "h id0;\n" +
        "y id0;\n";

    public static readonly string SimpleHXHSandwichOptimized =
        "qubit id0;\n" +
        "z id0;\n" +
        "y id0;\n";

    public static readonly string SandwichedOnControlWire =@"
        qubit c;
        qubit a;
        
        qif a do
            h c;
        end

        qif c do
            z a;
        end
        
        qif a do
            h c;
        end
    ";

    
    public static readonly string SandwichedOnControlWireTranslated =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "ctrl(1) @ h id1, id0;\n" +
        "ctrl(1) @ z id0, id1;\n" +
        "ctrl(1) @ h id1, id0;\n";
    
    public static readonly string SandwichedOnControlWireOptimized =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "ctrl(1) @ h id1, id0;\n" +
        "ctrl(1) @ z id0, id1;\n" +
        "ctrl(1) @ h id1, id0;\n";

    [TestMethod]
    public void SimpleHZHSandwichTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleHZHSandwich);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SimpleHZHSandwichTranslated, code);

        QASMProgram optimized = program.Optimize(OptimizationType.HSandwichReduction);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(SimpleHZHSandwichOptimized, optimizedCode);

    }

    [TestMethod]
    public void SimpleHXHSandwichTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleHXHSandwich);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SimpleHXHSandwichTranslated, code);

        QASMProgram optimized = program.Optimize(OptimizationType.HSandwichReduction);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(SimpleHXHSandwichOptimized, optimizedCode);

    }

    [TestMethod]
    public void SandwichedOnControlWireTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SandwichedOnControlWire);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        string code = program.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SandwichedOnControlWireTranslated, code);

        QASMProgram optimized = program.Optimize(OptimizationType.HSandwichReduction);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(SandwichedOnControlWireOptimized, optimizedCode);

    }


}