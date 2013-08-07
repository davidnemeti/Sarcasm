using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections;
using System.IO;

using Sarcasm;
using Sarcasm.GrammarAst;
using Sarcasm.Parsing;
using Sarcasm.Unparsing;
using Sarcasm.DomainCore;
using Sarcasm.Utility;
using Sarcasm.Reflection;
using Sarcasm.Unparsing.Styles;
using Irony.Parsing;

namespace Sarcasm.UniversalGrammars
{
    [Grammar(typeof(object), "JSON")]
    public class JsonGrammar : Sarcasm.GrammarAst.Grammar
    {
        public class BnfTerms
        {
            public readonly BnfiTermChoiceTL Object = new BnfiTermChoiceTL(typeof(object), "Object");
            public readonly BnfiTermRecord<KeyValuePair<string, object>> KeyValuePair = new BnfiTermRecord<KeyValuePair<string, object>>("KeyValuePair");
            public readonly BnfiTermCollection<List<KeyValuePair<string, object>>, KeyValuePair<string, object>> KeyValuePairs = new BnfiTermCollection<List<KeyValuePair<string, object>>, KeyValuePair<string, object>>("KeyValuePairs");
            public readonly BnfiTermRecord<Array> Array = new BnfiTermRecord<Array>("Array");
            public readonly BnfiTermCollectionTL ArrayElements = new BnfiTermCollectionTL(typeof(List<object>), "ArrayElements");
            public readonly BnfiTermCopy<string> Key = new BnfiTermCopy<string>("Key");
            public readonly BnfiTermCopyTL Value = new BnfiTermCopyTL(typeof(object), "Value");

            public readonly BnfiTermKeyTerm OBJECT_BEGIN;
            public readonly BnfiTermKeyTerm OBJECT_END;
            public readonly BnfiTermKeyTerm ARRAY_BEGIN;
            public readonly BnfiTermKeyTerm ARRAY_END;
            public readonly BnfiTermKeyTerm COMMA;
            public readonly BnfiTermKeyTerm COLON;
            public readonly BnfiTermConversionTL NUMBER;
            public readonly BnfiTermConversion<string> STRING;
            public readonly BnfiTermConstant<bool> BOOLEAN;
            public readonly BnfiTermConstantTL NULL;

            internal BnfTerms(TerminalFactoryS TerminalFactoryS)
            {
                this.OBJECT_BEGIN = TerminalFactoryS.CreateKeyTerm("{");
                this.OBJECT_END = TerminalFactoryS.CreateKeyTerm("}");
                this.ARRAY_BEGIN = TerminalFactoryS.CreateKeyTerm("[");
                this.ARRAY_END = TerminalFactoryS.CreateKeyTerm("]");
                this.COMMA = TerminalFactoryS.CreateKeyTerm(",");
                this.COLON = TerminalFactoryS.CreateKeyTerm(":");
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

        public readonly BnfTerms B;

        public const string TYPE_KEYWORD = "$type";
        public const string COLLECTION_VALUES_KEYWORD = "$values";
        public const string PRIMITIVE_VALUE_KEYWORD = "$value";

        public JsonGrammar()
            : this(new UniversalDefaultDomain())
        {
        }

        public JsonGrammar(Domain domain)
            : base(domain)
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
                B.Array
                |
                B.NULL + SetUnparsePriority((astValue, childAstValues) => astValue == null ? (int?)1 : null)
                |
                B.OBJECT_BEGIN
                + B.KeyValuePairs.ConvertValue(KeyValuePairsToObject, ObjectToKeyValuePairs)
                + B.OBJECT_END
                ;

            B.KeyValuePairs.Rule =
                B.KeyValuePair.PlusList(B.COMMA)
                ;

            B.KeyValuePair.Rule =
                B.Key.BindTo(B.KeyValuePair, fv => fv.Key)
                + B.COLON
                + B.Value.BindTo(B.KeyValuePair, fv => fv.Value);

            B.Key.Rule =
                B.STRING
                ;

            B.Value.Rule =
                B.Object
                ;

            B.Array.Rule =
                B.ARRAY_BEGIN
                + B.ArrayElements.BindTo(B.Array, metaArray => metaArray.Elements)
                + B.ARRAY_END
                ;

            B.ArrayElements.Rule =
                B.Object.StarListTL(B.COMMA)
                ;

            RegisterBracePair(B.OBJECT_BEGIN, B.OBJECT_END);
            RegisterBracePair(B.ARRAY_BEGIN, B.ARRAY_END);

            // TODO: although JSON does not have comments, we define C-style comments for until we solve parsing and unparsing of comments into JSON data
            CommentTerminal DelimitedComment = new CommentTerminal("DelimitedComment", "/*", "*/");
            CommentTerminal SingleLineComment = new CommentTerminal("SingleLineComment", "//", Environment.NewLine);

            NonGrammarTerminals.Add(DelimitedComment);
            NonGrammarTerminals.Add(SingleLineComment);
        }

