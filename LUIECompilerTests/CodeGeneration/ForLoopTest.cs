using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;
using LUIECompiler.SemanticAnalysis;

namespace LUIECompilerTests.CodeGeneration;

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
     
    public const string InputCorrectTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "h id0;\n" +
        "cx id0, id1;\n" +
        "h id0;\n" +
        "cx id0, id1;\n" +
        "h id0;\n" +
        "cx id0, id1;\n";


    public const string InputRedefine =
        "qubit c;\n" +
        "qubit a;\n" +
        "for a in 1..3 do\n" + // Use of already defined identifier
        "    h c;\n" +
        "    cx c, a;\n" +
        "end";

    public const string InputIdenExpression = 
        "qubit[3] c;\n" +
        "qubit a;\n" +
        "for i in 0..2 do\n" +
        "    h c[i];\n" +
        "    cx c[i], a;\n" +
        "end";

    public const string InputIdenExpressionTranslation =
        "qubit[3] id0;\n" +
        "qubit id1;\n" +
        "h id0[0];\n" +
        "cx id0[0], id1;\n" +
        "h id0[1];\n" +
        "cx id0[1], id1;\n" +
        "h id0[2];\n" +
        "cx id0[2], id1;\n";

    /// <summary>
    /// Test the translation of a simple for loop.
    /// </summary>
    [TestMethod]
    public void CorrectInputTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputCorrect);
        var gen = new CodeGenerationListener();
        walker.Walk(gen, parser.parse());
        
        string? code = gen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(code, InputCorrectTranslation);
    }

    /// <summary>
    /// Test that already defined identifiers are correctly reported.
    /// </summary>
    [TestMethod]
    public void RedefineErrorTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputRedefine);
        var gen = new CodeGenerationListener();
        CodeGenerationException e = Assert.ThrowsException<CodeGenerationException>(() 
            => walker.Walk(gen, parser.parse())
            );
        Assert.IsTrue(e.Error is RedefineError);
        Assert.AreEqual(3, e.Error.ErrorContext.Line);
    }

    /// <summary>
    /// Test the translation of a simple for loop.
    /// </summary>
    [TestMethod]
    public void InputIdenExpressionTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputIdenExpression);
        var gen = new CodeGenerationListener();
        walker.Walk(gen, parser.parse());
        
        string? code = gen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(InputIdenExpressionTranslation, code);
    }
}