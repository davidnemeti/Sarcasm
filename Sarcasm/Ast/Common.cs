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

namespace Sarcasm.Ast
{
    public delegate T ValueParser<T>(AstContext context, ParseTreeNodeWithOutAst parseTreeNode);
    public delegate TOut ValueConverter<TIn, TOut>(TIn inputObject);

    public interface IBnfiTerm
    {
        BnfTerm AsBnfTerm();
    }

    public interface INonTerminal
    {
        NonTerminal AsNonTerminal();
    }

    /// <summary>
    /// Typeless IBnfiTerm
    /// </summary>
    public interface IBnfiTermTL : IBnfiTerm
    {
    }

    /// <summary>
    /// Typesafe IBnfiTerm
    /// </summary>
    public interface IBnfiTerm<out T> : IBnfiTerm
    {
    }

    public interface IHasType
    {
        Type Type { get; }
    }

    // NOTE: cannot inherit from IBnfiTerm<T> because of interface implementation conflict in BnfiTermCollection
    public interface IBnfiTermOrAbleForChoice<out T> : IBnfiTerm
    {
    }

    // NOTE: cannot inherit from IBnfiTerm<T> because of covariance vs. contravariance conflict
    public interface IBnfiTermPlusAbleForType<in T> : IBnfiTerm
    {
    }

    public interface IBnfiTermCopyable : IHasType, IBnfiTerm
    {
    }

    public interface IBnfiTermCopyableTL : IBnfiTermCopyable, IBnfiTermTL
    {
    }

    public interface IBnfiTermCopyable<out T> : IBnfiTermCopyable, IBnfiTerm<T>
    {
    }

    public abstract class BnfiTermNonTerminal : NonTerminal, IHasType, IBnfiTerm, INonTerminal
    {
        protected readonly Type type;
        protected readonly bool isReferable;

        protected BnfiTermNonTerminal(Type type, string name, bool isReferable)
            : base(name: name ?? GrammarHelper.TypeNameWithDeclaringTypes(type))
        {
            this.type = type;
            this.isReferable = isReferable;
        }

        internal const string typelessQErrorMessage = "Use the typesafe QVal or QRef extension methods combined with CreateValue or ConvertValue extension methods instead";
        internal const string typelessMemberBoundErrorMessage = "Typeless MemberBoundToBnfTerm should not mix with typesafe MemberBoundToBnfTerm<TDeclaringType>";
        internal const string invalidUseOfNonExistingTypesafePipeOperatorErrorMessage = "There is no typesafe pipe operator for different types. Use 'SetRuleOr' or 'Or' method instead.";

        public bool IsReferable { get { return isReferable; } }

        protected bool IsMovable { get { return !IsReferable; } }

        Type IHasType.Type
        {
            get { return this.type; }
        }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
        }

        NonTerminal INonTerminal.AsNonTerminal()
        {
            return this;
        }

        protected virtual string GetExtraStrForToString()
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
