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

        internal static IEnumerable<Utoken> Cook(this IEnumerable<UtokenBase> utokens)
        {
            return Formatter.PostProcess(utokens);
        }
    }

    public delegate IEnumerable<UtokenValue> ValueUtokenizer<T>(IFormatProvider formatProvider, T obj);

    public interface IUnparsableNonTerminal : INonTerminal
    {
        bool TryGetUtokensDirectly(IUnparser unparser, object obj, out IEnumerable<UtokenValue> utokens);
        IEnumerable<UnparsableObject> GetChildren(BnfTermList childBnfTerms, object obj);
        int? GetChildrenPriority(IUnparser unparser, object obj, IEnumerable<UnparsableObject> children);
    }

    public interface IUnparser
    {
        int? GetPriority(UnparsableObject unparsableObject);
        IFormatProvider FormatProvider { get; }
    }

    public class UnparsableObject
    {
        public BnfTerm BnfTerm { get; private set; }
        public object Obj { get; private set; }

        public UnparsableObject(BnfTerm bnfTerm, object obj)
        {
            this.BnfTerm = bnfTerm;
            this.Obj = obj;
        }

        public bool Equals(UnparsableObject that)
        {
            return object.ReferenceEquals(this, that)
                ||
                that != null &&
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

        public override string ToString()
        {
            return string.Format("[bnfTerm: {0}, obj: {1}]", BnfTerm, Obj);
        }
    }
}
