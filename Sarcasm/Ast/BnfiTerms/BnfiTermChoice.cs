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

namespace Sarcasm.Ast
{
    public abstract partial class BnfiTermChoice : BnfiTermNonTerminal, IBnfiTerm, IUnparsable
    {
        protected BnfiTermChoice(Type type, string name)
            : base(type, name, isReferable: true)
        {
            GrammarHelper.MarkTransient(this);      // the child node already contains the created ast node
        }

        protected new BnfiExpression Rule { set { base.Rule = value; } }

        public BnfExpression RuleRaw { get { return base.Rule; } set { base.Rule = value; } }

        #region Unparse

        bool IUnparsable.TryGetUtokensDirectly(IUnparser unparser, object obj, out IEnumerable<Utoken> utokens)
        {
            utokens = null;
            return false;
        }

        IEnumerable<UnparsableObject> IUnparsable.GetChildUnparsableObjects(BnfTermList childBnfTerms, object obj)
        {
            return childBnfTerms
                .Select(childBnfTerm =>
                    new UnparsableObject(
                        childBnfTerm,
                        IsMainChild(childBnfTerm) ? obj : null
                        )
                    );
        }

        int? IUnparsable.GetChildBnfTermListPriority(IUnparser unparser, object obj, IEnumerable<UnparsableObject> childUnparsableObjects)
        {
            BnfTerm mainChildBnfTerm = childUnparsableObjects.Single(childValue => IsMainChild(childValue.bnfTerm)).bnfTerm;

            if (obj.GetType() == this.type)
                return unparser.GetBnfTermPriority(mainChildBnfTerm, obj);
            else
            {
                IHasType mainChildBnfTermWithType = mainChildBnfTerm as IHasType;

                if (mainChildBnfTermWithType == null)
                {
                    throw new UnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' is not a BnfiTermNonTerminal.",
                        obj, obj.GetType().Name, mainChildBnfTerm.Name));
                }

                int? priority = 0 - mainChildBnfTermWithType.Type.GetInheritanceDistance(obj);

                Unparser.tsPriorities.Indent();
                priority.DebugWriteLinePriority(Unparser.tsPriorities, mainChildBnfTerm, obj);
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

    public partial class BnfiTermChoice<TType> : BnfiTermChoice, IBnfiTerm<TType>, IBnfiTermOrAbleForChoice<TType>
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

        public BnfiExpression RuleTypeless { set { base.Rule = value; } }

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
