using LUIECompiler.CodeGeneration;
using LUIECompiler.Common.Extensions;

namespace LUIECompilerTests.CodeGeneration;


[TestClass, TestCategory("CodeGeneration")]
public class ExpressionTest
{
    public const string BasicAddition = "10 + 12";
    public const int BasicAdditionResult = 22;

    public const string BasicSubtraction = "85 - 20";
    public const int BasicSubtractionResult = 65;

    public const string BasicMultiplication = "15 * 2";
    public const int BasicMultiplicationResult = 30;

    public const string BasicDivision = "21 / 2";
    public const int BasicDivisionResult = 10;

    public const string OrderOfOperations = "10 + 2 * 3";
    public const int OrderOfOperationsResult = 16;
    public const string ParanthesesExpression = "2 * (3+4)";
    public const int ParanthesesExpressionResult = 14;

    public const string AdditiveInverseExpression = "3 + -2";
    public const int AdditiveInverseExpressionResult = 1;

    public const string ComplexExample = "4 + 360 / ((2 + 4) + 3 * 2)";
    public const int ComplexExampleResult = 34;

    public const string SizeOfFunctionExpressionInput =
        "qubit[3] a;\n" +
        "qubit[sizeof(a)] b;\n" +
        "qubit[sizeof(b) + 1] c;\n" +
        "qubit[sizeof(c) + 1] d;";
    public const string SizeOfFunctionExpressionTranslation =
        "qubit[3] id0;\n" +
        "qubit[3] id1;\n" +
        "qubit[4] id2;\n" +
        "qubit[5] id3;\n";

    public const string SizeOfAccessInput =
        "qubit[5] a;\n" +
        "for i in 1..5 do\n" +
        "    x a[sizeof(a) - i];\n" +
        "end";
    public const string SizeOfAccessTranslation =
        "qubit[5] id0;\n" +
        "x id0[4];\n" +
        "x id0[3];\n" +
        "x id0[2];\n" +
        "x id0[1];\n" +
        "x id0[0];\n";

    public class ExpressionListener : LuieBaseListener
    {
        public int? Result { get; private set; } = null;

        /// <summary>
        /// The function is called any time an expression is exited, this can be multiple times.
        /// However, because we set result when the expression is exited, we should get the result of the top most expression.
        /// </summary>
        /// <param name="context"></param>
        public override void ExitExpression(LuieParser.ExpressionContext context)
        {
            Result = context.GetExpression<int>().Evaluate(new(), new()
            {
                Parent = null,
            });
        }
    }
    
    /// <summary>
    /// Tests addition expression with a basic example.
    /// </summary>
    [TestMethod]
    public void BasicAdditionTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(BasicAddition);

        var codegen = new ExpressionListener();
        walker.Walk(codegen, parser.expression());

        int? result = codegen.Result;
        Assert.IsNotNull(result);

        Assert.AreEqual(BasicAdditionResult, result);
    }    

    /// <summary>
    /// Tests subtraction expression with a basic example.
    /// </summary>
    [TestMethod]
    public void BasicSubtractionTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(BasicSubtraction);

        var codegen = new ExpressionListener();
        walker.Walk(codegen, parser.expression());

        int? result = codegen.Result;
        Assert.IsNotNull(result);

        Assert.AreEqual(BasicSubtractionResult, result);
    }

    /// <summary>
    /// Tests multiplication expression with a basic example.
    /// </summary>
    [TestMethod]
    public void BasicMultiplicationTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(BasicMultiplication);

        var codegen = new ExpressionListener();
        walker.Walk(codegen, parser.expression());

        int? result = codegen.Result;
        Assert.IsNotNull(result);

        Assert.AreEqual(BasicMultiplicationResult, result);
    }

    /// <summary>
    /// Tests division expression with a basic example.
    /// </summary>
    [TestMethod]
    public void BasicDivisionTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(BasicDivision);

        var codegen = new ExpressionListener();
        walker.Walk(codegen, parser.expression());

        int? result = codegen.Result;
        Assert.IsNotNull(result);

        Assert.AreEqual(BasicDivisionResult, result);
    }

    /// <summary>
    /// Tests order of operations with a basic example.
    /// </summary>
    [TestMethod]
    public void OrderOfOperationsTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(OrderOfOperations);

        var codegen = new ExpressionListener();
        walker.Walk(codegen, parser.expression());

        int? result = codegen.Result;
        Assert.IsNotNull(result);

        Assert.AreEqual(OrderOfOperationsResult, result);
    }
    /// <summary>
    /// Tests order of operations with a basic example.
    /// </summary>
    [TestMethod]
    public void ParanthesesExpressionTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(ParanthesesExpression);

        var codegen = new ExpressionListener();
        walker.Walk(codegen, parser.expression());

        int? result = codegen.Result;
        Assert.IsNotNull(result);

        Assert.AreEqual(ParanthesesExpressionResult, result);
    }
    /// <summary>
    /// Tests order of operations with a basic example.
    /// </summary>
    [TestMethod]
    public void AdditiveInverseExpressionTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(AdditiveInverseExpression);

        var codegen = new ExpressionListener();
        walker.Walk(codegen, parser.expression());

        int? result = codegen.Result;
        Assert.IsNotNull(result);

        Assert.AreEqual(AdditiveInverseExpressionResult, result);
    }

    /// <summary>
    /// Tests order of operations with a basic example.
    /// </summary>
    [TestMethod]
    public void ComplexExampleTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(ComplexExample);

        var codegen = new ExpressionListener();
        walker.Walk(codegen, parser.expression());

        int? result = codegen.Result;
        Assert.IsNotNull(result);

        Assert.AreEqual(ComplexExampleResult, result);
    }

    /// <summary>
    /// Tests the size of function expression.
    /// </summary>
    [TestMethod]
    public void SizeOfFunctionExpressionTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SizeOfFunctionExpressionInput);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? result = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(result);

        Assert.AreEqual(SizeOfFunctionExpressionTranslation, result);
    }

    /// <summary>
    /// Tests the size of access expression.
    /// </summary>
    [TestMethod]
    public void SizeOfAccessTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SizeOfAccessInput);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? result = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(result);

        Assert.AreEqual(SizeOfAccessTranslation, result);
    }
}