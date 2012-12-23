﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.ITG.Ast
{
    public partial class BnfiTermConstant<T> : ConstantTerminal, IBnfiTerm<T>, IEnumerable<KeyValuePair<string, T>>
    {
        public BnfiTermConstant()
            : base(GrammarHelper.TypeNameWithDeclaringTypes(typeof(T)))
        {
        }

        public void Add(string lexeme, T value)
        {
            base.Add(lexeme, value);
        }

        [Obsolete("Type of value does not match", error: true)]
        public new void Add(string lexeme, object value)
        {
            base.Add(lexeme, value);
        }

        public BnfTerm AsBnfTerm()
        {
            return this;
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return base.Constants.Select(keyValuePair => new KeyValuePair<string, T>(keyValuePair.Key, (T)keyValuePair.Value)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
