
using LUIECompiler.CodeGeneration;

namespace LUIECompilerTests.CodeGeneration;


[TestClass, TestCategory("CodeGeneration"), TestCategory("Constants")]
public class ConstantsTest
{
    public const string SimpleIntConstant =
    @"
        const a : int = 5;
        qubit[6] b;

        x b[a];
    ";

    public const string SimpleIntConstantTranslation =
        "qubit[6] id0;\n" +
        "x id0[5];\n";

    public const string IntConstantInExpression =
    @"
        const n : int = 5;
        qubit[n] a;

        for i in range(n) do
            x a[i];
        end
    ";

    public const string IntConstantInExpressionTranslation =
        "qubit[5] id0;\n" +
        "x id0[0];\n" +
        "x id0[1];\n" +
        "x id0[2];\n" +
        "x id0[3];\n" +
        "x id0[4];\n";
    public const string SimpleUIntConstant =
    @"
        const a : uint = 5;
        qubit[6] b;

        x b[a];
    ";

    public const string SimpleUIntConstantTranslation =
        "qubit[6] id0;\n" +
        "x id0[5];\n";

    public const string UIntConstantInExpression =
    @"
        const n : uint = 5;
        qubit[n] a;

        for i in range(n) do
            x a[i];
        end
    ";
    public const string UIntConstantInExpressionWithFunc =
    @"
        const n : uint = 5;
        const m : uint = 6;
        qubit[n] a;

        for i in range(min(n,m)) do
            x a[i];
        end
    ";

    public const string UIntConstantInExpressionTranslation =
        "qubit[5] id0;\n" +
        "x id0[0];\n" +
        "x id0[1];\n" +
        "x id0[2];\n" +
        "x id0[3];\n" +
        "x id0[4];\n";
    public const string SimpleDoubleConstant =
    @"
        const a : double = 5;
        qubit[6] b;

        x b[a];
    ";

    public const string SimpleDoubleConstantTranslation =
        "qubit[6] id0;\n" +
        "x id0[5];\n";

    public const string DoubleConstantInExpression =
    @"
        const n : double = 5;
        qubit[n] a;

        for i in range(n) do
            x a[i];
        end
    ";

    public const string DoubleConstantInExpressionTranslation =
        "qubit[5] id0;\n" +
        "x id0[0];\n" +
        "x id0[1];\n" +
        "x id0[2];\n" +
        "x id0[3];\n" +
        "x id0[4];\n";

    public const string ConstAsSizeInCompGate =
    @"
        gate h_all(reg) do
            for i in range(sizeof(reg)) do
                x reg[i];
            end
        end

        const n : double = 5;
        qubit[n] a;
        h_all a;
    ";
    public const string ConstAsSizeInCompGateTranslation =
        "qubit[5] id0;\n" +
        "x id0[0];\n" +
        "x id0[1];\n" +
        "x id0[2];\n" +
        "x id0[3];\n" +
        "x id0[4];\n";

    public const string ConstantsDeclarationNotOnTopLevel =
    @"  qubit a;
        qubit b;
        qubit c;

        qif c do 
            const n : int = 2;
            for _ in range(n) do
                x a;
            end
        end

        qif c do 
            const n : int = 3;
            for _ in range(n) do
                x b;
            end
        end
    ";

    public const string ConstantsDeclarationNotOnTopLevelTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "qubit id2;\n" +
        "ctrl(1) @ x id2, id0;\n" +
        "ctrl(1) @ x id2, id0;\n" +
        "ctrl(1) @ x id2, id1;\n" +
        "ctrl(1) @ x id2, id1;\n" +
        "ctrl(1) @ x id2, id1;\n";

    public const string ConstantsDeclarationInCompGate =
    @"  gate test(a) do
            const n : int = 10 / 2;
            for _ in range(n) do
                x a;
            end
        end

        qubit a;
        test a;
    ";
    public const string ConstantsDeclarationInCompGateTranslation =
        "qubit id0;\n" +
        "x id0;\n" +
        "x id0;\n" +
        "x id0;\n" +
        "x id0;\n" +
        "x id0;\n";

    [TestMethod]
    public void SimpleIntConstantTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleIntConstant);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SimpleIntConstantTranslation, code);
    }
    [TestMethod]
    public void ConstantIntExpressionTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(IntConstantInExpression);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(IntConstantInExpressionTranslation, code);
    }
    [TestMethod]
    public void SimpleUIntConstantTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleUIntConstant);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SimpleUIntConstantTranslation, code);
    }

    [TestMethod]
    public void ConstantUIntExpressionTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(UIntConstantInExpression);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(UIntConstantInExpressionTranslation, code);
    }

    [TestMethod]
    public void UIntConstantInExpressionWithFuncTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(UIntConstantInExpressionWithFunc);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(UIntConstantInExpressionTranslation, code);
    }
    [TestMethod]
    public void SimpleDoubleConstantTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleDoubleConstant);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SimpleDoubleConstantTranslation, code);
    }
    [TestMethod]
    public void ConstantDoubleExpressionTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(DoubleConstantInExpression);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(DoubleConstantInExpressionTranslation, code);
    }
    [TestMethod]
    public void ConstAsSizeInCompGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(ConstAsSizeInCompGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(ConstAsSizeInCompGateTranslation, code);
    }
    [TestMethod]
    public void ConstantsDeclarationNotOnTopLevelTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(ConstantsDeclarationNotOnTopLevel);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(ConstantsDeclarationNotOnTopLevelTranslation, code);
    }
    [TestMethod]
    public void ConstantsDeclarationInCompGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(ConstantsDeclarationInCompGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(ConstantsDeclarationInCompGateTranslation, code);
    }
}