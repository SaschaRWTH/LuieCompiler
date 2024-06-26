using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;
using LUIECompiler.SemanticAnalysis;

namespace LUIECompilerTests;

[TestClass]
public class SemanticAnalysisTest
{
    public const string InputSimple =
        "qubit c;\n" +
        "qubit b;\n" +
        "x a;\n" +
        "qubit c;\n" +
        "qif a do\n" +
        "skip;\n" +
        "end";

    public const string InputScopeCorrect =
        "qubit a;\n" +
        "qif a do\n" +
        "qubit a;\n" +
        "qif a do\n" +
        "qubit a;\n" +
        "end\n" +
        "end";

    public const string InputScopeIncorrect =
        "qubit a;\n" +
        "qif a do\n" +
        "qubit a;\n" +
        "qif a do\n" +
        "qubit a;\n" +
        "qubit a;\n" +
        "end\n" +
        "end";


    public const string InputScopeUseOfOuterScope =
        "qubit a;\n" +
        "qif a do\n" +
        "qubit b;\n" +
        "qif a do\n" +
        "qubit a;\n" +
        "end\n" +
        "end";

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

        Assert.IsTrue(error.Errors.Any(e => e is RedefineError && e.Line == 4));
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

        Assert.IsTrue(error.Errors.Any(e => e is UndefinedError && e.Line == 3));
        Assert.IsTrue(error.Errors.Any(e => e is UndefinedError && e.Line == 5));
    }

    /// <summary>
    /// Test that in a simple, correct program with scopes there are no errors reported. 
    /// </summary>
    [TestMethod]
    public void ScopeCorrectTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputScopeCorrect);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(!error.ContainsCriticalError);
    }

    /// <summary>
    /// Test that redefinitions in correctly reported in scopes.
    /// </summary>
    [TestMethod]
    public void ScopeIncorrectTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputScopeIncorrect);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);
        Assert.IsTrue(error.Errors.Any(e => e is RedefineError && e.Line == 6));
    }

    /// <summary>
    /// Tests that there are no errors reported when using an identifier defined in an outer scope.
    /// </summary>
    [TestMethod]
    public void ScopeUseOfOuterScopeTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputScopeUseOfOuterScope);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(!error.ContainsCriticalError);
    }

    /// <summary>
    /// Tests whether the correct symbol is returned when using the same identifier in different scopes.
    /// </summary>
    [TestMethod]
    public void CorrectInfoTest()
    {
        SymbolTable table = new();
        
        table.PushScope();
        Qubit firstA = new("A");
        table.AddSymbol(firstA);

        table.PushScope();
        Qubit secondA = new("A");
        table.AddSymbol(secondA);

        Qubit? secondScopeA = table.GetSymbolInfo("A") as Qubit;
        Assert.IsNotNull(secondScopeA);
        Assert.AreNotEqual(firstA, secondScopeA);
        Assert.AreEqual(secondA, secondScopeA);

        table.PopScope();
        Qubit? firstScopeA = table.GetSymbolInfo("A") as Qubit;
        Assert.IsNotNull(secondScopeA);
        Assert.AreNotEqual(secondA, firstScopeA);
        Assert.AreEqual(firstA, firstScopeA);
    }
}