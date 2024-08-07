using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;
using LUIECompiler.SemanticAnalysis;

namespace LUIECompilerTests.SemanticAnalysis;

[TestClass, TestCategory("SemanticAnalysis"), TestCategory("Definitions")]
public class DefinitionTest
{
    public const string InputSimple =
        "qubit c;\n" +
        "qubit b;\n" +
        "x a;\n" +
        "qubit c;\n" +
        "qif a do\n" +
        "skip;\n" +
        "end";

    public const string DefineLaterInput = 
        "h c;\n" +
        "qubit c;";

    public const string UndefinedExpressionInput = 
        "qubit[5] c;\n" +
        "h c[i];\n" +
        "h c[2];\n" +
        "h c[j];";

    public const string UndefinedFunctionParameterInput =
        "qubit[5] c;\n" +
        "qubit[sizeof(i)] d;";

    /// <summary>
    /// Test that already defined identifiers are correctly reported.
    /// </summary>
    [TestMethod]
    public void RedefineErrorTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputSimple);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);

        Assert.IsTrue(error.Errors.Any(e => e is RedefineError && e.ErrorContext.Line == 4));
    }

    /// <summary>
    /// Test that already defined identifiers are correctly reported.
    /// </summary>
    [TestMethod]
    public void DefineLaterTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(DefineLaterInput);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);

        Assert.IsTrue(error.Errors.Any(e => e is UndefinedError && e.ErrorContext.Line == 1));
    }

    /// <summary>
    /// Tests that undefined identifiers are correctly reported.
    /// </summary>
    [TestMethod]
    public void UndefinedErrorTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputSimple);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);

        Assert.IsTrue(error.Errors.Any(e => e is UndefinedError && e.ErrorContext.Line == 3));
        Assert.IsTrue(error.Errors.Any(e => e is UndefinedError && e.ErrorContext.Line == 5));
    }

    /// <summary>
    /// Tests that undefined identifiers are correctly reported.
    /// </summary>
    [TestMethod]
    public void UndefinedExpressionTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(UndefinedExpressionInput);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);

        Assert.IsTrue(error.Errors.Any(e => e is UndefinedError && e.ErrorContext.Line == 2));
        Assert.IsTrue(error.Errors.Any(e => e is UndefinedError && e.ErrorContext.Line == 4));
    }
    
    /// <summary>
    /// Tests that undefined identifiers are correctly reported.
    /// </summary>
    [TestMethod]
    public void UndefinedFunctionParameterTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(UndefinedFunctionParameterInput);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);

        Assert.IsTrue(error.Errors.Any(e => e is UndefinedError && e.ErrorContext.Line == 2));
    }
}