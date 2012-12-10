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

namespace Irony.Extension.AstBinders
{
    public class Properties
    {
        private readonly Grammar defaultGrammar = null;

        private readonly Properties instance = new Properties();

        public Properties Instance { get { return instance; } }

        private Properties()
        {
            this[defaultGrammar, BoolProperty.BrowsableAstNodes] = false;
        }

        public bool this[Grammar grammar, BoolProperty boolProperty]
        {
            get
            {
                return GetValue<BoolProperty, bool>(grammar, boolProperty);
            }
            set
            {
                SetValue(grammar, boolProperty, value);
            }
        }

        private TValue GetValue<TProperty, TValue>(Grammar grammar, TProperty property)
        {
            object value;
            return propertyToValue.TryGetValue(Tuple.Create(grammar, (object)property), out value)
                ? (TValue)value
                : (TValue)propertyToValue[Tuple.Create(defaultGrammar, (object)property)];
        }

        private void SetValue<TProperty, TValue>(Grammar grammar, TProperty property, TValue value)
        {
            propertyToValue[Tuple.Create(defaultGrammar, (object)property)] = value;
        }

        private Dictionary<Tuple<Grammar, object>, object> propertyToValue = new Dictionary<Tuple<Grammar, object>, object>();
    }

    public enum BoolProperty
    {
        BrowsableAstNodes
    }
}