        #region Helpers

        private object KeyValuePairsToObject(IEnumerable<KeyValuePair<string, object>> keyValuePairs)
        {
            var typeKeyValue = keyValuePairs.First();

            if (typeKeyValue.Key != TYPE_KEYWORD)
                throw new AstException(string.Format("{0} is missing for type declaration", TYPE_KEYWORD));

            MetaRepository.EnsureThatAssemblyResolverIsRegistered();

            string typeNameWithAssembly = (string)typeKeyValue.Value;
            Type type = Type.GetType(typeNameWithAssembly);

            if (type == null)
                throw new AstException("Type cannot be resolved: " + typeNameWithAssembly);

            var discriminatorKeyValue = keyValuePairs.ElementAtOrDefault(1);

            if (discriminatorKeyValue.Key == PRIMITIVE_VALUE_KEYWORD)
            {
                try
                {
                    if (type.IsEnum)
                        return Enum.Parse(type, (string)discriminatorKeyValue.Value);

                    else if (type == typeof(Boolean))
                        return Boolean.Parse((string)discriminatorKeyValue.Value);

                    else if (type == typeof(Byte))
                        return Byte.Parse((string)discriminatorKeyValue.Value, this.DefaultCulture);

                    else if (type == typeof(SByte))
                        return SByte.Parse((string)discriminatorKeyValue.Value, this.DefaultCulture);

                    else if (type == typeof(Int16))
                        return Int16.Parse((string)discriminatorKeyValue.Value, this.DefaultCulture);

                    else if (type == typeof(Int32))
                        return Int32.Parse((string)discriminatorKeyValue.Value, this.DefaultCulture);

                    else if (type == typeof(Int64))
                        return Int64.Parse((string)discriminatorKeyValue.Value, this.DefaultCulture);

                    else if (type == typeof(UInt16))
                        return UInt16.Parse((string)discriminatorKeyValue.Value, this.DefaultCulture);

                    else if (type == typeof(UInt32))
                        return UInt32.Parse((string)discriminatorKeyValue.Value, this.DefaultCulture);

                    else if (type == typeof(UInt64))
                        return UInt64.Parse((string)discriminatorKeyValue.Value, this.DefaultCulture);

                    else if (type == typeof(Single))
                        return Single.Parse((string)discriminatorKeyValue.Value, this.DefaultCulture);

                    else if (type == typeof(Double))
                        return Double.Parse((string)discriminatorKeyValue.Value, this.DefaultCulture);

                    else if (type == typeof(Decimal))
                        return Decimal.Parse((string)discriminatorKeyValue.Value, this.DefaultCulture);

                    else if (type == typeof(Char))
                        return Char.Parse((string)discriminatorKeyValue.Value);

                    else if (type == typeof(String))
                        return (String)discriminatorKeyValue.Value;

                    else if (type == typeof(DateTime))
                        return DateTime.Parse((string)discriminatorKeyValue.Value, this.DefaultCulture);

                    else
                        throw new AstException(string.Format("Unsupported primitive type: {0}", type.FullName));
                }
                catch (ArgumentException)
                {
                    throw new AstException(string.Format("Cannot parse value '{0}' of type '{1}'", discriminatorKeyValue.Value, type.FullName));
                }
                catch (FormatException)
                {
                    throw new AstException(string.Format("Cannot parse value '{0}' of type '{1}'", discriminatorKeyValue.Value, type.FullName));
                }
            }
            else if (discriminatorKeyValue.Key == COLLECTION_VALUES_KEYWORD)
            {
                if (!IsCollectionType(type))
                    throw new AstException(string.Format("{0} for collection style is only possible for types that implements IList or ICollection<>", COLLECTION_VALUES_KEYWORD));

                dynamic array = Activator.CreateInstance(type);

                foreach (dynamic element in ((Array)discriminatorKeyValue.Value).Elements)
                    array.Add(element);

                return array;
            }
            else
            {
                object obj = Activator.CreateInstance(type, nonPublic: true);

                foreach (var keyValuePair in keyValuePairs.Skip(1))
                {
                    string fieldName = keyValuePair.Key;
                    object value = keyValuePair.Value;

                    MemberInfo field = (MemberInfo)type.GetProperty(fieldName) ?? (MemberInfo)type.GetField(fieldName);

                    if (field is PropertyInfo)
                        ((PropertyInfo)field).SetValue(obj, value);
                    else if (field is FieldInfo)
                        ((FieldInfo)field).SetValue(obj, value);
                    else
                        throw new AstException(string.Format("Type '{0}' does not have a '{1}' field", type.FullName, fieldName));
                }

                return obj;
            }
        }

