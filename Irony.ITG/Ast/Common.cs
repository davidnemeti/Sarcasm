using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.ITG.Ast
{
    public delegate T ValueCreator<T>(AstContext context, ParseTreeNodeWithOutAst parseTreeNode);
    public delegate TOut ValueConverter<TIn, TOut>(TIn inputObject);

    public interface IBnfiTerm
    {
        BnfTerm AsBnfTerm();
    }

    public interface IBnfiTerm<out T> : IBnfiTerm
    {
    }

    public interface IHasType
    {
        Type Type { get; }
    }

    public interface IBnfiTermCopyable : IHasType, IBnfiTerm
    {
    }

    public interface IBnfiTermCopyable<in T> : IBnfiTermCopyable
    {
    }

    public abstract class BnfiTermNonTerminal : NonTerminal, IHasType
    {
        protected Type Type { get; private set; }

        protected BnfiTermNonTerminal(Type type, string errorAlias)
            : base(name: GrammarHelper.TypeNameWithDeclaringTypes(type), errorAlias: errorAlias)
        {
            this.Type = type;
        }

        internal const string typelessQErrorMessage = "Use the typesafe QVal or QRef extension methods combined with CreateValue or ConvertValue extension methods instead";
        internal const string typelessMemberBoundErrorMessage = "Typeless MemberBoundToBnfTerm should not mix with typesafe MemberBoundToBnfTerm<TDeclaringType>";
        internal const string invalidUseOfNonExistingTypesafePipeOperatorErrorMessage = "There is no typesafe pipe operator for different types. Use 'SetRuleOr' or 'Or' method instead.";

        Type IHasType.Type
        {
            get { return this.Type; }
        }

        public virtual string GetExtraStrForToString()
        {
            return null;
        }

        public override string ToString()
        {
            string extraStr = GetExtraStrForToString();
            return string.Format("{0}<{1}>{2}", this.GetType().Name, this.Name, extraStr != null ? "(" + extraStr + ")" : "");
        }
    }
}
