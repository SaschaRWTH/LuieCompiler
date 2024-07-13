using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompilerTests.CodeGeneration;

[TestClass, TestCategory("CodeGeneration"), TestCategory("Register")]
public class RegisterTest
{
        public const string RegisterSizeInput =
                "qubit[3] a;\n" +
                "qubit[6] b;\n" +
                "qubit[9] c;";

        /// <summary>
        /// Beware: with changes to the translation code, the translations could change, while still being correct!
        /// </summary>
        public const string RegisterSizeTranslation =
                "qubit[3] id0;\n" +
                "qubit[6] id1;\n" +
                "qubit[9] id2;\n";

        public const string InvalidRegisterSizeTemplate =
                "qubit[@size] a;";

        public const string RegisterAccessInput =
                "qubit[3] c;\n" +
                "qubit[3] a;\n" +
                "x c[1];\n" +
                "qif c[1] do\n" +
                "qubit[3] a;\n" +
                "x c[1];\n" +
                "h a[2];\n" +
                "end";

        /// <summary>
        /// Beware: with changes to the translation code, the translations could change, while still being correct!
        /// </summary>
        public const string RegisterAccessTranslation =
                "qubit[3] id0;\n" +
                "qubit[3] id1;\n" +
                "x id0[1];\n" +
                "qubit[3] id2;\n" +
                "ctrl(1) @ x id0[1], id0[1];\n" +
                "ctrl(1) @ h id0[1], id2[2];\n";

        public const string InvalidRegisterAccessSimple =
            "qubit[5] c;\n" +
            "h c[-1];\n";
            
        public const string InvalidRegisterAccessNegative =
            "qubit[5] c;\n" +
            "h c[5];\n";

        public const string InvalidRegisterAccessForLoop =
            "qubit[5] c;\n" +
            "for i in 0..10 do\n" +
            "    h c[i];\n" +
            "end";

        public const string IteratorRegisterSizeInput = 
                "qubit[3] c;\n" +
                "for i in 2..4 do\n" +
                "    qubit[i] d;\n" +
                "    h d[0];\n" +
                "end\n";

        public const string IteratorRegisterSizeTranslation =
                "qubit[3] id0;\n" +
                "qubit[2] id1;\n" +
                "h id1[0];\n" +
                "qubit[3] id2;\n" +
                "h id2[0];\n" +
                "qubit[4] id3;\n" +
                "h id3[0];\n";

        /// <summary>
        /// Tests if the scope is correctly handled in the input.
        /// </summary>
        [TestMethod]
        public void RegisterSizeTest()
        {
                var walker = Utils.GetWalker();
                var parser = Utils.GetParser(RegisterSizeInput);

                var codegen = new CodeGenerationListener();
                walker.Walk(codegen, parser.parse());

                string? code = codegen.CodeGen.GenerateCode()?.ToString();
                Assert.IsNotNull(code);

             
                Assert.AreEqual(code, RegisterSizeTranslation);
        }
        /// <summary>
        /// Tests the invalid register sizes (negative of 0) throw an exception.
        /// </summary>
        [TestMethod]
        public void InvalidRegisterSizeTest()
        {
                for(int i = -10; i <= 0; i++)
                {
                        InvalidRegisterSizeTest(i);
                }
        }
        private void InvalidRegisterSizeTest(int size)
        {
                string input = InvalidRegisterSizeTemplate.Replace("@size", size.ToString());
                var walker = Utils.GetWalker();
                var parser = Utils.GetParser(input);

                var codegen = new CodeGenerationListener();
                walker.Walk(codegen, parser.parse());
                
                var e = Assert.ThrowsException<CodeGenerationException>(
                        codegen.CodeGen.GenerateCode
                );
                
                Assert.IsTrue(e.Error is InvalidSizeError);
                Assert.IsTrue(e.Error.ErrorContext.Line == 1);
        }


        /// <summary>
        /// Tests if the scope is correctly handled in the input.
        /// </summary>
        [TestMethod]
        public void RegisterAccessTest()
        {
                var walker = Utils.GetWalker();
                var parser = Utils.GetParser(RegisterAccessInput);

                var codegen = new CodeGenerationListener();
                walker.Walk(codegen, parser.parse());

                string? code = codegen.CodeGen.GenerateCode()?.ToString();
                Assert.IsNotNull(code);

                Assert.AreEqual(code, RegisterAccessTranslation);
        }

        /// <summary>
        /// Tests if the scope is correctly handled in the input.
        /// </summary>
        [TestMethod]
        public void SimpleInvalidRegisterAccessTest()
        {
                var walker = Utils.GetWalker();
                var parser = Utils.GetParser(InvalidRegisterAccessSimple);

                var codegen = new CodeGenerationListener();
                walker.Walk(codegen, parser.parse());
                CodeGenerationException exception = Assert.ThrowsException<CodeGenerationException>(
                        codegen.CodeGen.GenerateCode
                );

                Assert.IsNotNull(exception);

                Assert.AreEqual(exception.Error.ErrorContext.Line, 2);
        }

        /// <summary>
        /// Tests if the scope is correctly handled in the input.
        /// </summary>
        [TestMethod]
        public void InvalidRegisterAccessNegativeTest()
        {
                var walker = Utils.GetWalker();
                var parser = Utils.GetParser(InvalidRegisterAccessNegative);

                var codegen = new CodeGenerationListener();
                walker.Walk(codegen, parser.parse());
                CodeGenerationException exception = Assert.ThrowsException<CodeGenerationException>(
                        codegen.CodeGen.GenerateCode
                );

                Assert.IsNotNull(exception);

                Assert.AreEqual(exception.Error.ErrorContext.Line, 2);
        }
        /// <summary>
        /// Tests if the scope is correctly handled in the input.
        /// </summary>
        [TestMethod]
        public void ForLoopInvalidRegisterAccessTest()
        {
                var walker = Utils.GetWalker();
                var parser = Utils.GetParser(InvalidRegisterAccessForLoop);

                var codegen = new CodeGenerationListener();
                walker.Walk(codegen, parser.parse());
                CodeGenerationException exception = Assert.ThrowsException<CodeGenerationException>(
                        codegen.CodeGen.GenerateCode
                );

                Assert.IsNotNull(exception);

                Assert.AreEqual(exception.Error.ErrorContext.Line, 3);
        }
        /// <summary>
        /// Tests if the scope is correctly handled in the input.
        /// </summary>
        [TestMethod]
        public void IteratorRegisterSizeTest()
        {
                var walker = Utils.GetWalker();
                var parser = Utils.GetParser(IteratorRegisterSizeInput);

                var codegen = new CodeGenerationListener();
                walker.Walk(codegen, parser.parse());

                string? code = codegen.CodeGen.GenerateCode()?.ToString();
                Assert.IsNotNull(code);

                Assert.AreEqual(IteratorRegisterSizeTranslation, code);
        }

}