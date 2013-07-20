using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Sarcasm;
using Sarcasm.GrammarAst;
using Sarcasm.Unparsing;
using Sarcasm.DomainCore;
using System.Reflection;
using Irony.Parsing;
using System.Collections;
using Sarcasm.Utility;
using System.IO;

namespace Sarcasm.UniversalGrammars
{
    [Grammar(typeof(object), "JSON")]
    public class JsonGrammar : Sarcasm.GrammarAst.Grammar
    {
        public class BnfTerms
        {
            public readonly BnfiTermChoiceTL Object = new BnfiTermChoiceTL(typeof(object));
            public readonly BnfiTermRecord<KeyValuePair<string, object>> KeyValuePair = new BnfiTermRecord<KeyValuePair<string, object>>();
            public readonly BnfiTermCollection<List<KeyValuePair<string, object>>, KeyValuePair<string, object>> KeyValuePairs = new BnfiTermCollection<List<KeyValuePair<string, object>>, KeyValuePair<string, object>>();
            public readonly BnfiTermConversion<IEnumerable> Array = new BnfiTermConversion<IEnumerable>();
            public readonly BnfiTermCollectionTL ArrayElements = new BnfiTermCollectionTL(typeof(List<object>));

            public readonly BnfiTermKeyTermPunctuation OBJECT_BEGIN;
            public readonly BnfiTermKeyTermPunctuation OBJECT_END;
            public readonly BnfiTermKeyTermPunctuation ARRAY_BEGIN;
            public readonly BnfiTermKeyTermPunctuation ARRAY_END;
            public readonly BnfiTermKeyTermPunctuation COMMA;
            public readonly BnfiTermKeyTermPunctuation COLON;
            public readonly BnfiTermConversion<int> NUMBER;
//            public readonly BnfiTermConversionTL NUMBER;
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
//                this.NUMBER = TerminalFactoryS.CreateNumberLiteral().MakeUncontractible();
                this.NUMBER = TerminalFactoryS.CreateNumberLiteralInt32().MakeUncontractible();
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
        public const string ENUM_KEYWORD = "$enum";

        public JsonGrammar()
            : base(AstCreation.CreateAstWithAutoBrowsableAstNodes, EmptyCollectionHandling.ReturnEmpty, ErrorHandling.ThrowException)
        {
            B = new BnfTerms(new TerminalFactoryS(this));

            this.Root = B.Object;

            B.Object.Rule =
                B.NUMBER
                |
                B.STRING
                |
                B.BOOLEAN
                |
                B.Array
                |
                B.OBJECT_BEGIN
                + B.KeyValuePairs.ConvertValue(KeyValuePairsToObject, ObjectToKeyValuePairs)
                + B.OBJECT_END
                |
                B.NULL
                ;

            B.KeyValuePairs.Rule =
                B.KeyValuePair.StarList(B.COMMA)
                ;

            B.KeyValuePair.Rule =
                B.STRING.BindTo(B.KeyValuePair, fv => fv.Key)
                + B.COLON
                + B.Object.BindTo(B.KeyValuePair, fv => fv.Value);

            B.Array.Rule =
                B.ARRAY_BEGIN
                + B.ArrayElements.ConvertValue(array => (IEnumerable)array, arrayObject => arrayObject.Cast<object>())
                + B.ARRAY_END;

            B.ArrayElements.Rule =
                B.Object.StarListTL(B.COMMA)
                ;

            B.NUMBER.UtokenizerForUnparse = (formatProvider, astValue) => new UtokenValue[] { UtokenValue.CreateText(Util.ToString(formatProvider, astValue)) };
            B.STRING.UtokenizerForUnparse = (formatProvider, astValue) => new UtokenValue[] { UtokenValue.CreateText(Util.ToString(formatProvider, astValue)) };

            RegisterBracePair(B.OBJECT_BEGIN, B.OBJECT_END);
            RegisterBracePair(B.ARRAY_BEGIN, B.ARRAY_END);

            #region Unparse

            UnparseControl.DefaultFormatter = new Formatter(this);
            UnparseControl.NoPrecedenceBasedParenthesesNeededForExpressions();

            #endregion
        }

        private static object CheckNumber(object numberObject)
        {
            if (IsNumber(numberObject))
                return numberObject;
            else
                throw new InvalidCastException(string.Format("{0} is not a number", numberObject));
        }

        private static bool IsNumber(object numberObject)
        {
            return numberObject is int || numberObject is long || numberObject is float || numberObject is double || numberObject is decimal;
        }

        private object KeyValuePairsToObject(IEnumerable<KeyValuePair<string, object>> keyValuePairs)
        {
            var typeKeyValue = keyValuePairs.First();

            if (typeKeyValue.Key != TYPE_KEYWORD)
                throw new InvalidOperationException(string.Format("{0} is missing for type declaration", TYPE_KEYWORD));

            //Match match = Regex.Match((string)typeKeyValue.Value, @"([^,]+), ([^,]+)");

            //string typeName = match.Groups[1].Value;
            //string assemblyName = match.Groups[2].Value;

            //object obj = Activator.CreateInstance(assemblyName, typeName);

            string typeNameWithAssembly = (string)typeKeyValue.Value;
            Type type = Type.GetType(typeNameWithAssembly);
            object obj;

            var discriminatorKeyValue = keyValuePairs.ElementAtOrDefault(1);

