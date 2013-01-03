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
using Irony.ITG.Unparsing;

namespace Irony.ITG.Ast
{
    public partial class BnfiTermTransient : BnfiTermNonTerminal, IBnfiTerm, IUnparsable
    {
        public BnfiTermTransient(Type type, string errorAlias = null)
            : base(type, errorAlias)
        {
            GrammarHelper.MarkTransient(this);      // the child node already contains the created ast node
        }

        public new BnfiExpressionTransient Rule { set { base.Rule = value; } }

        public BnfExpression RuleRaw { get { return base.Rule; } set { base.Rule = value; } }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
        }

        public IEnumerable<Utoken> Unparse(Unparser unparser, object obj)
        {
            foreach (BnfTermList childBnfTerms in Unparser.GetChildBnfTermLists(this))
            {
                BnfTerm childBnfTermCandidate = childBnfTerms.Single(bnfTerm => !bnfTerm.Flags.IsSet(TermFlags.IsPunctuation) && !(bnfTerm is GrammarHint));

                IEnumerable<Utoken> utokens;

                if (obj.GetType() == this.Type)
                {
                    try
                    {
                        // we need to do the full unparse non-lazy in order to catch ValueMismatchException (that's why we use ToList here)
                        utokens = unparser.Unparse(obj, childBnfTermCandidate).ToList();
                    }
                    catch (ValueMismatchException)
                    {
                        // it is okay, keep trying with the others...
                        continue;
                    }
                }
                else
                {
                    BnfiTermNonTerminal childBnfiTermCandidate = childBnfTermCandidate as BnfiTermNonTerminal;

                    if (childBnfiTermCandidate == null)
                        throw new CannotUnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' is not a BnfiTermNonTerminal.", obj, obj.GetType().Name, childBnfTermCandidate.Name));

                    if (!childBnfiTermCandidate.Type.IsInstanceOfType(obj))
                    {
                        // keep trying with the others...
                        continue;
                    }

                    utokens = unparser.Unparse(obj, childBnfiTermCandidate);
                }

                foreach (Utoken utoken in utokens)
                    yield return utoken;

                yield break;
            }
        }
    }

    public partial class BnfiTermTransient<TType> : BnfiTermTransient, IBnfiTerm<TType>
    {
        public BnfiTermTransient(string errorAlias = null)
            : base(typeof(TType), errorAlias)
        {
        }

        [Obsolete(typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public BnfiExpressionTransient RuleTypeless { set { base.Rule = value; } }

        public new BnfiExpressionTransient<TType> Rule { set { base.Rule = value; } }

        public void SetRuleOr(IBnfiTerm<TType> bnfiTermFirst, params IBnfiTerm<TType>[] bnfiTerms)
        {
            this.Rule = Or(bnfiTermFirst, bnfiTerms);
        }

        public BnfiExpressionTransient<TType> Or(IBnfiTerm<TType> bnfiTermFirst, params IBnfiTerm<TType>[] bnfiTerms)
        {
            return (BnfiExpressionTransient<TType>)bnfiTerms
                .Select(bnfiTerm => bnfiTerm.AsBnfTerm())
                .Aggregate(
                new BnfExpression(bnfiTermFirst.AsBnfTerm()),
                (bnfExpressionProcessed, bnfTermToBeProcess) => bnfExpressionProcessed | bnfTermToBeProcess
                );
        }
    }
}
