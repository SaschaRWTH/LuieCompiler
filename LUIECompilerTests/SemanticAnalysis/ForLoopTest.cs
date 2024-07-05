using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;
using LUIECompiler.SemanticAnalysis;

namespace LUIECompilerTests.SemanticAnalysis;

[TestClass]
public class ForLoopTest
{            
    public const string InputCorrect =
        "qubit c;\n" +
        "qubit a;\n" +
        "for i in 1..3 do\n" +
        "    h c;\n" +
        "    cx c, a;\n" +
        "end";

    public const string InputRedefine =
        "qubit c;\n" +
        "qubit a;\n" +
        "for a in 1..3 do\n" + // Use of already defined identifier
        "    h c;\n" +
        "    cx c, a;\n" +
        "end";

    public const string InputIteratorExpressionCorrect = 
        "qubit[3] c;\n" +
        "for i in 1..3 do\n" +
        "    h c[i];\n" +
        "end";

    public const string InputIteratorExpressionIncorrect = 
        "qubit[3] c;\n" +
        "qubit b;\n" +
        "for i in 1..3 do\n" +
        "    h c[b];\n" +
        "end";

    /// <summary>
    /// Test no error are incorrectly reported.
    /// </summary>
    [TestMethod]
    public void CorrectInputTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputCorrect);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(!error.ContainsCriticalError);
    }

    /// <summary>
    /// Test that already defined identifiers are correctly reported.
    /// </summary>
    [TestMethod]
    public void RedefineErrorTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputRedefine);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);

        
        Assert.IsTrue(error.Errors.Any(e => e is RedefineError && e.Line == 3));
    }

    /// <summary>
    /// Test no error are incorrectly reported.
    /// </summary>
    [TestMethod]
    public void CorrectIteratorUseTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputIteratorExpressionCorrect);
        var analysis = new TypeCheckListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Console.WriteLine($"Error:\n{error}");
        Assert.IsTrue(!error.ContainsCriticalError);
    }

    /// <summary>
    /// Test no error are incorrectly reported.
    /// </summary>
    [TestMethod]
    public void IncorrectIteratorUseTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputIteratorExpressionIncorrect);
        var analysis = new TypeCheckListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);
        
        Console.WriteLine($"Error count: {error.Errors.Count}");
        Console.WriteLine($"Error: {error}");
        Assert.IsTrue(error.Errors.Any(e => e is TypeError && e.Line == 4));
    }

}