using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using LUIECompiler.CodeGeneration;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;
using LUIECompiler.SemanticAnalysis;

namespace LUIECompilerTests.SemanticAnalysis;

[TestClass, TestCategory("SemanticAnalysis"), TestCategory("Scope")]
public class ScopeTest
{

    public const string RedefineError =
        "qubit a;\n" +
        "qif a do\n" +
        "   qubit a;\n" +
        "   qif a do\n" +
        "       qubit a;\n" +
        "   end\n" +
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


    public const string RedefineInInnerScope =
        "qubit a;\n" +
        "qubit b;\n" +
        "qif a do\n" +
        "    qubit a;\n" +
        "    qif b do\n" +
        "        qubit a;\n" +
        "        qubit b;\n" +
        "    end\n" +
        "end";


    /// <summary>
    /// Test that in a simple, correct program with scopes there are no errors reported. 
    /// </summary>
    [TestMethod]
    public void ScopeCorrectTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(RedefineError);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);
        Assert.AreEqual(3, error.Errors.Count);

        Assert.AreEqual(2, error.Errors.Count(e => e is RedefineError));
        Assert.AreEqual(1, error.Errors.Count(e => e is UseOfGuardError));
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
        var parser = Utils.GetParser(RedefineInInnerScope);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);
        Assert.AreEqual(3, error.Errors.Count);
    }
}