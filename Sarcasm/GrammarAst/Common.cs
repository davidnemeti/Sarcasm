#region License
/*
    This file is part of Sarcasm.

    Copyright 2012-2013 Dávid Németi

    Sarcasm is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Sarcasm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Sarcasm.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

using Irony.Ast;
using Irony.Parsing;
using Sarcasm.Parsing;
using Sarcasm.Unparsing;

namespace Sarcasm.GrammarAst
{
    public delegate TD ValueIntroducer<out TD>(AstContext context, ParseTreeNodeWithoutAst parseTreeNode);
    public delegate TDOut ValueConverter<in TDIn, out TDOut>(TDIn inputAstValue);
    public delegate TD ValueCreatorFromNoAst<out TD>();

    public interface IBnfiTerm
    {
        BnfTerm AsBnfTerm();
        Type DomainType { get; }
    }

    public interface INonTerminal
    {
        NonTerminal AsNonTerminal();
    }

    public interface INonTerminal<out TD> : INonTerminal
    {
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
    public interface IBnfiTerm<out TD> : IBnfiTerm
    {
    }

    // NOTE: cannot inherit from IBnfiTerm<TD> because of interface implementation conflict in BnfiTermCollection
    public interface IBnfiTermOrAbleForChoice<out TD> : IBnfiTerm
    {
    }

    // NOTE: cannot inherit from IBnfiTerm<TD> because of covariance vs. contravariance conflict
    public interface IBnfiTermPlusAbleForType<in TD> : IBnfiTerm
    {
    }

    public interface IBnfiTermCopyable : IBnfiTerm
    {
    }

    public interface IBnfiTermCopyableTL : IBnfiTermCopyable, IBnfiTermTL
    {
    }

    public interface IBnfiTermCopyable<out TD> : IBnfiTermCopyable, IBnfiTerm<TD>
    {
    }

    public abstract class BnfiTermNonTerminal : NonTerminal, IBnfiTerm, INonTerminal, IBnfiTermCopyable, IUnparsableNonTerminal
    {
        protected readonly Type domainType;
        protected readonly bool hasExplicitName;
        private readonly Dictionary<int, UnparseHint> childBnfTermListIndexToUnparseHint;

        protected BnfiTermNonTerminal(string name)
            : base(name)
        {
            this.IsContractible = false;
            this.hasBeenContracted = false;
            this.hasExplicitName = name != null;
            this.childBnfTermListIndexToUnparseHint = new Dictionary<int, UnparseHint>();
        }

        protected BnfiTermNonTerminal(Type domainType, string name)
            : this(name: name ?? GrammarHelper.TypeNameWithDeclaringTypes(domainType))
        {
            if (domainType == null)
                throw new ArgumentNullException("domainType");

            this.domainType = domainType;
            this.hasExplicitName = name != null;
        }

        internal const string typelessQErrorMessage = "Use the typesafe QVal or QRef extension methods combined with IntroValue or ConvertValue extension methods instead";

        public bool IsContractible { get; protected set; }
        protected bool hasBeenContracted;

        public Type DomainType
        {
            get { return this.domainType; }
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
            string extraStr = verboseToString ? GetExtraStrForToString() : null;
            return string.Format("{0}<{1}>{2}", this.GetType().Name, this.Name, extraStr != null ? "(" + extraStr + ")" : "");
        }

        public bool verboseToString = false;

        protected new BnfExpression Rule
        {
            get { return base.Rule; }
            set
            {
                base.Rule = value;
                ProcessUnparseHints(value);
                CheckAfterRuleHasBeenSetThatChildrenAreNotContracted();
            }
        }

        protected void CheckAfterRuleHasBeenSetThatChildrenAreNotContracted()
        {
            if (Rule != null)
            {
                var children = Rule.Data
                    .SelectMany(_children => _children)
                    .OfType<BnfiTermNonTerminal>();

                if (children.Any(child => child.hasBeenContracted))
                {
                    GrammarHelper.ThrowGrammarErrorException(
                        GrammarErrorLevel.Error,
                        "NonTerminal '{0}' has been contracted. You should use MakeUncontractible() on it.", children.First(child => child.hasBeenContracted)
                        );
                }
            }
        }

        #region Unparse

        private void ProcessUnparseHints(BnfExpression rule)
        {
            if (rule != null)
            {
                for (int childBnfTermListIndex = 0; childBnfTermListIndex < rule.Data.Count; childBnfTermListIndex++)
                {
                    BnfTermList bnfTermList = rule.Data[childBnfTermListIndex];

                    try
                    {
                        UnparseHint unparseHint = (UnparseHint)bnfTermList.SingleOrDefault(bnfTerm => bnfTerm is UnparseHint);
                        childBnfTermListIndexToUnparseHint.Add(childBnfTermListIndex, unparseHint);
                    }
                    catch (InvalidOperationException)
                    {
                        GrammarHelper.ThrowGrammarErrorException(
                            GrammarErrorLevel.Error,
                            "NonTerminal '{0}' has more than one UnparseHint on its {1}. childrenlist. Only one UnparseHint is allowed per childrenlist.", this, childBnfTermListIndex + 1
                            );
                    }
                }
            }
        }

        internal UnparseHint GetUnparseHint(int childBnfTermListIndex)
        {
            UnparseHint unparseHint;
            return childBnfTermListIndexToUnparseHint.TryGetValue(childBnfTermListIndex, out unparseHint)
                ? unparseHint
                : null;
        }

        bool IUnparsableNonTerminal.TryGetUtokensDirectly(IUnparser unparser, UnparsableAst self, out IEnumerable<UtokenValue> utokens)
        {
            return TryGetUtokensDirectly(unparser, self, out utokens);
        }

        IEnumerable<UnparsableAst> IUnparsableNonTerminal.GetChildren(Unparser.ChildBnfTerms childBnfTerms, object astValue, Unparser.Direction direction)
        {
            return GetChildren(childBnfTerms, astValue, direction);
        }

        int? IUnparsableNonTerminal.GetChildrenPriority(IUnparser unparser, object astValue, Unparser.Children childrenAtRule, Unparser.Direction direction)
        {
            return GetChildrenPriority(unparser, astValue, childrenAtRule, direction);
        }

        protected abstract bool TryGetUtokensDirectly(IUnparser unparser, UnparsableAst self, out IEnumerable<UtokenValue> utokens);
        protected abstract IEnumerable<UnparsableAst> GetChildren(Unparser.ChildBnfTerms childBnfTerms, object astValue, Unparser.Direction direction);
        protected abstract int? GetChildrenPriority(IUnparser unparser, object astValue, Unparser.Children childrenAtRule, Unparser.Direction direction);

        #endregion
    }
}
