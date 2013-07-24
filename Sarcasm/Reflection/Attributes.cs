using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Sarcasm.DomainCore;
using Sarcasm.GrammarAst;
using Sarcasm.Unparsing;
using Sarcasm.Utility;

namespace Sarcasm.Reflection
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DomainRootAttribute : Attribute
    {
        public string Name { get; private set; }

        public DomainRootAttribute(string name)
        {
            this.Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class GrammarAttribute : Attribute
    {
        public Type DomainRoot { get; private set; }
        public string Name { get; private set; }

        public GrammarAttribute(Type domainRoot, string name)
        {
            this.DomainRoot = domainRoot;
            this.Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class FormatterAttribute : Attribute
    {
        public Type GrammarType { get; private set; }
        public string Name { get; private set; }

        public FormatterAttribute(Type grammarType, string name)
        {
            this.GrammarType = grammarType;
            this.Name = name;
        }
    }
}
