using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.GrammarAst;
using Sarcasm.Utility;

namespace Sarcasm.Unparsing
{
    public static class UnparserExtensions
    {
        public static string AsText(this IEnumerable<Utoken> utokens, Unparser unparser)
        {
            return string.Concat(utokens.Select(utoken => utoken.ToText(unparser.Formatter)));
        }

        public static async Task WriteToStreamAsync(this IEnumerable<Utoken> utokens, Stream stream, Unparser unparser)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (Utoken utoken in utokens)
                    await sw.WriteAsync(utoken.ToText(unparser.Formatter));
            }
        }

        public static void WriteToStream(this IEnumerable<Utoken> utokens, Stream stream, Unparser unparser)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (Utoken utoken in utokens)
                    sw.Write(utoken.ToText(unparser.Formatter));
            }
        }

        internal static IEnumerable<Utoken> Cook(this IEnumerable<UtokenBase> utokens, IPostProcessHelper postProcessHelper)
        {
            return Formatter.PostProcess(utokens, postProcessHelper);
        }
    }

    public delegate IEnumerable<UtokenValue> ValueUtokenizer<T>(IFormatProvider formatProvider, T obj);

    public interface IUnparsableNonTerminal : INonTerminal
    {
        bool TryGetUtokensDirectly(IUnparser unparser, object astValue, out IEnumerable<UtokenValue> utokens);
        IEnumerable<UnparsableAst> GetChildren(IList<BnfTerm> childBnfTerms, object astValue, Unparser.Direction direction);
        int? GetChildrenPriority(IUnparser unparser, object astValue, IEnumerable<UnparsableAst> children);
    }

    public interface IUnparser
    {
        int? GetPriority(UnparsableAst unparsableAst);
        IFormatProvider FormatProvider { get; }
    }

    internal interface IPostProcessHelper
    {
        Formatter Formatter { get; }
        Unparser.Direction Direction { get; }
        Action<UnparsableAst> UnlinkChildFromChildPrevSiblingIfNotFullUnparse { get; }
    }

    public class UnparsableAst
    {
        #region Constants

        private static readonly object nonCalculatedAstValue = new object();
        private static readonly object thrownOutAstValue = new object();

        internal static readonly UnparsableAst NonCalculated = new UnparsableAst(null, nonCalculatedAstValue);
        internal static readonly UnparsableAst ThrownOut = new UnparsableAst(null, thrownOutAstValue);

        #endregion

        #region State

        public BnfTerm BnfTerm { get; private set; }
        public object AstValue { get; private set; }
        public Member ParentMember { get; internal set; }

        private UnparsableAst syntaxParent = NonCalculated;
        private UnparsableAst astParent = NonCalculated;
        private UnparsableAst leftMostChild = NonCalculated;
        private UnparsableAst rightMostChild = NonCalculated;
        private UnparsableAst leftSibling = NonCalculated;
        private UnparsableAst rightSibling = NonCalculated;

        #endregion

        #region Construction

        public UnparsableAst(BnfTerm bnfTerm, object astValue, Member parentMember = null)
        {
            this.BnfTerm = bnfTerm;
            this.AstValue = astValue;
            this.ParentMember = parentMember;

            this.IsLeftSiblingNeededForDeferredCalculation = false;
        }

        #endregion

        #region Publics

        public UnparsableAst SyntaxParent
        {
            get { CheckIfValid(syntaxParent); return syntaxParent; }
            set { CheckIfNotThrownOut(syntaxParent); syntaxParent = value; }
        }

        public UnparsableAst AstParent
        {
            get
            {
                CheckIfNotThrownOut(astParent);     // NOTE: special handling for AstParent getter, that's why it's CheckIfNotThrownOut and not CheckIfValid

                return astParent != NonCalculated
                    ? astParent
                    : Util.RecurseStopBeforeNull(this, unparsableAst => unparsableAst.SyntaxParent).FirstOrDefault(unparsableAst => unparsableAst.AstValue != this.AstValue);
            }
            set { CheckIfNotThrownOut(astParent); astParent = value; }
        }

        public UnparsableAst LeftMostChild
        {
            get { CheckIfValid(leftMostChild); return leftMostChild; }
            set { CheckIfNotThrownOut(leftMostChild); leftMostChild = value; }
        }

        public UnparsableAst RightMostChild
        {
            get { CheckIfValid(rightMostChild); return rightMostChild; }
            set { CheckIfNotThrownOut(rightMostChild); rightMostChild = value; }
        }

        public UnparsableAst LeftSibling
        {
            get { CheckIfValid(leftSibling); return leftSibling; }
            set { CheckIfNotThrownOut(leftSibling); leftSibling = value; }
        }

        public UnparsableAst RightSibling
        {
            get { CheckIfValid(rightSibling); return rightSibling; }
            set { CheckIfNotThrownOut(rightSibling); rightSibling = value; }
        }

        public bool IsSyntaxParentCalculated { get { return IsCalculated(syntaxParent); } }
        public bool IsAstParentCalculated { get { return IsCalculated(astParent); } }
        public bool IsLeftMostChildCalculated { get { return IsCalculated(leftMostChild); } }
        public bool IsRightMostChildCalculated { get { return IsCalculated(rightMostChild); } }
        public bool IsLeftSiblingCalculated { get { return IsCalculated(leftSibling); } }
        public bool IsRightSiblingCalculated { get { return IsCalculated(rightSibling); } }

        public bool IsLeftSiblingNeededForDeferredCalculation { get; set; }

        public void SetAsRoot()
        {
            SyntaxParent = AstParent = null;
            LeftSibling = null;
            RightSibling = null;
        }

        public void SetAsLeave()
        {
            LeftMostChild = null;
            RightMostChild = null;
        }

        public override string ToString()
        {
            return IsCalculated(this)
                ? string.Format("[bnfTerm: {0}, obj: {1}]", BnfTerm, AstValue)
                : "<<NonCalculated>>";
        }

        #endregion

        #region Equality

        public bool Equals(UnparsableAst that)
        {
            return object.ReferenceEquals(this, that)
                ||
                !object.ReferenceEquals(that, null) &&
                this.BnfTerm == that.BnfTerm &&
                this.AstValue == that.AstValue;
        }

        public override bool Equals(object obj)
        {
            return obj is UnparsableAst && Equals((UnparsableAst)obj);
        }

        public override int GetHashCode()
        {
            return Util.GetHashCodeMulti(BnfTerm, AstValue);
        }

        public static bool operator ==(UnparsableAst unparsableAst1, UnparsableAst unparsableAst2)
        {
            return object.ReferenceEquals(unparsableAst1, unparsableAst2) || !object.ReferenceEquals(unparsableAst1, null) && unparsableAst1.Equals(unparsableAst2);
        }

        public static bool operator !=(UnparsableAst unparsableAst1, UnparsableAst unparsableAst2)
        {
            return !(unparsableAst1 == unparsableAst2);
        }

        #endregion

        #region Helpers

        private void CheckIfValid(UnparsableAst relative, [CallerMemberName] string nameOfRelative = "")
        {
            if (!IsCalculated(relative))
                throw new NonCalculatedException(string.Format("Tried to use a non-calculated relative '{0}' for {1}", nameOfRelative, this));
            else if (IsThrownOut(relative))
                throw new ThrownOutException(string.Format("Tried to use a thrown out relative '{0}' for {1}", nameOfRelative, this));
        }

        private void CheckIfNotThrownOut(UnparsableAst relative, [CallerMemberName] string nameOfRelative = "")
        {
            if (IsThrownOut(relative))
                throw new ThrownOutException(string.Format("Tried to set a thrown out relative '{0}' for {1}", nameOfRelative, this));
        }

        internal static bool IsCalculated(UnparsableAst unparsableAst)
        {
            return !object.ReferenceEquals(unparsableAst, NonCalculated);
        }

        private static bool IsThrownOut(UnparsableAst unparsableAst)
        {
            return object.ReferenceEquals(unparsableAst, ThrownOut);
        }

        #endregion
    }
}
