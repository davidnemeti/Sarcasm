using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;

using Sarcasm;
using Sarcasm.GrammarAst;
using Sarcasm.Unparsing;
using Sarcasm.Unparsing.Styles;
using Sarcasm.DomainCore;
using Sarcasm.Utility;
using Sarcasm.Reflection;
using Microsoft.CSharp.RuntimeBinder;

namespace Sarcasm.UniversalGrammars
{
    [Grammar(typeof(object), "JSON")]
    public class JsonGrammar : Sarcasm.GrammarAst.Grammar
    {
        #region BnfTerms

        public class BnfTerms
        {
            public readonly BnfiTermChoiceTL Object = new BnfiTermChoiceTL(typeof(object), "Object");
            public readonly BnfiTermRecord<MetaObject> MetaObject = new BnfiTermRecord<MetaObject>();
            public readonly BnfiTermRecord<MetaArray> MetaArray = new BnfiTermRecord<MetaArray>();
            public readonly BnfiTermCopyTL Array = new BnfiTermCopyTL(typeof(IEnumerable), "Array");
            public readonly BnfiTermConversion<Type> Type = new BnfiTermConversion<Type>();
            public readonly BnfiTermRecord<MemberValue> MemberValue = new BnfiTermRecord<MemberValue>();
            public readonly BnfiTermConversion<string> Key = new BnfiTermConversion<string>();
            public readonly BnfiTermConversionTL Value = new BnfiTermConversionTL("Value");

            public readonly BnfiTermKeyTermPunctuation OBJECT_BEGIN;
            public readonly BnfiTermKeyTermPunctuation OBJECT_END;
            public readonly BnfiTermKeyTermPunctuation ARRAY_BEGIN;
            public readonly BnfiTermKeyTermPunctuation ARRAY_END;
            public readonly BnfiTermKeyTermPunctuation COMMA;
            public readonly BnfiTermKeyTermPunctuation COLON;
            public readonly BnfiTermKeyTermPunctuation KEYWORD_QUOTE_MARK;

            public readonly BnfiTermKeyTerm TYPE_KEYWORD;
            public readonly BnfiTermKeyTerm COLLECTION_VALUES_KEYWORD;
            public readonly BnfiTermKeyTerm PRIMITIVE_VALUE_KEYWORD;

            public readonly BnfiTermNoAst Type_Key = new BnfiTermNoAst("Type_Key");
            public readonly BnfiTermNoAst CollectionValues_Key = new BnfiTermNoAst("CollectionValues_Key");
            public readonly BnfiTermNoAst PrimitiveValue_Key = new BnfiTermNoAst("PrimitiveValue_Key");

            public readonly BnfiTermConversionTL NUMBER;
            public readonly BnfiTermConversion<string> STRING;

            public readonly BnfiTermConstant<bool> BOOLEAN;
            public readonly BnfiTermConstantTL NULL;

            internal BnfTerms(TerminalFactoryS TerminalFactoryS)
            {
                this.OBJECT_BEGIN = TerminalFactoryS.CreatePunctuation("{");
                this.OBJECT_END = TerminalFactoryS.CreatePunctuation("}");
                this.ARRAY_BEGIN = TerminalFactoryS.CreatePunctuation("[");
                this.ARRAY_END = TerminalFactoryS.CreatePunctuation("]");
                this.COMMA = TerminalFactoryS.CreatePunctuation(",");
                this.COLON = TerminalFactoryS.CreatePunctuation(":");
                this.KEYWORD_QUOTE_MARK = TerminalFactoryS.CreatePunctuation("\"");

                this.TYPE_KEYWORD = TerminalFactoryS.CreateKeyTerm("$type");
                this.COLLECTION_VALUES_KEYWORD = TerminalFactoryS.CreateKeyTerm("$values");
                this.PRIMITIVE_VALUE_KEYWORD = TerminalFactoryS.CreateKeyTerm("$value");

                this.NUMBER = TerminalFactoryS.CreateNumberLiteral().MakeUncontractible();
                this.STRING = TerminalFactoryS.CreateStringLiteral(name: "stringliteral", startEndSymbol: "\"").MakeUncontractible();

                this.BOOLEAN = new BnfiTermConstant<bool>()
                {
                    { "true", true },
                    { "false", false }
                };

                this.NULL = new BnfiTermConstantTL()
                {
                    { "null", (object)null }
                };
            }
        }

        #endregion

        public readonly BnfTerms B;

