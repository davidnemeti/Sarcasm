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
using Sarcasm;
using Sarcasm.Unparsing;

namespace Sarcasm.Ast
{
    public partial class BnfiTermType : BnfiTermNonTerminal, IBnfiTermTL, IBnfiTermCopyable, IUnparsable
    {
        public BnfiTermType(Type type, string errorAlias = null)
            : base(type, errorAlias)
        {
            if (type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, binder: null, types: Type.EmptyTypes, modifiers: null) == null)
                throw new ArgumentException("Type has no default constructor (neither public nor nonpublic)", "type");

            this.AstConfig.NodeCreator = (context, parseTreeNode) =>
            {
                object objValue = Activator.CreateInstance(type, nonPublic: true);

                var childUnparsableObjects = parseTreeNode.ChildNodes.Select(childNode => GrammarHelper.AstNodeToValue(childNode.AstNode)).Where(childNode => childNode != null);

                // 1. memberwise copy for ast values
                foreach (var childValue in childUnparsableObjects.Where(childValue => objValue.GetType().IsAssignableFrom(childValue.GetType())))
                    MemberwiseCopyExceptNullValues(objValue, childValue);

                // 2. set member values by MemberValues (so that we can overwrite the copied members if we want)
                foreach (var memberValue in childUnparsableObjects.OfType<MemberValue>())
                    SetValue(memberValue, objValue);

                parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(objValue, context, parseTreeNode);
            };
        }

        public new BnfiExpressionType Rule { set { base.Rule = value; } }

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

        protected static void SetValue(MemberInfo memberInfo, object obj, object value)
        {
            if (memberInfo is PropertyInfo)
                ((PropertyInfo)memberInfo).SetValue(obj, value);
            else if (memberInfo is FieldInfo)
                ((FieldInfo)memberInfo).SetValue(obj, value);
            else
                throw new ApplicationException("Object with wrong type in memberinfo: " + memberInfo.Name);
        }

        private static void SetValue(MemberValue memberValue, object obj)
        {
            SetValue(memberValue.MemberInfo, obj, memberValue.Value);
        }

        protected static object GetValue(MemberInfo memberInfo, object obj)
        {
            if (memberInfo is PropertyInfo)
                return ((PropertyInfo)memberInfo).GetValue(obj);
            else if (memberInfo is FieldInfo)
                return ((FieldInfo)memberInfo).GetValue(obj);
            else
                throw new ApplicationException("Object with wrong type in memberinfo: " + memberInfo.Name);
        }

        public BnfExpression RuleRaw { get { return base.Rule; } set { base.Rule = value; } }

        #region Unparse

        bool IUnparsable.TryGetUtokensDirectly(IUnparser unparser, object obj, out IEnumerable<Utoken> utokens)
        {
            utokens = null;
            return false;
        }

        IEnumerable<UnparsableObject> IUnparsable.GetChildUnparsableObjects(BnfTermList childBnfTerms, object obj)
        {
            foreach (BnfTerm childBnfTerm in childBnfTerms)
            {
                object childObj;

                if (childBnfTerm is BnfiTermMember)
                {
                    BnfiTermMember bnfiTermMember = (BnfiTermMember)childBnfTerm;
                    childObj = GetValue(bnfiTermMember.MemberInfo, obj);
                }
                else if (childBnfTerm is BnfiTermCopyable)
                    childObj = obj;
                else
                    childObj = null;

                yield return new UnparsableObject(childBnfTerm, childObj);
            }
        }

        int? IUnparsable.GetChildBnfTermListPriority(IUnparser unparser, object obj, IEnumerable<UnparsableObject> childUnparsableObjects)
        {
            return childUnparsableObjects.SumIncludingNullValues(childValue => unparser.GetBnfTermPriority(childValue.bnfTerm, childValue.obj));
        }

        #endregion
    }

    public partial class BnfiTermType<TType> : BnfiTermType, IBnfiTerm<TType>, IBnfiTermCopyable<TType>
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

        public BnfiExpressionType RuleTypeless { set { base.Rule = value; } }

        public new BnfiExpressionType<TType> Rule { set { base.Rule = value; } }
    }
}
