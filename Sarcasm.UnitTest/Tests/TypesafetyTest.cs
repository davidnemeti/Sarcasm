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
using Sarcasm.Ast;
using Sarcasm.Unparsing;

using MiniPL.DomainModel;

using Grammar = Sarcasm.Ast.Grammar;
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

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_NonPunctuationForChoice_SetRuleOr()
        {
            string sourceCodeFail = @"
                B.Expression.SetRuleOr(
                    B.BinaryExpression,
                    B.UnaryExpression,
                    B.ConditionalTernaryExpression,
                    B.FOR + B.Expression + B.IF
                    );
            ";

            string sourceCodeSuccess = @"
                B.Expression.RuleRaw = 
                    (BnfTerm)B.BinaryExpression
                    | (BnfTerm)B.UnaryExpression
                    | (BnfTerm)B.ConditionalTernaryExpression
                    | B.FOR + (BnfTerm)B.Expression + B.IF
                    ;
            ";

            CompileAndCheck(sourceCodeSuccess, shouldCompile: true);
            CompileAndCheck(sourceCodeFail, shouldCompile: false);
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

            CompileAndCheck_RuleShouldFail_RuleRawShouldSucceed(sourceCodeWithRule);
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

            CompileAndCheck(sourceCodeSuccess, shouldCompile: true);
            CompileAndCheck(sourceCodeFail, shouldCompile: false);
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

            CompileAndCheck_RuleShouldFail_RuleTypelessShouldSucceed(sourceCodeWithRule);
        }

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_BindMemberForChoice()
        {
            string sourceCodeWithRule = @"
                var numberLiteral = new BnfiTermChoice<MiniPL.DomainModel.NumberLiteral>();
                numberLiteral.Rule =
                    B.LEFT_PAREN
                    + B.Expression.BindMember(numberLiteral, t => t.Value)
                    + B.RIGHT_PAREN;
            ";

            CompileAndCheck_RuleShouldFail_RuleRawShouldSucceed(sourceCodeWithRule);
        }

        [TestMethod]
        [TestCategory(category)]
        public void TypesafetyCheck_BindMember_ValueTypeMemberTypeMismatch_SpecialCaseWithInheritance()
        {
            string sourceCodeInit = @"
                var logicalBinaryBoolExpression = new BnfiTermType<MiniPLExtension.LogicalBinaryBoolExpression>();
                var logicalBinaryBoolExpressionTL = new BnfiTermTypeTL(typeof(MiniPLExtension.LogicalBinaryBoolExpression));
                var boolExpression = new BnfiTermType<MiniPLExtension.BoolExpression>();
                var expressionTL = new BnfiTermChoiceTL(typeof(MiniPL.DomainModel.Expression));
            ";

            string sourceCodeFail = @"
                var foo = B.Expression.BindMember(logicalBinaryBoolExpression, t => t.Term1);
            ";

            string sourceCodeSuccess = @"
                var foo2 = expressionTL.BindMember(logicalBinaryBoolExpression, t => t.Term1);
            ";

            string sourceCodeSuccess2 = @"
                var foo3 = expressionTL.BindMember(() => new MiniPLExtension.LogicalBinaryBoolExpression().Term1);
            ";

            string sourceCodeSuccess3 = @"
                var foo4 = boolExpression.BindMember(B.BinaryExpression, t => t.Term1);
            ";

            CompileAndCheck(sourceCodeInit + sourceCodeFail, shouldCompile: false);
            CompileAndCheck(sourceCodeInit + sourceCodeSuccess, shouldCompile: true);
            CompileAndCheck(sourceCodeInit + sourceCodeSuccess2, shouldCompile: true);
            CompileAndCheck(sourceCodeInit + sourceCodeSuccess3, shouldCompile: true);
        }

        #region Helpers

        private void CompileAndCheck_RuleShouldFail_RuleTypelessShouldSucceed(string sourceCodeWithRule)
        {
            CompileAndCheck(sourceCodeWithRule, shouldCompile: false);
            CompileAndCheck(ReplaceWithCheck(sourceCodeWithRule, ruleString, ruleTypelessString), shouldCompile: true);
            CompileAndCheck(ReplaceWithCheck(sourceCodeWithRule, ruleString, ruleRawString), shouldCompile: true);
        }

        private void CompileAndCheck_RuleShouldFail_RuleRawShouldSucceed(string sourceCodeWithRule)
        {
            CompileAndCheck(sourceCodeWithRule, shouldCompile: false);
            CompileAndCheck(ReplaceWithCheck(sourceCodeWithRule, ruleString, ruleTypelessString), shouldCompile: false);
            CompileAndCheck(ReplaceWithCheck(sourceCodeWithRule, ruleString, ruleRawString), shouldCompile: true);
        }

        private void CompileAndCheck(string methodBodySourceCode, bool shouldCompile)
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
                Assert.IsTrue(results.Errors.Count > 0, "This should have failed, but has been compiled with success:\n{0}", methodBodySourceCode);
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
                using Sarcasm.Ast;
                using Sarcasm.Unparsing;
                using Sarcasm.UnitTest;
                using MiniPL.DomainModel;

                public static class TestCode
                {
                    public static void TestMethod()
                    {
                        var grammar = new MiniPL.GrammarP();
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
