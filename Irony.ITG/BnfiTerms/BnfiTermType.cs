using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.ITG
{
    public partial class BnfiTermType : BnfiTermNonTerminal, IBnfiTerm
    {
        public BnfiTermType(Type type, string errorAlias = null)
            : base(type, errorAlias)
        {
            if (type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, binder: null, types: Type.EmptyTypes, modifiers: null) == null)
                throw new ArgumentException("Type has no default constructor (neither public nor nonpublic)", "type");

            this.AstConfig.NodeCreator = (context, parseTreeNode) =>
            {
                object objValue = Activator.CreateInstance(type, nonPublic: true);

                var childValues = parseTreeNode.ChildNodes.Select(childNode => GrammarHelper.AstNodeToValue(childNode.AstNode)).Where(childNode => childNode != null);

                // 1. memberwise copy for ast values
                foreach (var childValue in childValues.Where(childValue => objValue.GetType().IsAssignableFrom(childValue.GetType())))
                    MemberwiseCopyExceptNullValues(objValue, childValue);

                // 2. set member values by MemberValues (so that we can overwrite the copied members if we want)
                foreach (var memberValue in childValues.OfType<MemberValue>())
                {
                    if (memberValue.MemberInfo is PropertyInfo)
                        ((PropertyInfo)memberValue.MemberInfo).SetValue(objValue, memberValue.Value);
                    else if (memberValue.MemberInfo is FieldInfo)
                        ((FieldInfo)memberValue.MemberInfo).SetValue(objValue, memberValue.Value);
                    else
                        throw new ApplicationException("Object with wrong type in memberinfo: " + memberValue.MemberInfo.Name);
                }

                parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(objValue, context, parseTreeNode);
            };
        }

        public new BnfiExpressionType Rule
        {
            get { return (BnfiExpressionType)base.Rule; }
            set { base.Rule = value; }
        }

        protected static void MemberwiseCopyExceptNullValues(object destination, object source)
        {
            MemberwiseCopy(destination, source, fieldValue => fieldValue != null);
        }

        protected static void MemberwiseCopy(object destination, object source)
        {
            MemberwiseCopy(destination, source, fieldValue => true);
        }

        protected static void MemberwiseCopy(object destination, object source, Predicate<object> cloneFieldValue)
        {
            Type destinationType = destination.GetType();
            Type sourceType = source.GetType();

            if (!destinationType.IsAssignableFrom(sourceType))
                throw new ArgumentException(string.Format("{0} is not assignable from {1}", destinationType.Name, sourceType.Name), "source");

            foreach (FieldInfo fieldInfo in GetAllFields(sourceType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                object fieldValue = fieldInfo.GetValue(source);
                if (cloneFieldValue(fieldValue))
                    fieldInfo.SetValue(destination, fieldValue);
            }
        }

        protected static IEnumerable<FieldInfo> GetAllFields(Type type, BindingFlags bindingFlags)
        {
            return type.GetFields(bindingFlags)
                .Concat(type.BaseType != null
                    ? GetAllFields(type.BaseType, bindingFlags)
                    : new FieldInfo[0]);
        }

        public BnfExpression RuleTL { get { return base.Rule; } set { base.Rule = value; } }

        public BnfTerm AsBnfTerm()
        {
            return this;
        }
    }

    public partial class BnfiTermType<TType> : BnfiTermType, IBnfiTerm<TType>, INonTerminalWithMultipleTypesafeRule<TType>
        where TType : new()
    {
        public static TType __ { get; private set; }

        static BnfiTermType()
        {
            __ = new TType();
        }

        public TType _ { get { return BnfiTermType<TType>.__; } }

        public BnfiTermType(string errorAlias = null)
            : base(typeof(TType), errorAlias)
        {
        }

        [Obsolete(typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public new BnfiExpressionType<TType> Rule { set { base.Rule = value; } }

    }
}
