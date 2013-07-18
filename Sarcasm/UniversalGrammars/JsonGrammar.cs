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

namespace Sarcasm.UniversalGrammars
{
    [Grammar(typeof(object), "JSON")]
    public class JsonGrammar : Sarcasm.GrammarAst.Grammar
    {
        public class BnfTerms
        {
            public readonly BnfiTermConversion<object> Object;
            public readonly BnfiTermRecord<KeyValuePair<string, object>> KeyValuePair;
            public readonly BnfiTermConversion<IEnumerable> Array;
            public readonly BnfiTermKeyTermPunctuation OBJECT_BEGIN;
            public readonly BnfiTermKeyTermPunctuation OBJECT_END;
            public readonly BnfiTermKeyTermPunctuation ARRAY_BEGIN;
            public readonly BnfiTermKeyTermPunctuation ARRAY_END;
            public readonly BnfiTermKeyTermPunctuation COMMA;
            public readonly BnfiTermKeyTermPunctuation COLON;
            public readonly BnfiTermConversionTL NumberLiteral;
            public readonly BnfiTermConversion<string> StringLiteral;
            public readonly BnfiTermConstant<bool> BOOLEAN;
            public readonly BnfiTermConstant NULL;

            internal BnfTerms(TerminalFactoryS TerminalFactoryS)
            {
                this.OBJECT_BEGIN = TerminalFactoryS.CreatePunctuation("{");
                this.OBJECT_END = TerminalFactoryS.CreatePunctuation("}");
                this.ARRAY_BEGIN = TerminalFactoryS.CreatePunctuation("[");
                this.ARRAY_END = TerminalFactoryS.CreatePunctuation("]");
                this.COMMA = TerminalFactoryS.CreatePunctuation(",");
                this.COLON = TerminalFactoryS.CreatePunctuation(":");
                this.NumberLiteral = TerminalFactoryS.CreateNumberLiteral();
                this.StringLiteral = TerminalFactoryS.CreateStringLiteral(name: "stringliteral", startEndSymbol: "\"");

                this.BOOLEAN = new BnfiTermConstant<bool>()
                {
                    { "true", true },
                    { "false", false }
                };

                this.NULL = new BnfiTermConstant()
                {
                    { "null", (object)null }
                };
            }
        }

        public readonly BnfTerms B;
        public const string TYPE_KEYWORD = "$type";
        public const string COLLECTION_VALUES_KEYWORD = "$values";

        public JsonGrammar()
            : base(AstCreation.CreateAstWithAutoBrowsableAstNodes, EmptyCollectionHandling.ReturnEmpty, ErrorHandling.ThrowException)
        {
            B = new BnfTerms(new TerminalFactoryS(this));

            this.Root = B.Object;

            B.Object.Rule =
                B.OBJECT_BEGIN
                + B.KeyValuePair.StarList(B.COMMA).ConvertValue(KeyValuePairsToObject, ObjectToKeyValuePairs)
                + B.OBJECT_END
                |
                B.Array.ConvertValue(array => (object)array, arrayObject => (IEnumerable)arrayObject)
                |
                B.NumberLiteral.ConvertValue<object>(Identity<object>, Identity<object>)
                |
                B.StringLiteral.ConvertValue(stringLiteral => (object)stringLiteral, stringObject => (string)stringObject)
                |
                B.BOOLEAN.ConvertValue(booleanLiteral => (object)booleanLiteral, booleanObject => (bool)booleanObject)
                |
                B.NULL.ConvertValue(nullLiteral => nullLiteral, nullObject => nullObject);

            B.KeyValuePair.Rule =
                B.StringLiteral.BindTo(B.KeyValuePair, fv => fv.Key)
                + B.COLON
                + B.Object.BindTo(B.KeyValuePair, fv => fv.Value);

            B.Array.Rule =
                B.ARRAY_BEGIN
                + B.Object.StarList(B.COMMA).ConvertValue(array => (IEnumerable)array, arrayObject => arrayObject.Cast<object>())
                + B.ARRAY_END;
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
            object obj = Activator.CreateInstance(type);

            var collectionKeyValue = keyValuePairs.ElementAtOrDefault(1);

            if (collectionKeyValue.Key == COLLECTION_VALUES_KEYWORD)
            {
                if (!IsCollectionType(type))
                    throw new InvalidOperationException(string.Format("{0} for collection style is only possible for types that implements IList or ICollection<>", COLLECTION_VALUES_KEYWORD));

                if (keyValuePairs.Count() != 2)
                    throw new InvalidOperationException(string.Format("For collection style there can be only {0} and {1} specified", TYPE_KEYWORD, COLLECTION_VALUES_KEYWORD));

                dynamic collection = obj;
                IEnumerable elements = (IEnumerable)collectionKeyValue.Value;

                foreach (object element in elements)
                    collection.Add(element);
            }
            else
            {
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

        private IEnumerable<KeyValuePair<string, object>> ObjectToKeyValuePairs(object obj)
        {
            Type type = obj.GetType();

            yield return new KeyValuePair<string, object>(TYPE_KEYWORD, type.AssemblyQualifiedName);

            if (IsCollectionType(type))
            {
                yield return new KeyValuePair<string, object>(COLLECTION_VALUES_KEYWORD, obj);
            }
            else
            {
                foreach (FieldInfo field in type.GetFields())
                    yield return new KeyValuePair<string, object>(field.Name, field.GetValue(obj));

                foreach (PropertyInfo property in type.GetProperties())
                    yield return new KeyValuePair<string, object>(property.Name, property.GetValue(obj));
            }
        }

        private static bool IsCollectionType(Type type)
        {
            return type is IList || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>);
        }

        private object Identity(object o)
        {
            return o;
        }

        private T Identity<T>(T t)
        {
            return t;
        }
    }
}
