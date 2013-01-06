using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Irony.ITG;
using Irony.ITG.Ast;

using Grammar = Irony.ITG.Ast.Grammar;

namespace Irony.ITG.Unparsing
{
    internal class Formatter
    {
        private enum State { Begin, End }

        private readonly Formatting formatting;

        IList<BnfTerm> leftBnfTermsFromTopToBottom = new List<BnfTerm>();
        State lastState = State.Begin;

        public Formatter(Formatting formatting)
        {
            this.formatting = formatting;
        }

        public IEnumerable<Utoken> Begin(BnfTerm bnfTerm)
        {
            lastState = State.Begin;

            foreach (BnfTerm leftBnfTerm in leftBnfTermsFromTopToBottom)
            {
                InsertedUtokens insertedUtokensBetween;
                if (formatting.HasUtokensBetween(leftBnfTerm, bnfTerm, out insertedUtokensBetween))
                    yield return insertedUtokensBetween;
            }

            InsertedUtokens insertedUtokensBefore;
            if (formatting.HasUtokensBefore(bnfTerm, out insertedUtokensBefore))
                yield return insertedUtokensBefore;
        }

        public IEnumerable<Utoken> End(BnfTerm bnfTerm)
        {
            if (lastState != State.End)
                leftBnfTermsFromTopToBottom.Clear();

            if (formatting.IsLeftBnfTermOfABetweenPair(bnfTerm))
                leftBnfTermsFromTopToBottom.Insert(0, bnfTerm);  // for efficiency reasons we only store bnfTerm as leftBnfTerm if it is really a leftBnfTerm

            lastState = State.End;

            InsertedUtokens insertedUtokensAfter;
            if (formatting.HasUtokensAfter(bnfTerm, out insertedUtokensAfter))
                yield return insertedUtokensAfter;
        }

        public static IEnumerable<Utoken> PostProcess(IEnumerable<Utoken> utokens)
        {
            return utokens
                .FilterInsertedUtokens()
                .Flatten()
                .ProcessControls();
        }
    }

    internal static class FormatterExtensions
    {
        #region FilterInsertedUtokens

        /*
         * utokens contains several InsertedUtokens "sessions" which consist of "After", "Before" and "Between" InsertedUtokens.
         * A session looks like this: (After)*((Between)?(Before)?)*
         * 
         * Note that "Between" and "Before" InsertedUtokens are mixed with each other.
         * 
         * "After" InsertedUtokens are handled so that in case of equal priority we will choose the right one.
         * "Between" InsertedUtokens are handled so that in case of equal priority we will choose the left one.
         * "Before" InsertedUtokens are handled so that in case of equal priority we will choose the left one.
         * 
         * Since "Between" and "Before" InsertedUtokens are handled in the same way, we can handle them mixed.
         * */

        public static IEnumerable<Utoken> FilterInsertedUtokens(this IEnumerable<Utoken> utokens)
        {
            InsertedUtokens leftInsertedUtokensToBeYield = null;

            foreach (Utoken utoken in utokens)
            {
                if (utoken is InsertedUtokens)
                {
                    InsertedUtokens rightInsertedUtokens = (InsertedUtokens)utoken;

                    bool switchToNewInsertedUtokens = SwitchToRight(leftInsertedUtokensToBeYield, rightInsertedUtokens);

                    if (rightInsertedUtokens.overridable)
                        leftInsertedUtokensToBeYield = switchToNewInsertedUtokens ? rightInsertedUtokens : leftInsertedUtokensToBeYield;
                    else
                    {
                        if (!switchToNewInsertedUtokens)
                            yield return leftInsertedUtokensToBeYield;

                        yield return rightInsertedUtokens;
                        leftInsertedUtokensToBeYield = null;
                    }
                }
                else
                {
                    if (leftInsertedUtokensToBeYield != null)
                        yield return leftInsertedUtokensToBeYield;

                    yield return utoken;
                }
            }

            if (leftInsertedUtokensToBeYield != null)
                yield return leftInsertedUtokensToBeYield;
        }

        private static bool SwitchToRight(InsertedUtokens leftInsertedUtokens, InsertedUtokens rightInsertedUtokens)
        {
            int compareResult = InsertedUtokens.Compare(leftInsertedUtokens, rightInsertedUtokens);

            if (compareResult == 0)
                return leftInsertedUtokens.kind == InsertedUtokens.Kind.After;
            else
                return compareResult < 0;
        }

        #endregion

        public static IEnumerable<Utoken> Flatten(this IEnumerable<Utoken> utokens)
        {
            return utokens.SelectMany(utoken => utoken.Flatten());
        }

        public static IEnumerable<Utoken> ProcessControls(this IEnumerable<Utoken> utokens)
        {
            int indentLevel = 0;
            foreach (Utoken utoken in utokens)
            {
                if (utoken is UtokenControl)
                {
                    UtokenControl utokenControl = (UtokenControl)utoken;

//                    Utoken.CreateIndent(indentLevel);
                }
                else
                    yield return utoken;
            }
        }
    }
}
