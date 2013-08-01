using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    public class MetaGrammar
    {
        public Type GrammarType { get; private set; }
        public GrammarAttribute GrammarAttribute { get; private set; }
        public Type DomainRoot { get; private set; }

        private ObservableCollection<MetaFormatter> metaFormatters;
        public ReadOnlyObservableCollection<MetaFormatter> MetaFormatters { get; private set; }

        public string Name { get { return GrammarAttribute.Name; } }

        public MetaGrammar(Type grammarType)
        {
            var grammarAttribute = grammarType.GetCustomAttribute<GrammarAttribute>();

            if (!IsGrammarType(grammarType))
                throw new ArgumentException("Type should be a grammar type, i.e. a subclass of Grammar with GrammarAttribute", "type");

            this.GrammarType = grammarType;
            this.GrammarAttribute = grammarAttribute;
            this.DomainRoot = grammarAttribute.DomainRoot;

            MetaFormatters = Util.CreateAndGetReadonlyCollection(out metaFormatters);
        }

        public static bool IsGrammarType(Type type)
        {
            return type.IsSubclassOf(typeof(Grammar)) && type.GetCustomAttribute<GrammarAttribute>() != null;
        }

        public void RegisterFormatter(MetaFormatter metaFormatter)
        {
            if (metaFormatters.Any(_metaFormatter => _metaFormatter.FormatterType == metaFormatter.FormatterType))
                throw new ArgumentException("Formatter already registered " + metaFormatter.Name, "metaFormatter");

            metaFormatters.Add(metaFormatter);
        }

        public bool IsUniversalGrammar()
        {
            return DomainRoot == null || DomainRoot == typeof(object);
        }

        public Grammar CreateGrammar()
        {
            return (Grammar)Activator.CreateInstance(GrammarType);
        }

        public Grammar CreateGrammar(CultureInfo cultureInfo)
        {
            try
            {
                return (Grammar)Activator.CreateInstance(GrammarType, cultureInfo);
            }
            catch (MissingMethodException)
            {
                Grammar grammar = CreateGrammar();
                grammar.DefaultCulture = cultureInfo;
                return grammar;
            }
        }
    }
}
