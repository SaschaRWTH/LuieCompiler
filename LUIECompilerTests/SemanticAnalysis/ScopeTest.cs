using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;
using LUIECompiler.SemanticAnalysis;

namespace LUIECompilerTests.SemanticAnalysis;

[TestClass, TestCategory("SemanticAnalysis"), TestCategory("Scope")]
public class ScopeTest
{

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
        Assert.IsTrue(error.Errors.Any(e => e is RedefineError && e.ErrorContext.Line == 6));
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
        Qubit firstA = new("A", new ErrorContext());
        table.AddSymbol(firstA);

        table.PushScope();
        Qubit secondA = new("A", new ErrorContext());
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