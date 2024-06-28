using LUIECompiler.CodeGeneration;
using LUIECompiler.Common.Symbols;

namespace LUIECompilerTests;

[TestClass]
public class CodeGenerationTests
{
    [TestMethod]
    public void ScopeCorrectInfoTest()
    {
        CodeGenerationHandler handler = new();
        
        handler.PushCodeBlock();
        Register firstA = handler.AddRegister("A", 1);

        handler.PushCodeBlock();
        Register secondA = handler.AddRegister("A", 2);

        Register? secondScopeA = handler.GetSymbolInfo("A", 3) as Register;
        Assert.IsNotNull(secondScopeA);
        Assert.AreNotEqual(firstA, secondScopeA);
        Assert.AreEqual(secondA, secondScopeA);

        handler.PopCodeBlock();
        Register? firstScopeA = handler.GetSymbolInfo("A", 4) as Register;
        Assert.IsNotNull(secondScopeA);
        Assert.AreNotEqual(secondA, firstScopeA);
        Assert.AreEqual(firstA, firstScopeA);
    }
}