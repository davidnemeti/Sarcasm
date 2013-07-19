using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.Unparsing;
using Sarcasm.Utility;

namespace Sarcasm.GrammarAst
{
    public abstract partial class BnfiTermChoice : BnfiTermNonTerminal, IBnfiTerm, IUnparsableNonTerminal
    {
        protected BnfiTermChoice(Type type, string name)
            : base(type, name)
        {
            GrammarHelper.MarkTransientForced(this);      // the child node already contains the created ast node
        }

        protected new BnfiExpression Rule { set { base.Rule = value; } }

        public BnfExpression RuleRaw { get { return base.Rule; } set { base.Rule = value; } }

        #region Unparse

        bool IUnparsableNonTerminal.TryGetUtokensDirectly(IUnparser unparser, object astValue, out IEnumerable<UtokenValue> utokens)
        {
            utokens = null;
            return false;
        }

        IEnumerable<UnparsableAst> IUnparsableNonTerminal.GetChildren(IList<BnfTerm> childBnfTerms, object astValue, Unparser.Direction direction)
        {
            return childBnfTerms.Select(childBnfTerm => new UnparsableAst(childBnfTerm, astValue));
        }

        int? IUnparsableNonTerminal.GetChildrenPriority(IUnparser unparser, object astValue, IEnumerable<UnparsableAst> children)
        {
            UnparsableAst mainChild = children.Single(childValue => IsMainChild(childValue.BnfTerm));

            if (astValue.GetType() == this.type)
                return unparser.GetPriority(mainChild);
            else
            {
                IHasType mainChildBnfTermWithType = mainChild.BnfTerm as IHasType;

                if (mainChildBnfTermWithType == null)
                {
                    throw new UnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' is not a BnfiTermNonTerminal.",
                        astValue, astValue.GetType().Name, mainChild.BnfTerm));
                }

                int? inheritancePriority = 0 - mainChildBnfTermWithType.Type.GetInheritanceDistance(astValue);
                int? childPriority = inheritancePriority != null && (mainChild.BnfTerm != this || mainChild.AstValue != astValue) ? unparser.GetPriority(mainChild) : null;
                int? priority = inheritancePriority * 100 + childPriority;

                Unparser.tsPriorities.Indent();
                priority.DebugWriteLinePriority(Unparser.tsPriorities, mainChild);
                Unparser.tsPriorities.Unindent();

                return priority;
            }
        }

        private static bool IsMainChild(BnfTerm bnfTerm)
        {
            return !bnfTerm.Flags.IsSet(TermFlags.IsPunctuation) && !(bnfTerm is GrammarHint);
        }

        #endregion
    }

    public partial class BnfiTermChoiceTL : BnfiTermChoice, IBnfiTermTL
    {
        public BnfiTermChoiceTL(Type type, string name = null)
            : base(type, name)
        {
        }

        public new BnfiExpressionChoiceTL Rule { set { base.Rule = value; } }
    }

    public partial class BnfiTermChoice<TType> : BnfiTermChoice, IBnfiTerm<TType>, IBnfiTermOrAbleForChoice<TType>, INonTerminal<TType>
    {
        public BnfiTermChoice(string name = null)
            : base(typeof(TType), name)
        {
        }

        [Obsolete(typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public BnfiExpressionChoiceTL RuleTypeless { set { base.Rule = value; } }

        public new BnfiExpressionChoice<TType> Rule { set { base.Rule = value; } }

        // NOTE: type inference for subclasses works only if SetRuleOr is an instance method and not an extension method
        public void SetRuleOr(IBnfiTermOrAbleForChoice<TType> bnfiTermFirst, IBnfiTermOrAbleForChoice<TType> bnfiTermSecond, params IBnfiTermOrAbleForChoice<TType>[] bnfiTerms)
        {
            this.Rule = Or(bnfiTermFirst, bnfiTermSecond, bnfiTerms);
        }

        public BnfiExpressionChoice<TType> Or(IBnfiTermOrAbleForChoice<TType> bnfiTermFirst, IBnfiTermOrAbleForChoice<TType> bnfiTermSecond, params IBnfiTermOrAbleForChoice<TType>[] bnfiTerms)
        {
            return (BnfiExpressionChoice<TType>)bnfiTerms
                .Select(bnfiTerm => bnfiTerm.AsBnfTerm())
                .Aggregate(
                bnfiTermFirst.AsBnfTerm() | bnfiTermSecond.AsBnfTerm(),
                (bnfExpressionProcessed, bnfTermToBeProcess) => bnfExpressionProcessed | bnfTermToBeProcess
                );
        }
    }
}
