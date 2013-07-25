using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CSharp;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.GrammarAst;
using Sarcasm.Parsing;
using Sarcasm.Unparsing;

using MiniPL.DomainModel;

using Grammar = Sarcasm.GrammarAst.Grammar;
using System.Reflection;

namespace Sarcasm.UnitTest
{
    [TestClass]
    public partial class TypesafetyTest : CommonTest
    {
        protected const string category = "TypesafetyTest";

        private const string ruleString = ".Rule";
        private const string ruleTypelessString = ".RuleTypeless";
        private const string ruleRawString = ".RuleRaw";

        private static void TypesafetyCheck_BodyShouldCompile()
        {
            // NOTE: This is just to test whether these calls compile successfully. There is no need to actually call the Parse methods.

            // ensure that parser is typesafe
            MultiParser<MiniPL.DomainModel.Program> _parser = parser;

            parser.Parse(string.Empty);
            parser.Parse(string.Empty, string.Empty);

            parser.Parse(string.Empty, grammar.B.Expression);
            parser.Parse(string.Empty, string.Empty, grammar.B.Expression);

            parser.Parse(string.Empty, (NonTerminal)grammar.B.Expression);
            parser.Parse(string.Empty, string.Empty, (NonTerminal)grammar.B.Expression);
        }

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_NonPunctuationForChoice_SetRuleOr()
        {
            string sourceCodeSuccess = @"
                B.Expression.SetRuleOr(
                    B.BinaryExpression,
                    B.UnaryExpression,
                    B.ConditionalTernaryExpression,
                    B.FOR + B.Expression + B.IF
                    );
            ";

            CompileShouldSucceed(sourceCodeSuccess);
        }

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_NonPunctuationForChoice_Rule()
        {
            string sourceCodeWithRule = @"
                B.Expression.Rule =
                    B.FOR + B.Expression + B.IF
                    ;
            ";

            CompileShouldSucceed(sourceCodeWithRule);
        }

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_NonBnfiTermKeyTermForChoice_SetRuleOr()
        {
            string sourceCodeFail = @"
                B.Expression.SetRuleOr(
                    B.BinaryExpression,
                    B.UnaryExpression,
                    B.ConditionalTernaryExpression,
                    B.BinaryExpression + B.Expression + B.IF
                    );
            ";

            string sourceCodeSuccess = @"
                B.Expression.RuleRaw = 
                    (BnfTerm)B.BinaryExpression
                    | (BnfTerm)B.UnaryExpression
                    | (BnfTerm)B.ConditionalTernaryExpression
                    | (BnfTerm)B.BinaryExpression + (BnfTerm)B.Expression + B.IF
                    ;
            ";

            CompileShouldFail(sourceCodeFail, "CS1502", "CS1503");
            CompileShouldSucceed(sourceCodeSuccess);
        }

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_NonBnfiTermKeyTermForChoice_Rule()
        {
            string sourceCodeWithRule = @"
                B.Expression.Rule =
                    B.BinaryExpression + B.Expression + B.IF
                    ;
            ";

            CompileShouldFail_Rule(sourceCodeWithRule, "CS0266");
            CompileShouldSucceed_RuleRaw(sourceCodeWithRule);
        }

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_WrongChildTypeForChoice_SetRuleOr()
        {
            string sourceCodeFail = @"
                B.Expression.SetRuleOr(
                    B.BinaryExpression,
                    B.UnaryExpression,
                    B.Program
                    );
            ";

            string sourceCodeSuccess = @"
                B.Expression.RuleTypeless =
                    B.BinaryExpression
                    | B.UnaryExpression
                    | B.Program
                    ;
            ";

            CompileShouldFail(sourceCodeFail, "CS1502", "CS1503");
            CompileShouldSucceed(sourceCodeSuccess);
        }

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_WrongChildTypeForChoice_Rule()
        {
            string sourceCodeWithRule = @"
                B.Expression.Rule =
                    B.BinaryExpression
                    | B.UnaryExpression
                    | B.Program
                    ;
            ";

            CompileShouldFail_Rule(sourceCodeWithRule, "CS0029");
            CompileShouldSucceed_RuleTypeless(sourceCodeWithRule);
        }

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_BindToForChoice()
        {
            // BindTo for Choice is not good
            string sourceCodeFail = @"
                var numberLiteral = new BnfiTermChoice<MiniPL.DomainModel.NumberLiteral>();
                numberLiteral.Rule =
                    B.LEFT_PAREN
                    + B.Expression.BindTo(numberLiteral, t => t.Value)
                    + B.RIGHT_PAREN;
            ";

            // BindTo for Record is good
            string sourceCodeSuccess = @"
                var numberLiteral = new BnfiTermRecord<MiniPL.DomainModel.NumberLiteral>();
                numberLiteral.Rule =
                    B.LEFT_PAREN
                    + B.Expression.BindTo(numberLiteral, t => t.Value)
                    + B.RIGHT_PAREN;
            ";

            CompileShouldFail(sourceCodeFail, "CS0029");
            CompileShouldSucceed(sourceCodeSuccess);
        }

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_BindTo_ValueTypeMemberTypeMismatch_SpecialCaseWithInheritance()
        {
            string sourceCodeInit = @"
                var logicalBinaryBoolExpression = new BnfiTermRecord<MiniPLExtension.LogicalBinaryBoolExpression>();
                var logicalBinaryBoolExpressionTL = new BnfiTermRecordTL(typeof(MiniPLExtension.LogicalBinaryBoolExpression));
                var boolExpression = new BnfiTermRecord<MiniPLExtension.BoolExpression>();
                var expressionTL = new BnfiTermChoiceTL(typeof(MiniPL.DomainModel.Expression));
            ";

            string sourceCodeFail = @"
                var foo = B.Expression.BindTo(logicalBinaryBoolExpression, t => t.Term1);
            ";

            // typeless binding is okay
            string sourceCodeSuccess = @"
                var foo2 = expressionTL.BindTo(logicalBinaryBoolExpression, t => t.Term1);
            ";

            // another form of typeless binding is okay
            string sourceCodeSuccess2 = @"
                var foo3 = expressionTL.BindTo(() => new MiniPLExtension.LogicalBinaryBoolExpression().Term1);
            ";

            // opposite situation with type safety is okay
            string sourceCodeSuccess3 = @"
                var foo4 = boolExpression.BindTo(B.BinaryExpression, t => t.Term1);
            ";

            CompileShouldFail(sourceCodeInit + sourceCodeFail, "CS0311");
            CompileShouldSucceed(sourceCodeInit + sourceCodeSuccess);
            CompileShouldSucceed(sourceCodeInit + sourceCodeSuccess2);
            CompileShouldSucceed(sourceCodeInit + sourceCodeSuccess3);
        }

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_BindTo_ValueTypeMemberTypeMismatch()
        {
            string sourceCodeFail = @"
                var foo = B.Function.BindTo(B.While, t => t.Body);
            ";

            // typeless binding is okay
            string sourceCodeSuccess = @"
                var foo2 = new BnfiTermRecordTL(typeof(MiniPL.DomainModel.Function)).BindTo(B.While, t => t.Body);
            ";

            // another form of typeless binding is okay
            string sourceCodeSuccess2 = @"
                var foo3 = new BnfiTermRecordTL(typeof(MiniPL.DomainModel.Function)).BindTo(() => new MiniPL.DomainModel.While().Body);
            ";

            CompileShouldFail(sourceCodeFail, "CS0311");
            CompileShouldSucceed(sourceCodeSuccess);
            CompileShouldSucceed(sourceCodeSuccess2);
        }

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_BindTo_ValueTypeMemberTypeMismatchForCollection()
        {
            string sourceCodeFail = @"
                var foo = B.Function.PlusList().BindTo(B.Write, t => t.Arguments);
            ";

            // typeless binding is okay
            string sourceCodeSuccess = @"
                var foo2 = new BnfiTermRecordTL(typeof(MiniPL.DomainModel.Function)).PlusListTL().BindTo(B.While, t => t.Body);
            ";

            // another form of typeless binding is okay
            string sourceCodeSuccess2 = @"
                var foo3 = new BnfiTermRecordTL(typeof(MiniPL.DomainModel.Function)).PlusListTL().BindTo(() => new MiniPL.DomainModel.While().Body);
            ";

            CompileShouldFail(sourceCodeFail, "CS0311");
            CompileShouldSucceed(sourceCodeSuccess);
            CompileShouldSucceed(sourceCodeSuccess2);
        }

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_BindTo_MissingDeclaringType()
        {
            string sourceCodeFail = @"
                var foo = B.Statement.BindTo(t => t.Body);
            ";

            // with declaring type specified it is okay
            string sourceCodeSuccess = @"
                var foo2 = B.Statement.BindTo(B.While, t => t.Body);
            ";

            CompileShouldFail(sourceCodeFail, "CS1660");
            CompileShouldSucceed(sourceCodeSuccess);
        }

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_BindTo_NonExistentMember()
        {
            string sourceCodeFail = @"
                var foo = B.Statement.BindTo(B.Return, t => t.Body);
            ";

            // with proper declaring type it is okay
            string sourceCodeSuccess = @"
                var foo2 = B.Statement.BindTo(B.While, t => t.Body);
            ";

            CompileShouldFail(sourceCodeFail, "CS1061");
            CompileShouldSucceed(sourceCodeSuccess);
        }

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_BindTo_DeclaringTypeMismatch()
        {
            string sourceCodeFail = @"
                B.Program.Rule = B.Type.BindTo(B.LocalVariable, t => t.Type);
            ";

            // with typeless bnfterms it is okay
            string sourceCodeSuccess = @"
                B.Program.RuleTypeless = new BnfiTermRecordTL(typeof(MiniPL.DomainModel.Type)).BindTo(() => new MiniPL.DomainModel.LocalVariable().Type);
            ";

            CompileShouldFail(sourceCodeFail, "CS0029");
            CompileShouldSucceed(sourceCodeSuccess);
        }

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_BindTo_DeclaringTypeMismatch_ComplexRule()
        {
            string sourceCodeFail = @"
                B.Program.RuleTypeless =
                    B.Type.BindTo(B.LocalVariable, t => t.Type)
                    + B.Expression.BindTo(B.LocalVariable, t => t.InitValue);
            ";

            // with typeless bnfterms it is okay
            string sourceCodeSuccess = @"
                B.Program.RuleTypeless =
                    new BnfiTermRecordTL(typeof(MiniPL.DomainModel.Type)).BindTo(() => new MiniPL.DomainModel.LocalVariable().Type)
                    + new BnfiTermRecordTL(typeof(MiniPL.DomainModel.Expression)).BindTo(() => new MiniPL.DomainModel.LocalVariable().InitValue);
            ";

            CompileShouldFail(sourceCodeFail, "CS0029");
            CompileShouldSucceed(sourceCodeSuccess);
        }

