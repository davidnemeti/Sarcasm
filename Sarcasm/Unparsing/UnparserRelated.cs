#region License
/*
    This file is part of Sarcasm.

    Copyright 2012-2013 Dávid Németi

    Sarcasm is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Sarcasm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Sarcasm.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

using Sarcasm.GrammarAst;
using System.Threading;

namespace Sarcasm.Unparsing
{
    public delegate IEnumerable<UtokenValue> ValueUtokenizer<in T>(IFormatProvider formatProvider, UnparsableAst reference, T astValue);

    public interface IUnparsableNonTerminal : INonTerminal
    {
        bool TryGetUtokensDirectly(IUnparser unparser, UnparsableAst self, out IEnumerable<UtokenValue> utokens);
        IEnumerable<UnparsableAst> GetChildren(Unparser.ChildBnfTerms childBnfTerms, object astValue, Unparser.Direction direction);
        int? GetChildrenPriority(IUnparser unparser, object astValue, Unparser.Children children, Unparser.Direction direction);
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

    public static class UnparserExtensions
    {
        public static string AsText(this IEnumerable<Utoken> utokens, Unparser unparser)
        {
            return utokens.AsText(unparser.Formatter);
        }

        public static string AsText(this IEnumerable<Utoken> utokens, Formatter formatter)
        {
            return string.Concat(utokens.Select(utoken => utoken.ToText(formatter)));
        }

        public static void WriteToStream(this IEnumerable<Utoken> utokens, Stream stream, Unparser unparser)
        {
            utokens.WriteToStream(stream, unparser.Formatter);
        }

        public static void WriteToStream(this IEnumerable<Utoken> utokens, Stream stream, Formatter formatter)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (Utoken utoken in utokens)
                    sw.Write(utoken.ToText(formatter));
            }
        }

        public static async Task WriteToStreamAsync(this IEnumerable<Utoken> utokens, Stream stream, Unparser unparser)
        {
            await utokens.WriteToStreamAsync(stream, unparser, CancellationToken.None);
        }

        public static async Task WriteToStreamAsync(this IEnumerable<Utoken> utokens, Stream stream, Unparser unparser, CancellationToken cancellationToken)
        {
            using (await unparser.Lock.LockAsync())
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    foreach (Utoken utoken in utokens)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        await sw.WriteAsync(utoken.ToText(unparser.Formatter));
                    }
                }
            }
        }

        internal static IEnumerable<Utoken> Cook(this IEnumerable<UtokenBase> utokens, IPostProcessHelper postProcessHelper)
        {
            return FormatYielder.PostProcess(utokens, postProcessHelper);
        }
    }
}
