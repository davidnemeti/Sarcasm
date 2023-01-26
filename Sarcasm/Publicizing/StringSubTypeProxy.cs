using System;
using System.Reflection;
using Irony.Parsing;

namespace Sarcasm.Publicizing
{
    internal class StringSubTypeProxy
    {
        private static readonly Type _StringSubType_Type = typeof(StringLiteral).GetNestedType("StringSubType", BindingFlags.NonPublic);

        private static readonly Func<object, string> _GetField_Start =
            MemberInfoHelpers.CreateGetFuncByExpression<object, string>(_StringSubType_Type.GetField("Start", BindingFlags.Instance | BindingFlags.Public));

        private static readonly Func<object, string> _GetField_End =
            MemberInfoHelpers.CreateGetFuncByExpression<object, string>(_StringSubType_Type.GetField("End", BindingFlags.Instance | BindingFlags.Public));

        private static readonly Func<object, StringOptions> _GetField_Flags =
            MemberInfoHelpers.CreateGetFuncByExpression<object, StringOptions>(_StringSubType_Type.GetField("Flags", BindingFlags.Instance | BindingFlags.Public));

        private static readonly Func<object, byte> _GetField_Index =
            MemberInfoHelpers.CreateGetFuncByExpression<object, byte>(_StringSubType_Type.GetField("Index", BindingFlags.Instance | BindingFlags.Public));

        public string Start => _GetField_Start(_stringSubType);
        public string End => _GetField_End(_stringSubType);
        public StringOptions Flags => _GetField_Flags(_stringSubType);
        public byte Index => _GetField_Index(_stringSubType);

        private readonly object _stringSubType;

        public StringSubTypeProxy(object stringSubType)
        {
            _stringSubType = stringSubType;
        }
    }
}