        #region Helpers

        private void CompileShouldFail_Rule(string sourceCodeWithRule, params string[] expectedErrorCodes)
        {
            CompileAndCheck(sourceCodeWithRule, shouldCompile: false, expectedErrorCodes: expectedErrorCodes);
        }

        private void CompileShouldSucceed_RuleTypeless(string sourceCodeWithRule)
        {
            CompileAndCheck(ReplaceWithCheck(sourceCodeWithRule, ruleString, ruleTypelessString), shouldCompile: true);
        }

        private void CompileShouldSucceed_RuleRaw(string sourceCodeWithRule)
        {
            CompileAndCheck(ReplaceWithCheck(sourceCodeWithRule, ruleString, ruleRawString), shouldCompile: true);
        }

        private void CompileShouldSucceed(string methodBodySourceCode)
        {
            CompileAndCheck(methodBodySourceCode, shouldCompile: true);
        }

        private void CompileShouldFail(string methodBodySourceCode, params string[] expectedErrorCodes)
        {
            CompileAndCheck(methodBodySourceCode, shouldCompile: false, expectedErrorCodes: expectedErrorCodes);
        }

        private void CompileAndCheck(string methodBodySourceCode, bool shouldCompile, params string[] expectedErrorCodes)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            parameters.ReferencedAssemblies.Add("Irony.dll");
            parameters.ReferencedAssemblies.Add("Sarcasm.dll");
            parameters.ReferencedAssemblies.Add("MiniPL.dll");
            parameters.ReferencedAssemblies.Add(typeof(TypesafetyTest).Assembly.Location);

