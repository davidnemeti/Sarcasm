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

using Irony.Parsing;

namespace Sarcasm.GrammarAst
{
    public abstract partial class BnfiTermConstant : ConstantTerminal, IBnfiTerm
    {
        protected readonly Type domainType;

        protected BnfiTermConstant(Type domainType)
            : base(GrammarHelper.TypeNameWithDeclaringTypes(domainType))
        {
            this.domainType = domainType;
            this.AstConfig.NodeCreator = (context, parseTreeNode) => parseTreeNode.AstNode = parseTreeNode.Token.Value;
        }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
        }

        public Type DomainType
        {
            get { return domainType; }
        }
    }

    public partial class BnfiTermConstantTL : BnfiTermConstant, IBnfiTermTL, IEnumerable<KeyValuePair<string, object>>
    {
        public BnfiTermConstantTL()
            : base(typeof(object))
        {
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return base.Constants.Select(keyValuePair => new KeyValuePair<string, object>(keyValuePair.Key, keyValuePair.Value)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public partial class BnfiTermConstant<TD> : BnfiTermConstant, IBnfiTerm<TD>, IBnfiTermOrAbleForChoice<TD>, IEnumerable<KeyValuePair<string, TD>>
    {
        public BnfiTermConstant()
            : base(typeof(TD))
        {
        }

        public void Add(string lexeme, TD value)
        {
            base.Add(lexeme, value);
        }

        [Obsolete("Type of value does not match", error: true)]
        public new void Add(string lexeme, object value)
        {
            base.Add(lexeme, value);
        }

        public IEnumerator<KeyValuePair<string, TD>> GetEnumerator()
        {
            return base.Constants.Select(keyValuePair => new KeyValuePair<string, TD>(keyValuePair.Key, (TD)keyValuePair.Value)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
