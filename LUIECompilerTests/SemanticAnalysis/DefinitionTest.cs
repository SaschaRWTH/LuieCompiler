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

    public const string ValidQFTInput =
    @"
        // Swaps the values of two qubits
        gate swap(a, b) do
            cx a, b;
            cx b, a;
            cx a, b;
        end

        // Performs a discrete Fourier transform on a register of qubits
        gate qft(reg) do
            for i in range(sizeof(reg)) do
                h reg[i];
                for j in range(sizeof(reg) - (i + 1)) do
                    qif reg[j + (i + 1)] do
                        p(1/(power(2, (j + 1)))) reg[i];
                    end
                end
            end
            for j in range(sizeof(reg) / 2) do
                swap reg[j], reg[sizeof(reg) - (j + 1)];
            end
        end


        qubit[5] a;
        qft a;
    ";
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
    
    [TestMethod]
    public void ValidQFTInputTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(ValidQFTInput);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsFalse(error.ContainsCriticalError);
        Assert.IsTrue(error.Errors.Count == 0);
    }
}