            string sourceCode = GetFullSourceCodeFromMethodBody(methodBodySourceCode);

            CompilerResults results = provider.CompileAssemblyFromSource(parameters, sourceCode);

            if (shouldCompile)
                Assert.IsTrue(results.Errors.Count == 0, string.Join("\n", results.Errors.Cast<CompilerError>()));
            else
            {
                Assert.IsTrue(results.Errors.Count > 0, "This should have failed, but has been compiled with success:\n{0}", methodBodySourceCode);

                var actualErrorCodes = results.Errors.Cast<CompilerError>().Select(error => error.ErrorNumber);

                if (expectedErrorCodes.Length > 0)
                {
                    CollectionAssert.AreEquivalent(expectedErrorCodes, actualErrorCodes.ToList(),
                        "Failed:\n{0}\nbut not with the expected errors. (Probably the test sourcecode needs to be updated.)\nExpected errors: {1}\nActual errors: {2}",
                        methodBodySourceCode, string.Join(", ", expectedErrorCodes), string.Join(", ", actualErrorCodes));
                }
            }
        }

        private static string GetFullSourceCodeFromMethodBody(string methodBodySourceCode)
        {
            return @"
                using System;
                using System.Linq;
                using System.Linq.Expressions;
                using Irony;
                using Irony.Ast;
                using Irony.Parsing;
                using Sarcasm;
                using Sarcasm.GrammarAst;
                using Sarcasm.Unparsing;
                using Sarcasm.UnitTest;
                using MiniPL.DomainModel;

                public static class TestCode
                {
                    public static void TestMethod()
                    {
                        var grammar = new MiniPL.Grammars.GrammarP();
                        var B = grammar.B;

                        " + methodBodySourceCode + @"
                    }
                }
            ";
        }

        private static string ReplaceWithCheck(string sourceCode, string oldValue, string newValue)
        {
            if (sourceCode.IndexOf(oldValue) == -1)
                throw new ArgumentException(string.Format("Source code does not contain the string \"{0}\"", oldValue), "sourceCode");

            return sourceCode.Replace(oldValue, newValue);
        }

        #endregion
    }

    public class MiniPLExtension
    {
        public class LogicalBinaryBoolExpression : BoolExpression
        {
            public BoolExpression Term1 { get; set; }
            public BinaryBoolOperator Op { get; set; }
            public BoolExpression Term2 { get; set; }
        }

        public class BoolExpression : MiniPL.DomainModel.Expression
        {
        }

        public enum BinaryBoolOperator
        {
            And,
            Or
        }
    }
}
