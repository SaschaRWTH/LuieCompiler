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
        Assert.AreEqual(3, e.Error.Line);
    }

}