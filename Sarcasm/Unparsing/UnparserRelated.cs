﻿using System;
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

        internal static IEnumerable<Utoken> Cook(this IEnumerable<Utoken> utokens)
        {
            return Formatter.PostProcess(utokens);
        }
    }

    public delegate IEnumerable<Utoken> ValueUtokenizer<T>(IFormatProvider formatProvider, T obj);

    public class Context
    {
        public MemberInfo MemberInfo { get; private set; }

        public Context(MemberInfo memberInfo)
        {
            this.MemberInfo = memberInfo;
        }
    }

    public interface IUnparsable : INonTerminal
    {
        bool TryGetUtokensDirectly(IUnparser unparser, object obj, out IEnumerable<Utoken> utokens);
        IEnumerable<UnparsableObject> GetChildUnparsableObjects(BnfTermList childBnfTerms, object obj);
        int? GetChildBnfTermListPriority(IUnparser unparser, object obj, IEnumerable<UnparsableObject> childUnparsableObjects);
    }

    public interface IUnparser
    {
        int? GetBnfTermPriority(BnfTerm bnfTerm, object obj);
        IFormatProvider FormatProvider { get; }
    }

    public class UnparsableObject
    {
        public readonly BnfTerm bnfTerm;
        public readonly object obj;

        public UnparsableObject(BnfTerm bnfTerm, object obj)
        {
            this.bnfTerm = bnfTerm;
            this.obj = obj;
        }
    }
}