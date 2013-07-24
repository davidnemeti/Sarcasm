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
    public class MetaFormatter
    {
        public Type FormatterType { get; private set; }
        public FormatterAttribute FormatterAttribute { get; private set; }
        public Formatter Formatter { get; private set; }
        public Type GrammarType { get; private set; }

        public string Name { get { return FormatterAttribute.Name; } }

        public MetaFormatter(Type formatterType)
        {
            var formatterAttribute = formatterType.GetCustomAttribute<FormatterAttribute>();

            if (!IsFormatterType(formatterType))
                throw new ArgumentException("Type should be a formatter type, i.e. a subclass of Formatter with FormatterAttribute", "type");

            this.FormatterType = formatterType;
            this.FormatterAttribute = formatterAttribute;
            this.GrammarType = formatterAttribute.GrammarType;
        }

        public static bool IsFormatterType(Type type)
        {
            return type.IsSubclassOf(typeof(Formatter)) && type.GetCustomAttribute<FormatterAttribute>() != null;
        }
    }
}
