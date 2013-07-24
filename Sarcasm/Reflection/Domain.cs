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
    public class Domain
    {
        public Type DomainRoot { get; private set; }
        public DomainRootAttribute DomainRootAttribute { get; private set; }

        private ObservableCollection<MetaGrammar> metaGrammars;
        public ReadOnlyObservableCollection<MetaGrammar> MetaGrammars { get; private set; }

        public string Name { get { return DomainRootAttribute.Name; } }

        public Domain(Type domainRoot)
        {
            var domainRootAttribute = domainRoot.GetCustomAttribute<DomainRootAttribute>();

            if (!IsDomainRoot(domainRoot))
                throw new ArgumentException("Type should be a domain root, i.e. a type with DomainRootAttribute", "type");

            this.DomainRoot = domainRoot;
            this.DomainRootAttribute = domainRootAttribute;

            MetaGrammars = Util.CreateAndGetReadonlyCollection(out metaGrammars);
        }

        public static bool IsDomainRoot(Type type)
        {
            return type.GetCustomAttribute<DomainRootAttribute>() != null;
        }

        public void RegisterGrammar(MetaGrammar metaGrammar)
        {
            if (metaGrammars.Any(_metaGrammar => _metaGrammar.GrammarType == metaGrammar.GrammarType))
                throw new ArgumentException("Grammar already registered", "metaGrammar");

            metaGrammars.Add(metaGrammar);
        }
    }
}