        public JsonGrammar()
            : base(AstCreation.CreateAstWithAutoBrowsableAstNodes, EmptyCollectionHandling.ReturnEmpty, ErrorHandling.ThrowException)
        {
            B = new BnfTerms(new TerminalFactoryS(this));

            this.Root = B.Object;

            B.Object.Rule =
                B.NUMBER + SetUnparsePriority((astValue, childAstValues) => astValue is int || astValue is double ? (int?)1 : null)
                |
                B.STRING
                |
                B.BOOLEAN
                |
                B.MetaArray.ConvertValue(MetaArrayToArray, ArrayToMetaArray)
                |
                B.NULL + SetUnparsePriority((astValue, childAstValues) => astValue == null ? (int?)1 : null)
                |
                B.MetaObject.ConvertValue(MetaObjectToObject, ObjectToMetaObject)
                ;

            B.MetaObject.Rule =
                B.OBJECT_BEGIN
                + B.Type_Key
                + B.COLON
                + B.Type.BindTo(B.MetaObject, metaObject => metaObject.Type)
                + B.COMMA
                + B.PrimitiveValue_Key
                + B.COLON
                + B.STRING.BindTo(B.MetaObject, metaObject => metaObject.PrimitiveValueStr)
                + B.OBJECT_END
                |
                B.OBJECT_BEGIN
                + B.Type_Key
                + B.COLON
                + B.Type.BindTo(B.MetaObject, metaObject => metaObject.Type)
                + B.COMMA
                + B.MemberValue.StarList(B.COMMA).BindTo(B.MetaObject, metaObject => metaObject.MemberValues)
                + B.OBJECT_END
                ;

            B.Type.Rule =
                B.STRING.ConvertValue(typeNameWithAssembly => Type.GetType(typeNameWithAssembly), type => type.AssemblyQualifiedName)
                ;

            B.MemberValue.Rule =
                B.Key.BindTo(B.MemberValue, memberValue => memberValue.Name)
                + B.COLON
                + B.Value.BindTo_(B.MemberValue, memberValue => memberValue.Value)
                ;

            B.Key.Rule =
                B.STRING
                ;

            B.Value.Rule =
                B.Object.ConvertValue(obj => obj, value => value)
                ;

            B.MetaArray.Rule =
                B.OBJECT_BEGIN
                + B.Type_Key
                + B.COLON
                + B.Type.BindTo(B.MetaArray, metaArray => metaArray.Type)
                + B.COMMA
                + B.CollectionValues_Key
                + B.COLON
                + B.Array.BindTo(B.MetaArray, metaArray => metaArray.Elements)
                + B.OBJECT_END
                ;

            B.Array.Rule =
                B.ARRAY_BEGIN
                + B.Object.StarListTL(B.COMMA)
                + B.ARRAY_END
                ;

            B.Type_Key.Rule =
                B.KEYWORD_QUOTE_MARK
                + B.TYPE_KEYWORD
                + B.KEYWORD_QUOTE_MARK
                ;

            B.CollectionValues_Key.Rule =
                B.KEYWORD_QUOTE_MARK
                + B.COLLECTION_VALUES_KEYWORD
                + B.KEYWORD_QUOTE_MARK
                ;

            B.PrimitiveValue_Key.Rule =
                B.KEYWORD_QUOTE_MARK
                + B.PRIMITIVE_VALUE_KEYWORD
                + B.KEYWORD_QUOTE_MARK
                ;

            RegisterBracePair(B.OBJECT_BEGIN, B.OBJECT_END);
            RegisterBracePair(B.ARRAY_BEGIN, B.ARRAY_END);

            #region Unparse

            UnparseControl.DefaultFormatter = new Formatter(this);
            UnparseControl.NoPrecedenceBasedParenthesesNeededForExpressions();

            #endregion
        }

        #region Helpers

        private IEnumerable MetaArrayToArray(MetaArray metaArray)
        {
            try
            {
                dynamic array = (IEnumerable)Activator.CreateInstance(metaArray.Type);

                foreach (dynamic element in metaArray.Elements)
                    array.Add(element);

                return array;
            }
            catch (RuntimeBinderException)
            {
                throw new InvalidOperationException(GetErrorMessageFor_COLLECTION_VALUES_KEYWORD());
            }
            catch (InvalidCastException)
            {
                throw new InvalidOperationException(GetErrorMessageFor_COLLECTION_VALUES_KEYWORD());
            }
        }

