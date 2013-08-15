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
using System.Threading;

namespace Sarcasm.Unparsing
{
    public delegate IEnumerable<UtokenValue> ValueUtokenizer<in TD>(IFormatProvider formatProvider, UnparsableAst reference, TD astValue);

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
        public static string AsText(this IEnumerable<Utoken> utokens)
        {
            return string.Concat(utokens.Select(utoken => utoken.Text));
        }

#if NET4_0
        public static Task<string> AsTextAsync(this IEnumerable<Utoken> utokens)
        {
            return GetUnparserAsyncLock(utokens).LockAsync()
                .ContinueWith(task => { using (task.Result) return TaskEx.Run(() => utokens.AsText()); })
                .Unwrap();
        }

        public static Task<string> AsTextAsync(this IEnumerable<Utoken> utokens, CancellationToken cancellationToken)
        {
            return GetUnparserAsyncLock(utokens).LockAsync()
                .ContinueWith(
                    task =>
                    {
                        using (task.Result)
                            return TaskEx.Run(
                                () =>
                                    string.Concat(
                                        utokens.Select(
                                            utoken =>
                                            {
                                                cancellationToken.ThrowIfCancellationRequested();
                                                return utoken.Text;
                                            }
                                        )
                                    )
                                ,
                                cancellationToken
                            );
                    },
                    cancellationToken
                )
                .Unwrap();
        }
#else
        public static async Task<string> AsTextAsync(this IEnumerable<Utoken> utokens)
        {
            using (await GetUnparserAsyncLock(utokens).LockAsync())
                return await Task.Run(() => utokens.AsText());
        }

        public static async Task<string> AsTextAsync(this IEnumerable<Utoken> utokens, CancellationToken cancellationToken)
        {
            using (await GetUnparserAsyncLock(utokens).LockAsync())
            {
                return await Task.Run(
                    () =>
                    {
                        return string.Concat(
                            utokens.Select(
                                utoken =>
                                {
                                    cancellationToken.ThrowIfCancellationRequested();
                                    return utoken.Text;
                                }
                            )
                        );
                    },
                    cancellationToken
                );
            }
        }
#endif

        public static void WriteToStream(this IEnumerable<Utoken> utokens, Stream stream)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (Utoken utoken in utokens)
                    sw.Write(utoken.Text);
            }
        }

#if !NET4_0
        public static async Task WriteToStreamAsync(this IEnumerable<Utoken> utokens, Stream stream)
        {
            await utokens.WriteToStreamAsync(stream, CancellationToken.None);
        }

        public static async Task WriteToStreamAsync(this IEnumerable<Utoken> utokens, Stream stream, CancellationToken cancellationToken)
        {
            using (await GetUnparserAsyncLock(utokens).LockAsync())
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    foreach (Utoken utoken in utokens)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        await sw.WriteAsync(utoken.Text);
                    }
                }
            }
        }
#endif

        internal static IEnumerable<Utoken> Cook(this IEnumerable<UtokenBase> utokens, IPostProcessHelper postProcessHelper)
        {
            return Formatter.PostProcess(utokens, postProcessHelper);
        }

        private static AsyncLock GetUnparserAsyncLock(IEnumerable<Utoken> utokens)
        {
            // NOTE that utoken list might be empty
            // NOTE that utoken list might not come from the unparser (might be user created), so they might not contain an unparser
            var utoken = (UtokenBase)utokens.FirstOrDefault();
            return utoken != null && utoken.UnparserForAsyncLock != null ? utoken.UnparserForAsyncLock.Lock : new AsyncLock();
        }
    }
}