        private IEnumerable<KeyValuePair<string, object>> ObjectToKeyValuePairs(object obj)
        {
            Type type = obj.GetType();

            yield return new KeyValuePair<string, object>(TYPE_KEYWORD, type.AssemblyQualifiedName) { IsTypeInfo = true };

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
                yield return new KeyValuePair<string, object>(PRIMITIVE_VALUE_KEYWORD, Convert.ToString(obj, this.DefaultCulture));

            else if (IsCollectionType(type))
                yield return new KeyValuePair<string, object>(COLLECTION_VALUES_KEYWORD, new Array() { Elements = (IEnumerable)obj });

            else
            {
                var keyValuePairs =
                    Enumerable.Concat(
                        type.GetFields()
                            .Select(field => KeyValuePair.Create(field.Name, field.GetValue(obj)))
                        ,
                        type.GetProperties()
                            .Where(property => !IsNonSerializableMemberOfReferenceType(obj, property))
                            .Select(property => KeyValuePair.Create(property.Name, property.GetValue(obj)))
                    )
                    .Where(keyValuePair => keyValuePair.Value != null);

                foreach (var keyValuePair in keyValuePairs)
                    yield return keyValuePair;
            }
        }

        private static bool IsNonSerializableMemberOfReferenceType(object obj, MemberInfo member)
        {
            return obj is Reference &&
                (member.Name == Util.GetType<Reference>().GetMember(reference => reference.Target).Name || member.Name == Util.GetType<Reference>().GetMember(reference => reference.Type).Name);
        }

        private static bool IsCollectionType(Type type)
        {
            return type is IList || type.GetInterfaces().Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(ICollection<>));
        }

        private string ToString(IEnumerable<KeyValuePair<string, object>> keyValuePairs)
        {
            StringWriter sw = new StringWriter();
            foreach (var keyValuePair in keyValuePairs)
                sw.WriteLine("{{ {0}, {1} }}", keyValuePair.Key, keyValuePair.Value);
            return sw.ToString();
        }

        #endregion

        #region Helpers types

        public class Array
        {
            public IEnumerable Elements { get; set; }
        }

        public class KeyValuePair<TKey, TValue>
        {
            public KeyValuePair() { }

            public KeyValuePair(TKey key, TValue value)
            {
                this.Key = key;
                this.Value = value;
            }

            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public bool IsTypeInfo { get; set; }
        }