        private MetaArray ArrayToMetaArray(IEnumerable array)
        {
            return new MetaArray()
            {
                Type = array.GetType(),
                Elements = array
            };
        }

        private object MetaObjectToObject(MetaObject metaObject)
        {
            Type type = metaObject.Type;
            string primitiveValueStr = metaObject.PrimitiveValueStr;

            if (primitiveValueStr != null)
            {
                #region Primitive

                if (type.IsEnum)
                    return Enum.Parse(type, primitiveValueStr);

                else if (type == typeof(Boolean))
                    return Boolean.Parse(primitiveValueStr);

                else if (type == typeof(Byte))
                    return Byte.Parse(primitiveValueStr, this.DefaultCulture);

                else if (type == typeof(SByte))
                    return SByte.Parse(primitiveValueStr, this.DefaultCulture);

                else if (type == typeof(Int16))
                    return Int16.Parse(primitiveValueStr, this.DefaultCulture);

                else if (type == typeof(Int32))
                    return Int32.Parse(primitiveValueStr, this.DefaultCulture);

                else if (type == typeof(Int64))
                    return Int64.Parse(primitiveValueStr, this.DefaultCulture);

                else if (type == typeof(UInt16))
                    return UInt16.Parse(primitiveValueStr, this.DefaultCulture);

                else if (type == typeof(UInt32))
                    return UInt32.Parse(primitiveValueStr, this.DefaultCulture);

                else if (type == typeof(UInt64))
                    return UInt64.Parse(primitiveValueStr, this.DefaultCulture);

                else if (type == typeof(Single))
                    return Single.Parse(primitiveValueStr, this.DefaultCulture);

                else if (type == typeof(Double))
                    return Double.Parse(primitiveValueStr, this.DefaultCulture);

                else if (type == typeof(Decimal))
                    return Decimal.Parse(primitiveValueStr, this.DefaultCulture);

                else if (type == typeof(Char))
                    return Char.Parse(primitiveValueStr);

                else if (type == typeof(String))
                    return primitiveValueStr;

                else if (type == typeof(DateTime))
                    return DateTime.Parse(primitiveValueStr, this.DefaultCulture);

                else
                    throw new ArgumentException(string.Format("Unsupported primitive type: {0}", type.FullName), "type");

                #endregion
            }
            else
            {
                object obj = Activator.CreateInstance(type, nonPublic: true);

                foreach (MemberValue memberValue in metaObject.MemberValues)
                {
                    MemberInfo field = (MemberInfo)type.GetProperty(memberValue.Name) ?? (MemberInfo)type.GetField(memberValue.Name);

                    if (field is PropertyInfo)
                        ((PropertyInfo)field).SetValue(obj, memberValue.Value);
                    else if (field is FieldInfo)
                        ((FieldInfo)field).SetValue(obj, memberValue.Value);
                }
                return obj;
            }
        }

        private MetaObject ObjectToMetaObject(object obj)
        {
            Type type = obj.GetType();

            var metaObject = new MetaObject() { Type = type };

            if (type.IsEnum ||
                type == typeof(Boolean) ||
                type == typeof(Byte) ||
                type == typeof(SByte) ||
                type == typeof(Int16) ||
                type == typeof(Int32) ||
                type == typeof(Int64) ||
                type == typeof(UInt16) ||
                type == typeof(UInt32) ||
                type == typeof(UInt64) ||
                type == typeof(Single) ||
                type == typeof(Double) ||
                type == typeof(Decimal) ||
                type == typeof(Char) ||
                type == typeof(String) ||
                type == typeof(DateTime)
                )
            {
                metaObject.PrimitiveValueStr = Convert.ToString(obj, this.DefaultCulture);
            }
            else
            {
                metaObject.MemberValues =
                    Enumerable.Concat(
                        type.GetFields()
                            .Select(field => new MemberValue() { Name = field.Name, Value = field.GetValue(obj) })
                        ,
                        type.GetProperties()
                            .Where(property => !IsNonSerializableMemberOfReferenceType(obj, property))
                            .Select(property => new MemberValue() { Name = property.Name, Value = property.GetValue(obj) })
                    )
                    .Where(memberValue => memberValue.Value != null);
            }

            return metaObject;
        }

        private static bool IsNonSerializableMemberOfReferenceType(object obj, MemberInfo member)
        {
            return obj is Reference &&
                (member.Name == Util.GetType<Reference>().GetMember(reference => reference.Target).Name || member.Name == Util.GetType<Reference>().GetMember(reference => reference.Type).Name);
        }

