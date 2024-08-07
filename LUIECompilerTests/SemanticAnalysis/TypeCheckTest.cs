using LUIECompiler.Common.Errors;
using LUIECompiler.SemanticAnalysis;

namespace LUIECompilerTests.SemanticAnalysis;

[TestClass, TestCategory("SemanticAnalysis"), TestCategory("TypeCheck")]
public class TypeCheckTest
{
    public const string InputCorrect =
        "qubit[3] c;\n" +
        "qubit b;\n" +
        "x b;\n" +
        "qif c[1] do\n" +
        "qubit b;\n" +
        "qubit c;" +
        "qubit t;" +
        "qif c do\n" +
        "x c;\n" +
        "end\n" +
        "h c;\n" +
        "h t;\n" +
        "h b;\n" +
        "end";

    public const string InputIncorrect =
        "qubit[3] c;\n" +
        "qubit b;\n" +
        "x b;\n" +
        "qif c do\n" +
        "qubit b;\n" +
        "qubit[2] t;\n" +
        "qif t do\n" +
        "x i;\n" +
        "end\n" +
        "h c;\n" +
        "h t;\n" +
        "h b;\n" +
        "end";

    public const string InvalidArguments =
        "qubit a;\n" +
        "qubit b;\n" +
        "x a;\n" +
        "x a, b;\n" +
        "cx a;\n" +
        "cx a, b;";

    public const string TypeErrorsArguments = 
        "qubit[3] a;\n" +
        "qubit[2] b;\n" +
        "x a;\n" +
        "x a[2];\n" +
        "cx a[2], b[1];\n" +
        "cx a[1], b;";

    public const string FunctionParameterValidType = 
        "qubit[3] a;\n" +
        "qubit[sizeof(a)] b;";
        
    public const string FunctionParameterInvalidType = 
        "qubit[3] a;\n" +
        "for i in 1..5 do\n" +
        "    qubit[sizeof(i)] b;\n" +
        "end";

    public const string ParameterizedGateValid = 
        "qubit a;\n" +
        "p(1/8) a;";

    /// <summary>
    /// Test that redefinitions in correctly reported in scopes.
    /// </summary>
    [TestMethod]
    public void CorrectCodeTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputCorrect);
        var analysis = new TypeCheckListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsFalse(error.ContainsCriticalError);
    }

    /// <summary>
    /// Tests that there are no errors reported when using an identifier defined in an outer scope.
    /// </summary>
    [TestMethod]
    public void IncorrectCodeTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputIncorrect);
        var analysis = new TypeCheckListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);
        Console.WriteLine(error.ToString());
        Assert.IsTrue(error.Errors.Any(e => e is UndefinedError && e.ErrorContext.Line == 8));
        Assert.IsTrue(error.Errors.Any(e => e is TypeError && e.ErrorContext.Line == 4));
        Assert.IsTrue(error.Errors.Any(e => e is TypeError && e.ErrorContext.Line == 7));
    }

    
    /// <summary>
    /// Tests that there are errors reported when using invalid arguments.
    /// </summary>
    [TestMethod]
    public void InvalidArgumentsTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InvalidArguments);
        var analysis = new TypeCheckListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);
        Assert.AreEqual(2, error.Errors.Count);
        Assert.IsTrue(error.Errors.Any(e => e is InvalidArguments && e.ErrorContext.Line == 4));
        Assert.IsTrue(error.Errors.Any(e => e is InvalidArguments && e.ErrorContext.Line == 5));
    }
    
    /// <summary>
    /// Tests that there are errors reported when giving wrong type of arguments.
    /// </summary>
    [TestMethod]
    public void TypeErrorsArgumentsTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(TypeErrorsArguments);
        var analysis = new TypeCheckListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);
        Assert.AreEqual(2, error.Errors.Count);
        Assert.IsTrue(error.Errors.Any(e => e is TypeError && e.ErrorContext.Line == 3));
        Assert.IsTrue(error.Errors.Any(e => e is TypeError && e.ErrorContext.Line == 6));
    }   

    /// <summary>
    /// Tests that there are errors reported when giving wrong type of arguments.
    /// </summary>
    [TestMethod]
    public void FunctionParameterValidTypeTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(FunctionParameterValidType);
        var analysis = new TypeCheckListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(!error.ContainsCriticalError);
    }

    /// <summary>
    /// Tests that there are errors reported when giving wrong type of arguments.
    /// </summary>
    [TestMethod]
    public void FunctionParameterInvalidTypeTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(FunctionParameterInvalidType);
        var analysis = new TypeCheckListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);
        Assert.AreEqual(1, error.Errors.Count);
        Assert.IsTrue(error.Errors.Any(e => e is TypeError && e.ErrorContext.Line == 3));
    }

    /// <summary>
    /// Tests that there no errors reported when using a parameterized gate.
    /// </summary>
    [TestMethod]
    public void CheckParameterizedGateValid()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(ParameterizedGateValid);
        var analysis = new TypeCheckListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsFalse(error.ContainsCriticalError);
        Assert.IsTrue(error.Errors.Count == 0);
    }
}
