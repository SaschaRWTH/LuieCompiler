using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;
using LUIECompiler.SemanticAnalysis;

namespace LUIECompilerTests;

[TestClass]
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
        Assert.IsTrue(error.Errors.Any(e => e is UndefinedError && e.Line == 8));
        Assert.IsTrue(error.Errors.Any(e => e is TypeError && e.Line == 4));
        Assert.IsTrue(error.Errors.Any(e => e is TypeError && e.Line == 7));
    }
}