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
using Irony.ITG;
using Irony.ITG.Unparsing;

namespace Irony.ITG.Ast
{
    public partial class BnfiTermChoice : BnfiTermNonTerminal, IBnfiTerm, IUnparsable
    {
        public BnfiTermChoice(Type type, string errorAlias = null)
            : base(type, errorAlias)
        {
            GrammarHelper.MarkTransient(this);      // the child node already contains the created ast node
        }

        public new BnfiExpressionChoice Rule { set { base.Rule = value; } }

        public BnfExpression RuleRaw { get { return base.Rule; } set { base.Rule = value; } }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
        }

        public IEnumerable<Utoken> Unparse(IUnparser unparser, object obj)
        {
            BnfTermListToPriority bnfTermListToPriority;
            if (obj.GetType() == this.Type)
            {
                bnfTermListToPriority = (BnfTermList bnfTerms, out object outObj, out ICollection<Utoken> preYieldedUtokens) =>
                {
                    outObj = obj;

                    try
                    {
                        // we need to do the full unparse non-lazy in order to catch ValueMismatchException (that's why we use ToList here)
                        preYieldedUtokens = bnfTerms.SelectMany(bnfTerm => unparser.Unparse(obj, bnfTerm)).ToList();
                        return int.MaxValue;
                    }
                    catch (ValueMismatchException)
                    {
                        preYieldedUtokens = null;
                        return null;
                    }
                };
            }
            else
            {
                bnfTermListToPriority = (BnfTermList bnfTerms, out object outObj, out ICollection<Utoken> preYieldedUtokens) =>
                {
                    outObj = obj;
                    preYieldedUtokens = null;

                    BnfTerm bnfTermCandidate = bnfTerms.Single(bnfTerm => !bnfTerm.Flags.IsSet(TermFlags.IsPunctuation) && !(bnfTerm is GrammarHint));
                    IHasType bnfTermCandidateWithType = bnfTermCandidate as IHasType;

                    if (bnfTermCandidateWithType == null)
                        throw new CannotUnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' is not a BnfiTermNonTerminal.", obj, obj.GetType().Name, bnfTermCandidate.Name));

                    return -bnfTermCandidateWithType.Type.GetInheritanceDistance(obj);
                };
            }

            return Unparser.UnparseBestChildBnfTermList(this, unparser, obj, bnfTermListToPriority);
        }
    }

    public partial class BnfiTermChoice<TType> : BnfiTermChoice, IBnfiTerm<TType>
    {
        public BnfiTermChoice(string errorAlias = null)
            : base(typeof(TType), errorAlias)
        {
        }

        [Obsolete(typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public BnfiExpressionChoice RuleTypeless { set { base.Rule = value; } }

        public new BnfiExpressionChoice<TType> Rule { set { base.Rule = value; } }

        public void SetRuleOr(IBnfiTerm<TType> bnfiTermFirst, params IBnfiTerm<TType>[] bnfiTerms)
        {
            this.Rule = Or(bnfiTermFirst, bnfiTerms);
        }

        public BnfiExpressionChoice<TType> Or(IBnfiTerm<TType> bnfiTermFirst, params IBnfiTerm<TType>[] bnfiTerms)
        {
            return (BnfiExpressionChoice<TType>)bnfiTerms
                .Select(bnfiTerm => bnfiTerm.AsBnfTerm())
                .Aggregate(
                new BnfExpression(bnfiTermFirst.AsBnfTerm()),
                (bnfExpressionProcessed, bnfTermToBeProcess) => bnfExpressionProcessed | bnfTermToBeProcess
                );
        }
    }
}
