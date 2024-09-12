using LUIECompiler.Common.Errors;
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

    public const string MultipleErrors =
    @"  qubit[5] c;
        qif a do
            qubit c;
            h c;
        end

        h c[1];
        h b;
    ";

    public const string UseGuardInBlock =
    @"  qubit[5] a;
        qubit c;
        x c;

        qif c do
            h a[1];
            h c;                            // line 7: Error here
            for i in range(sizeof(a)) do
                h a[i];
                x c;                        // line 10: Error here
            end
        end
    ";

    public const string UseGuardInBlockIfStatment =
    @"  qubit[5] a;
        qubit c;
        x c;

        qif c do
            h a[1];
            h c;                            // line 7: Error here
            qif c do                         // line 8: Error here
                h a[1];
            end
        end
    ";

    public const string UseGuardInBlockCompositeGate =
    @"  
        // Swaps the values of two qubits
        gate swap(a, b) do
            cx a, b;
            cx b, a;
            cx a, b;
        end
        
        qubit a;
        qubit b;
        qubit c;
        x c;

        qif c do
            swap a, b;
            swap a, c;  // line 16: Error here
            swap b, c;  // line 17: Error here
        end
    ";

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

    public const string UsedGateSymbol =
    @"
        // Swaps the values of two qubits
        gate swap(a, b) do
            cx a, b;
            cx b, a;
            cx a, b;
        end

        qubit[5] a;
        h a[0];
    ";

    public const string UsedRegisterSymbol =
    @"
        qubit[5] a;
        qubit[5] b;
        h a[0];
    ";
    
    public const string UnusedParameterSymbol =
    @"
        // Swaps the values of two qubits
        gate test(a, b) do
            h a;
        end

        qubit[5] a;
        test a[0], a[1];
    ";
    
    public const string UnusedIteratorSymbol =
    @"
        qubit[5] a;
        
        for i in range(sizeof(a)) do
            h a[0];
        end
    ";
    public const string UseOfThrowAwaySymbol =
    @"
        qubit[5] a;
        
        for _ in range(sizeof(a)) do
            h a[0];
        end
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

    [TestMethod]
    public void MutlitpleErrorsTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(MultipleErrors);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);
        Assert.IsTrue(error.Errors.Count == 2);
        
        Assert.IsTrue(error.Errors.Any(e => e is UndefinedError && e.ErrorContext.Line == 2));
        Assert.IsTrue(error.Errors.Any(e => e is UndefinedError && e.ErrorContext.Line == 8));
    }

    [TestMethod]
    public void UseGuardInBlockTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(UseGuardInBlock);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);
        Assert.AreEqual(2, error.Errors.Count);
        
        Assert.IsTrue(error.Errors.Any(e => e is UseOfGuardError && e.ErrorContext.Line == 7));
        Assert.IsTrue(error.Errors.Any(e => e is UseOfGuardError && e.ErrorContext.Line == 7));
    }
    
    [TestMethod]
    public void UseGuardInBlockIfStatmentTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(UseGuardInBlockIfStatment);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Console.WriteLine(error);

        Assert.IsTrue(error.ContainsCriticalError);
        Assert.AreEqual(2, error.Errors.Count);
        
        Assert.IsTrue(error.Errors.Any(e => e is UseOfGuardError && e.ErrorContext.Line == 7));
        Assert.IsTrue(error.Errors.Any(e => e is UseOfGuardError && e.ErrorContext.Line == 8));
    }
    
    [TestMethod]
    public void UseGuardInBlockCompositeGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(UseGuardInBlockCompositeGate);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);
        Assert.AreEqual(2, error.Errors.Count);
        
        Assert.IsTrue(error.Errors.Any(e => e is UseOfGuardError && e.ErrorContext.Line == 16));
        Assert.IsTrue(error.Errors.Any(e => e is UseOfGuardError && e.ErrorContext.Line == 17));
    }
    
    [TestMethod]
    public void UsedGateSymbolTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(UsedGateSymbol);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsFalse(error.ContainsCriticalError);
        Assert.AreEqual(1, error.Errors.Count);
        
        Assert.IsTrue(error.Errors.Any(e => e is UnusedSymbolWarning && e.ErrorContext.Line == 3));
    }
    
    [TestMethod]
    public void UsedRegisterSymbolTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(UsedRegisterSymbol);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsFalse(error.ContainsCriticalError);
        Assert.AreEqual(1, error.Errors.Count);
        
        Assert.IsTrue(error.Errors.Any(e => e is UnusedSymbolWarning && e.ErrorContext.Line == 3));
    }
    
    [TestMethod]
    public void UnusedParameterSymbolTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(UnusedParameterSymbol);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsFalse(error.ContainsCriticalError);
        Assert.AreEqual(1, error.Errors.Count);
        
        Assert.IsTrue(error.Errors.Any(e => e is UnusedSymbolWarning && e.ErrorContext.Line == 3));
    }
    
    [TestMethod]
    public void UnusedIteratorSymbolTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(UnusedIteratorSymbol);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsFalse(error.ContainsCriticalError);
        Assert.AreEqual(1, error.Errors.Count);
        
        Assert.IsTrue(error.Errors.Any(e => e is UnusedSymbolWarning && e.ErrorContext.Line == 4));
    }
    
    [TestMethod]
    public void UseOfThrowAwaySymbolTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(UseOfThrowAwaySymbol);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsFalse(error.ContainsCriticalError);
        Assert.AreEqual(0, error.Errors.Count);
    }
}