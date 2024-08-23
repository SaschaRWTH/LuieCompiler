using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Optimization;

namespace LUIECompilerTests.Optimization;


[TestClass, TestCategory("Optimization")]
public class GeneralOptimizatioTest
{
    public const string NullGatePeepingControlCombined = @"
        qubit a;
        qubit b;
        qubit c;
        h b;
        z b;
        qif c do 
            h a;
        end
        x c;
        x c;
        qif c do 
            h a;
        end
        z b;
        y b;
    ";

    public const string NullGatePeepingControlCombinedOptimized =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "qubit id2;\n" +
        "h id1;\n" +
        "y id1;\n";

    [TestMethod]
    public void TestNullGatePeepingControlCombined()
    {       var walker = Utils.GetWalker();
        var parser = Utils.GetParser(NullGatePeepingControlCombined);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        QASMProgram optimized = program.Optimize(OptimizationType.All);

        string optimizedCode = optimized.ToString();
        Assert.IsNotNull(optimizedCode);

        Assert.AreEqual(NullGatePeepingControlCombinedOptimized, optimizedCode);
    }

}