        public static class KeyValuePair
        {
            public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
            {
                return new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        #endregion

        #region Unparse

        protected override UnparseControl GetUnparseControl()
        {
            return UnparseControl.Create(this, new Formatter(this));
        }

        #endregion

        #region Formatter

        [Formatter(typeof(JsonGrammar), "Default")]
        public class Formatter : Sarcasm.Unparsing.Formatter
        {
            #region Settings

            public bool CompactFormat { get; set; }
            public bool ShowTypesInBold { get; set; }

            public Color ForeColorOfKeyword { get; set; }
            public Color ForeColorOfKey { get; set; }
            public Color ForeColorOfType { get; set; }
            public Color ForeColorOfValue { get; set; }
            public Color ForeColorOfLiteral { get; set; }
            public Color ForeColorOfComment { get; set; }

            #endregion

            private readonly BnfTerms B;

            public Formatter(JsonGrammar grammar)
                : base(grammar)
            {
                this.B = grammar.B;

                this.CompactFormat = true;
                this.ShowTypesInBold = true;

                ForeColorOfKeyword = Color.Red;
                ForeColorOfKey = Color.Blue;
                ForeColorOfType = Color.ForestGreen;
                ForeColorOfValue = Color.Black;
                ForeColorOfLiteral = Color.Violet;
                ForeColorOfComment = Color.DarkGray;
            }

            protected override IDecoration GetDecoration(Utoken utoken, UnparsableAst target)
            {
                var decoration = base.GetDecoration(utoken, target);

                decoration.Add(DecorationKey.FontFamily, FontFamily.GenericMonospace);

                if (utoken.Discriminator.EqualToAny(CommentContent, CommentStartSymbol, CommentEndSymbol))
                {
                    decoration
                        .Add(DecorationKey.Foreground, ForeColorOfComment)
                        ;

                    return decoration;
                }

                if (target != null)
                {
                    if (target.BnfTerm.EqualToAny(B.BOOLEAN, B.NULL) || target.BnfTerm is NumberLiteral)
                        decoration.Add(DecorationKey.Foreground, ForeColorOfLiteral);
                    else if (target.AstParentMember != null && target.AstParentMember.BnfTerm == B.Key)
                    {
                        if (((string)target.AstImage.AstValue).EqualToAny(TYPE_KEYWORD, COLLECTION_VALUES_KEYWORD, PRIMITIVE_VALUE_KEYWORD))
                            decoration.Add(DecorationKey.Foreground, ForeColorOfKeyword);
                        else
                            decoration.Add(DecorationKey.Foreground, ForeColorOfKey);
                    }
                    else if (target.AstParentMember != null && target.AstParentMember.BnfTerm == B.Value)
                    {
                        if (target.AstParent != null && target.AstParent.AstValue is KeyValuePair<string, object> && ((KeyValuePair<string, object>)target.AstParent.AstValue).IsTypeInfo)
                            decoration.Add(DecorationKey.Foreground, ForeColorOfType);
                        else
                            decoration.Add(DecorationKey.Foreground, ForeColorOfValue);
                    }
                }

                return decoration;
            }

            protected override void GetUtokensAround(UnparsableAst target, out InsertedUtokens leftInsertedUtokens, out InsertedUtokens rightInsertedUtokens)
            {
                base.GetUtokensAround(target, out leftInsertedUtokens, out rightInsertedUtokens);

                if (target.BnfTerm == B.OBJECT_BEGIN)
                    rightInsertedUtokens = UtokenInsert.NewLine();

                else if (target.BnfTerm == B.OBJECT_END)
                    leftInsertedUtokens = rightInsertedUtokens = UtokenInsert.NewLine();

                else if (target.BnfTerm == B.ARRAY_BEGIN)
                    rightInsertedUtokens = UtokenInsert.NewLine();

                else if (target.BnfTerm == B.ARRAY_END)
                    leftInsertedUtokens = rightInsertedUtokens = UtokenInsert.NewLine();

                else if (target.BnfTerm == B.COMMA)
                {
                    leftInsertedUtokens = UtokenInsert.NoWhitespace();
                    rightInsertedUtokens = UtokenInsert.NewLine();
                }

                else if (target.BnfTerm == B.COLON && !CompactFormat)
                    rightInsertedUtokens = UtokenInsert.NewLine();
            }

            protected override BlockIndentation GetBlockIndentation(UnparsableAst leftTerminalLeaveIfAny, UnparsableAst target)
            {
                if (target.BnfTerm == B.KeyValuePairs)
                    return BlockIndentation.Indent;

                else if (target.BnfTerm == B.ArrayElements)
                    return BlockIndentation.Indent;

                else
                    return base.GetBlockIndentation(leftTerminalLeaveIfAny, target);
            }
        }

        #endregion
    }
}
