using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;

namespace LUIECompilerTests.CodeGeneration;

[TestClass, TestCategory("CodeGeneration"), TestCategory("UseOfGuard")]
public class UseOfGuardTest
{
    public const string UseGuardInBlock =
    @"  qubit c;

        qif c do
            h c;      // line 4: Error here
        end
    ";

    public const string UseGuardInBlockIfStatment =
    @"  qubit[5] a;
        qubit c;
        x c;

        qif c do
            h a[1];
            qif c do                         // line 7: Error here
                h a[1];
            end
            h c;                            // line 10: Error here
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

    public const string RegisterAccess =
    @"  qubit[3] c;

        qif c[1] do
            h c[1];    
        end
    ";

    [TestMethod]
    public void UseGuardInBlockTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(UseGuardInBlock);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());
        CodeGenerationException exception = Assert.ThrowsException<CodeGenerationException>
        (
            codegen.CodeGen.GenerateCode
        );

        Assert.IsNotNull(exception);

        Assert.AreEqual(typeof(UseOfGuardError), exception.Error.GetType());
        Assert.AreEqual(1, exception.Error.ErrorContext.Line);
    }

    [TestMethod]
    public void UseGuardInBlockIfStatmentTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(UseGuardInBlockIfStatment);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());
        CodeGenerationException exception = Assert.ThrowsException<CodeGenerationException>
        (
            codegen.CodeGen.GenerateCode
        );

        Assert.IsNotNull(exception);

        Assert.AreEqual(typeof(UseOfGuardError), exception.Error.GetType());
        Assert.AreEqual(2, exception.Error.ErrorContext.Line);
    }
    
    [TestMethod]
    public void UseGuardInBlockCompositeGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(UseGuardInBlockCompositeGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());
        CodeGenerationException exception = Assert.ThrowsException<CodeGenerationException>
        (
            codegen.CodeGen.GenerateCode
        );

        Assert.IsNotNull(exception);

        Assert.AreEqual(typeof(UseOfGuardError), exception.Error.GetType());
        Assert.AreEqual(11, exception.Error.ErrorContext.Line);
    }
    [TestMethod]
    public void RegisterAccessTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(RegisterAccess);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());
        CodeGenerationException exception = Assert.ThrowsException<CodeGenerationException>
        (
            codegen.CodeGen.GenerateCode
        );

        Assert.IsNotNull(exception);

        Assert.AreEqual(typeof(UseOfGuardError), exception.Error.GetType());
        Assert.AreEqual(1, exception.Error.ErrorContext.Line);
    }
}