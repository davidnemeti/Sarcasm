using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Irony.Parsing;

namespace Sarcasm.Publicizing
{
    internal static class PublicizerExtensions
    {
        private static readonly Func<StringLiteral, IReadOnlyList<object>> _GetField__subtypes =
            MemberInfoHelpers.CreateGetFuncByExpression<StringLiteral, IReadOnlyList<object>>(
                typeof(StringLiteral).GetField("_subtypes", BindingFlags.Instance | BindingFlags.NonPublic)
            );

        public static IEnumerable<StringSubTypeProxy> GetPrivate_subtypes(this StringLiteral stringLiteral) =>
            _GetField__subtypes(stringLiteral).Select(stringSubType => new StringSubTypeProxy(stringSubType));
    }
}