            if (discriminatorKeyValue.Key == ENUM_KEYWORD)
            {
                obj = Enum.Parse(type, (string)discriminatorKeyValue.Value);
            }
            else if (discriminatorKeyValue.Key == COLLECTION_VALUES_KEYWORD)
            {
                obj = Activator.CreateInstance(type);

                if (!IsCollectionType(type))
                    throw new InvalidOperationException(string.Format("{0} for collection style is only possible for types that implements IList or ICollection<>", COLLECTION_VALUES_KEYWORD));

                if (keyValuePairs.Count() != 2)
                    throw new InvalidOperationException(string.Format("For collection style there can be only {0} and {1} specified", TYPE_KEYWORD, COLLECTION_VALUES_KEYWORD));

                dynamic collection = obj;
                IEnumerable elements = (IEnumerable)discriminatorKeyValue.Value;

                foreach (object element in elements)
                    collection.Add(element);
            }
            else
            {
                obj = Activator.CreateInstance(type);

                foreach (var keyValuePair in keyValuePairs.Skip(1))
                {
                    string fieldName = keyValuePair.Key;
                    object value = keyValuePair.Value;

                    MemberInfo field = (MemberInfo)type.GetProperty(fieldName) ?? (MemberInfo)type.GetField(fieldName);

                    if (field is PropertyInfo)
                        ((PropertyInfo)field).SetValue(obj, value);
                    else if (field is FieldInfo)
                        ((FieldInfo)field).SetValue(obj, value);
                }
            }

            return obj;
        }

        private string ToString(IEnumerable<KeyValuePair<string, object>> keyValuePairs)
        {
            StringWriter sw = new StringWriter();
            foreach (var keyValuePair in keyValuePairs)
                sw.WriteLine("{{ {0}, {1} }}", keyValuePair.Key, keyValuePair.Value);
            return sw.ToString();
        }

        private IEnumerable<KeyValuePair<string, object>> ObjectToKeyValuePairs(object obj)
        {
            if (IsNumber(obj) || obj is string || obj is bool || obj == null)
                return null;

            var foo = _ObjectToKeyValuePairs(obj).ToList();
            string st = ToString(foo);
            return foo;
        }

        private IEnumerable<KeyValuePair<string, object>> _ObjectToKeyValuePairs(object obj)
        {
            if (IsNumber(obj) || obj is string || obj is bool || obj == null)
                yield break;

            Type type = obj.GetType();

            yield return new KeyValuePair<string, object>(TYPE_KEYWORD, type.AssemblyQualifiedName);

            if (type.IsEnum)
            {
                yield return new KeyValuePair<string, object>(ENUM_KEYWORD, obj.ToString());
            }
            else if (IsCollectionType(type))
            {
                yield return new KeyValuePair<string, object>(COLLECTION_VALUES_KEYWORD, obj);
            }
            else
            {
                var keyValuePairs =
                    type.GetFields()
                        .Select(field => KeyValuePair.Create(field.Name, field.GetValue(obj)))
                    .Concat(
                    type.GetProperties()
                        .Select(property => KeyValuePair.Create(property.Name, property.GetValue(obj)))
                    )
                    .Where(keyValuePair => keyValuePair.Value != null);

                foreach (var keyValuePair in keyValuePairs)
                    yield return keyValuePair;
            }
        }

        private static bool IsCollectionType(Type type)
        {
            return type is IList || type.IsGenericType && type.GetInterfaces().Any(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(ICollection<>));
        }

        private object Identity(object o)
        {
            return o;
        }

        private T Identity<T>(T t)
        {
            return t;
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
        }

        public static class KeyValuePair
        {
            public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
            {
                return new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        #region Formatter

        [Formatter(typeof(JsonGrammar), "Default")]
        public class Formatter : Sarcasm.Unparsing.Formatter
        {
            private readonly BnfTerms B;

            public Formatter(JsonGrammar grammar)
                : base(grammar)
            {
                this.B = grammar.B;
            }

            protected override IDecoration GetDecoration(UnparsableAst target)
            {
                return Decoration.None;
            }

            protected override InsertedUtokens GetUtokensLeft(UnparsableAst target)
            {
                if (target.BnfTerm == B.OBJECT_BEGIN)
                    return UtokenInsert.NewLine;

                else if (target.BnfTerm == B.OBJECT_END)
                    return UtokenInsert.NewLine;

                else if (target.BnfTerm == B.ARRAY_BEGIN)
                    return UtokenInsert.NewLine;

                else if (target.BnfTerm == B.ARRAY_END)
                    return UtokenInsert.NewLine;

                else if (target.BnfTerm == B.COMMA)
                    return UtokenInsert.NoWhitespace;

                else
                    return base.GetUtokensLeft(target);
            }

            protected override InsertedUtokens GetUtokensRight(UnparsableAst target)
            {
                if (target.BnfTerm == B.OBJECT_BEGIN)
                    return UtokenInsert.NewLine;

                else if (target.BnfTerm == B.OBJECT_END)
                    return UtokenInsert.NewLine;

                else if (target.BnfTerm == B.ARRAY_BEGIN)
                    return UtokenInsert.NewLine;

                else if (target.BnfTerm == B.ARRAY_END)
                    return UtokenInsert.NewLine;

                else if (target.BnfTerm == B.COMMA)
                    return UtokenInsert.NewLine;

                else
                    return base.GetUtokensRight(target);
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
