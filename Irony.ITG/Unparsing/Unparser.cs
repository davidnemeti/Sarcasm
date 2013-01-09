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
using Irony.ITG;
using Irony.ITG.Ast;

using Grammar = Irony.ITG.Ast.Grammar;

namespace Irony.ITG.Unparsing
{
    public class Unparser : IUnparser
    {
        internal const string logDirectoryName = "unparse_logs";

        internal readonly static TraceSource tsUnparse = new TraceSource("UNPARSE", SourceLevels.Verbose);

#if DEBUG
        static Unparser()
        {
            Directory.CreateDirectory(logDirectoryName);

            Trace.AutoFlush = true;

            tsUnparse.Listeners.Clear();
            tsUnparse.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(logDirectoryName, "00_unparse.log"))));
        }
#endif

        public Grammar Grammar { get; private set; }
        public Formatting Formatting { get { return Grammar.Formatting; } }
        private readonly Formatter formatter;

        public Unparser(Grammar grammar)
        {
            this.Grammar = grammar;
            this.formatter = new Formatter(grammar.Formatting);
        }

        public IEnumerable<Utoken> Unparse(object obj, Context context = null)
        {
            BnfTerm bnfTerm = GetBnfTerm(obj, context);
            return Unparse(obj, bnfTerm)
                .ConvertToUtokens(this);
        }

        public IEnumerable<Utoken> Unparse(object obj, BnfTerm bnfTerm)
        {
            return UnparseRaw(obj, bnfTerm)
                .Cook()
                .ConvertToUtokens(this);
        }

        bool steppedIntoUnparseRaw = false;

        private IEnumerable<Utoken> UnparseRaw(object obj, BnfTerm bnfTerm)
        {
            foreach (var utoken in formatter.Begin(bnfTerm))
                yield return utoken;

            steppedIntoUnparseRaw = true;

            if (bnfTerm is KeyTerm)
            {
                Unparser.tsUnparse.Debug("keyterm: [{0}]", ((KeyTerm)bnfTerm).Text);
                yield return ((KeyTerm)bnfTerm).Text;
            }
            else if (bnfTerm is Terminal)
            {
                Unparser.tsUnparse.Debug("terminal: [\"{0}\"]", obj.ToString());
                yield return obj.ToString();
            }
            else if (bnfTerm is NonTerminal)
            {
                Unparser.tsUnparse.Debug("nonterminal: {0}", bnfTerm.Name);

                Unparser.tsUnparse.Indent();

                NonTerminal nonTerminal = (NonTerminal)bnfTerm;
                IUnparsable unparsable = nonTerminal as IUnparsable;

                if (unparsable == null)
                    throw new CannotUnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' is not IUnparsable.", obj, obj.GetType().Name, nonTerminal.Name));

                steppedIntoUnparseRaw = false;

                foreach (Utoken utoken in unparsable.Unparse(this, obj))
                    yield return utoken;

                if (!steppedIntoUnparseRaw)
                    Unparser.tsUnparse.Debug("utokenized: [\"{0}\"]", obj.ToString());

                steppedIntoUnparseRaw = true;

                Unparser.tsUnparse.Unindent();
            }

            foreach (var utoken in formatter.End(bnfTerm))
                yield return utoken;
        }

        internal static IEnumerable<BnfTermList> GetChildBnfTermLists(NonTerminal nonTerminal)
        {
            return nonTerminal.Productions.Select(production => production.RValues);
        }

        private BnfTerm GetBnfTerm(object obj, Context context)
        {
            return Grammar.Root;
            // TODO: do this by choosing by context
        }

        #region IUnparser implementations

        IEnumerable<Utoken> IUnparser.Unparse(object obj, BnfTerm bnfTerm)
        {
            return UnparseRaw(obj, bnfTerm);
        }

        #region Error handling

        Error error = Error.None;
        string errorMessage;

        void IUnparser.RaiseError(Error error, string message)
        {
            this.error = error;
            this.errorMessage = message;
        }

        void IUnparser.ClearError()
        {
            this.error = Error.None;
            this.errorMessage = null;
        }

        bool IUnparser.HasError()
        {
            return this.error != Error.None;
        }

        Error IUnparser.GetError()
        {
            return this.error;
        }

        [DebuggerStepThrough()]
        void IUnparser.AssumeNoError()
        {
            if (((IUnparser)this).HasError())
                ((IUnparser)this).ThrowUnhandledErrorException(error, errorMessage);
        }

        [DebuggerStepThrough()]
        void IUnparser.ThrowUnhandledErrorException(Error error, string message)
        {
            throw new UnhandledInternalUnparseErrorException(error, errorMessage);
        }

        #endregion

        #endregion
    }

    public enum Error { None, ValueMismatch }

    public static class UnparserExtensions
    {
        public static string AsString(this IEnumerable<Utoken> utokens, Unparser unparser)
        {
            return string.Concat(utokens.Select(utoken => utoken.ToString(unparser.Formatting)));
        }

        public static async void WriteToStreamAsync(this IEnumerable<Utoken> utokens, Stream stream, Unparser unparser)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (Utoken utoken in utokens)
                    await sw.WriteAsync(utoken.ToString(unparser.Formatting));
            }
        }

        internal static IEnumerable<Utoken> Cook(this IEnumerable<Utoken> utokens)
        {
            return Formatter.PostProcess(utokens);
        }

        internal static Utokens ConvertToUtokens(this IEnumerable<Utoken> utokens, IUnparser unparser)
        {
            return new Utokens(utokens, unparser);
        }
    }

    public class Context
    {
        public MemberInfo MemberInfo { get; private set; }

        public Context(MemberInfo memberInfo)
        {
            this.MemberInfo = memberInfo;
        }
    }

    public interface IUnparsable
    {
        IEnumerable<Utoken> Unparse(IUnparser unparser, object obj);
    }

    public interface IUnparser
    {
        IEnumerable<Utoken> Unparse(object obj, BnfTerm bnfTerm);

        #region Error handling

		void RaiseError(Error error, string message);
        void ClearError();
        bool HasError();
        Error GetError();
        void AssumeNoError();
        void ThrowUnhandledErrorException(Error error, string message);

    	#endregion
    }

    public static class IUnparserExtensions
    {
        public static void RaiseError(this IUnparser unparser, Error error, string format, params object[] args)
        {
            unparser.RaiseError(error, string.Format(format, args));
        }
    }

    public class Utokens : IEnumerable<Utoken>
    {
        private readonly IEnumerable<Utoken> utokens;
        private readonly IUnparser unparser;

        public Utokens(IEnumerable<Utoken> utokens, IUnparser unparser)
        {
            this.utokens = utokens;
            this.unparser = unparser;
        }

        public IEnumerator<Utoken> GetEnumerator()
        {
            return new UtokensEnumerator(utokens.GetEnumerator(), unparser);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class UtokensEnumerator : IEnumerator<Utoken>
    {
        private readonly IEnumerator<Utoken> utokenEnumerator;
        private readonly IUnparser unparser;

        public UtokensEnumerator(IEnumerator<Utoken> utokenEnumerator, IUnparser unparser)
        {
            this.utokenEnumerator = utokenEnumerator;
            this.unparser = unparser;
        }

        public void Dispose()
        {
            utokenEnumerator.Dispose();
            unparser.AssumeNoError();   // this will throw exception if there are any errors left in the unparser
        }

        public Utoken Current
        {
            get { return utokenEnumerator.Current; }
        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            return utokenEnumerator.MoveNext();
        }

        public void Reset()
        {
            utokenEnumerator.Reset();
        }
    }
}