        private string GetErrorMessageFor_COLLECTION_VALUES_KEYWORD()
        {
            return string.Format("{0} for collection style is only possible for types that implements IEnumerable and have an Add method", B.COLLECTION_VALUES_KEYWORD.Text);
        }

        #endregion

        #region Types

        public class MetaObject
        {
            public Type Type { get; set; }
            public IEnumerable<MemberValue> MemberValues { get; set; }
            public string PrimitiveValueStr { get; set; }
        }

        public class MetaArray
        {
            public Type Type { get; set; }
            public IEnumerable Elements { get; set; }
        }

        public class MemberValue
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }

        #endregion

        #region Formatter

        [Formatter(typeof(JsonGrammar), "Default")]
        public class Formatter : Sarcasm.Unparsing.Formatter
        {
            public bool CompactFormat { get; set; }

            private readonly BnfTerms B;

            public Formatter(JsonGrammar grammar)
                : base(grammar)
            {
                this.B = grammar.B;

                this.CompactFormat = true;
            }

            protected override IDecoration GetDecoration(UnparsableAst target)
            {
                var decoration = base.GetDecoration(target);

                decoration.Add(DecorationKey.FontFamily, FontFamily.GenericMonospace);

                if (target.BnfTerm.EqualToAny(B.TYPE_KEYWORD, B.COLLECTION_VALUES_KEYWORD, B.PRIMITIVE_VALUE_KEYWORD))
                    decoration.Add(DecorationKey.Foreground, Color.Pink);
                else if (target.BnfTerm == B.Type)
                    decoration.Add(DecorationKey.Foreground, Color.Gray);
                else if (target.BnfTerm == B.Key)
                    decoration.Add(DecorationKey.Foreground, Color.OrangeRed);
                else if (target.BnfTerm.EqualToAny(B.BOOLEAN, B.NULL, B.NUMBER, B.STRING))
                    decoration.Add(DecorationKey.Foreground, Color.Blue);

                return decoration;
            }

            protected override void GetUtokensAround(UnparsableAst target, out InsertedUtokens leftInsertedUtokens, out InsertedUtokens rightInsertedUtokens)
            {
                base.GetUtokensAround(target, out leftInsertedUtokens, out rightInsertedUtokens);

                if (target.BnfTerm == B.OBJECT_BEGIN)
                    rightInsertedUtokens = UtokenInsert.NewLine;

                else if (target.BnfTerm == B.OBJECT_END)
                    leftInsertedUtokens = rightInsertedUtokens = UtokenInsert.NewLine;

                else if (target.BnfTerm == B.ARRAY_BEGIN)
                    rightInsertedUtokens = UtokenInsert.NewLine;

                else if (target.BnfTerm == B.ARRAY_END)
                    leftInsertedUtokens = rightInsertedUtokens = UtokenInsert.NewLine;

                else if (target.BnfTerm == B.COMMA)
                {
                    leftInsertedUtokens = UtokenInsert.NoWhitespace;
                    rightInsertedUtokens = UtokenInsert.NewLine;
                }

                else if (target.BnfTerm == B.COLON && !CompactFormat)
                    rightInsertedUtokens = UtokenInsert.NewLine;

                else if (target.BnfTerm.EqualToAny(B.TYPE_KEYWORD, B.COLLECTION_VALUES_KEYWORD, B.PRIMITIVE_VALUE_KEYWORD))
                    leftInsertedUtokens = rightInsertedUtokens = UtokenInsert.NoWhitespace;
            }

            protected override BlockIndentation GetBlockIndentation(UnparsableAst leftTerminalLeaveIfAny, UnparsableAst target)
            {
                if (target.BnfTerm == B.OBJECT_BEGIN || target.BnfTerm == B.OBJECT_END)
                    return BlockIndentation.Unindent;

                else if (target.BnfTerm == B.MetaObject)
                    return BlockIndentation.Indent;

                else if (target.BnfTerm == B.MetaArray)
                    return BlockIndentation.Indent;

                else if (target.BnfTerm == B.ARRAY_BEGIN || target.BnfTerm == B.ARRAY_END)
                    return BlockIndentation.Unindent;

                else if (target.BnfTerm == B.Array)
                    return BlockIndentation.Indent;

                else
                    return base.GetBlockIndentation(leftTerminalLeaveIfAny, target);
            }
        }

        #endregion
    }
}
