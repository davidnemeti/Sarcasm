using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Diagnostics;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.Ast;
using System.Runtime.CompilerServices;

namespace Sarcasm.Unparsing
{
    public static class UnparserExtensions
    {
        public static string AsString(this IEnumerable<Utoken> utokens, Unparser unparser)
        {
            return string.Concat(utokens.Select(utoken => utoken.ToString(unparser.Formatting)));
        }

        public static async Task WriteToStreamAsync(this IEnumerable<Utoken> utokens, Stream stream, Unparser unparser)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (Utoken utoken in utokens)
                    await sw.WriteAsync(utoken.ToString(unparser.Formatting));
            }
        }

        public static void WriteToStream(this IEnumerable<Utoken> utokens, Stream stream, Unparser unparser)
        {
            //            utokens.WriteToStreamAsync(stream, unparser).Wait();
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (Utoken utoken in utokens)
                    sw.Write(utoken.ToString(unparser.Formatting));
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
        bool TryGetUtokensDirectly(IUnparser unparser, object obj, out IEnumerable<UtokenValue> utokens);
        IEnumerable<UnparsableObject> GetChildren(IList<BnfTerm> childBnfTerms, object obj, Unparser.Direction direction);
        int? GetChildrenPriority(IUnparser unparser, object obj, IEnumerable<UnparsableObject> children);
    }

    public interface IUnparser
    {
        int? GetPriority(UnparsableObject unparsableObject);
        IFormatProvider FormatProvider { get; }
    }

    internal interface IPostProcessHelper
    {
        Unparser.Direction Direction { get; }
        bool IndentEmptyLines { get; }
        Action<UnparsableObject> UnlinkChildFromChildPrevSiblingIfNotFullUnparse { get; }
    }

    public class UnparsableObject
    {
        #region Constants

        private static readonly object nonCalculatedObj = new object();
        private static readonly object thrownOutObj = new object();

        internal static readonly UnparsableObject NonCalculated = new UnparsableObject(null, nonCalculatedObj);
        internal static readonly UnparsableObject ThrownOut = new UnparsableObject(null, thrownOutObj);

        #endregion

        #region State

        public BnfTerm BnfTerm { get; private set; }
        public object Obj { get; private set; }

        private UnparsableObject parent = NonCalculated;
        private UnparsableObject leftMostChild = NonCalculated;
        private UnparsableObject rightMostChild = NonCalculated;
        private UnparsableObject leftSibling = NonCalculated;
        private UnparsableObject rightSibling = NonCalculated;

        #endregion

        #region Construction

        public UnparsableObject(BnfTerm bnfTerm, object obj)
        {
            this.BnfTerm = bnfTerm;
            this.Obj = obj;
            this.IsLeftSiblingNeededForDeferredCalculation = false;
        }

        #endregion

        #region Publics

        public UnparsableObject Parent
        {
            get { CheckIfValid(parent); return parent; }
            set { CheckIfNotThrownOut(parent); parent = value; }
        }

        public UnparsableObject LeftMostChild
        {
            get { CheckIfValid(leftMostChild); return leftMostChild; }
            set { CheckIfNotThrownOut(leftMostChild); leftMostChild = value; }
        }

        public UnparsableObject RightMostChild
        {
            get { CheckIfValid(rightMostChild); return rightMostChild; }
            set { CheckIfNotThrownOut(rightMostChild); rightMostChild = value; }
        }

        public UnparsableObject LeftSibling
        {
            get { CheckIfValid(leftSibling); return leftSibling; }
            set { CheckIfNotThrownOut(leftSibling); leftSibling = value; }
        }

        public UnparsableObject RightSibling
        {
            get { CheckIfValid(rightSibling); return rightSibling; }
            set { CheckIfNotThrownOut(rightSibling); rightSibling = value; }
        }

        public bool IsParentCalculated { get { return IsCalculated(parent); } }
        public bool IsLeftMostChildCalculated { get { return IsCalculated(leftMostChild); } }
        public bool IsRightMostChildCalculated { get { return IsCalculated(rightMostChild); } }
        public bool IsLeftSiblingCalculated { get { return IsCalculated(leftSibling); } }
        public bool IsRightSiblingCalculated { get { return IsCalculated(rightSibling); } }

        public bool IsLeftSiblingNeededForDeferredCalculation { get; set; }

        public void SetAsRoot()
        {
            Parent = null;
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
                ? string.Format("[bnfTerm: {0}, obj: {1}]", BnfTerm, Obj)
                : "<<NonCalculated>>";
        }

        #endregion

        #region Equality

        public bool Equals(UnparsableObject that)
        {
            return object.ReferenceEquals(this, that)
                ||
                !object.ReferenceEquals(that, null) &&
                this.BnfTerm == that.BnfTerm &&
                this.Obj == that.Obj;
        }

        public override bool Equals(object obj)
        {
            return obj is UnparsableObject && Equals((UnparsableObject)obj);
        }

        public override int GetHashCode()
        {
            return Util.GetHashCodeMulti(BnfTerm, Obj);
        }

        public static bool operator ==(UnparsableObject unparsableObject1, UnparsableObject unparsableObject2)
        {
            return object.ReferenceEquals(unparsableObject1, unparsableObject2) || !object.ReferenceEquals(unparsableObject1, null) && unparsableObject1.Equals(unparsableObject2);
        }

        public static bool operator !=(UnparsableObject unparsableObject1, UnparsableObject unparsableObject2)
        {
            return !(unparsableObject1 == unparsableObject2);
        }

        #endregion

        #region Helpers

        private void CheckIfValid(UnparsableObject relative, [CallerMemberName] string nameOfRelative = "")
        {
            if (!IsCalculated(relative))
                throw new NonCalculatedException(string.Format("Tried to use a non-calculated relative '{0}' for {1}", nameOfRelative, this));
            else if (IsThrownOut(relative))
                throw new ThrownOutException(string.Format("Tried to use a thrown out relative '{0}' for {1}", nameOfRelative, this));
        }

        private void CheckIfNotThrownOut(UnparsableObject relative, [CallerMemberName] string nameOfRelative = "")
        {
            if (IsThrownOut(relative))
                throw new ThrownOutException(string.Format("Tried to set a thrown out relative '{0}' for {1}", nameOfRelative, this));
        }

        internal static bool IsCalculated(UnparsableObject unparsableObject)
        {
            return !object.ReferenceEquals(unparsableObject, NonCalculated);
        }

        private static bool IsThrownOut(UnparsableObject unparsableObject)
        {
            return object.ReferenceEquals(unparsableObject, ThrownOut);
        }

        #endregion
    }
}
