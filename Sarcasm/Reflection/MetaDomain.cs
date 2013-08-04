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
    public class MetaDomain
    {
        public Type DomainType { get; private set; }
        public DomainAttribute DomainAttribute { get; private set; }

        private ObservableCollection<MetaGrammar> metaGrammars;
        public ReadOnlyObservableCollection<MetaGrammar> MetaGrammars { get; private set; }

        private ObservableCollection<MetaCodeGenerator> metaCodeGenerators;
        public ReadOnlyObservableCollection<MetaCodeGenerator> MetaCodeGenerators { get; private set; }

        public string Name { get { return DomainAttribute.Name; } }

        public MetaDomain(Type domainType)
        {
            var domainAttribute = domainType.GetCustomAttribute<DomainAttribute>();

            if (!IsDomain(domainType))
                throw new ArgumentException("Type should be a domain, i.e. a type with DomainAttribute", "type");

            this.DomainType = domainType;
            this.DomainAttribute = domainAttribute;

            MetaGrammars = Util.CreateAndGetReadonlyCollection(out metaGrammars);
            MetaCodeGenerators = Util.CreateAndGetReadonlyCollection(out metaCodeGenerators);
        }

        public static bool IsDomain(Type type)
        {
            return type.GetCustomAttribute<DomainAttribute>() != null;
        }

        public void RegisterGrammar(MetaGrammar metaGrammar)
        {
            if (metaGrammars.Any(_metaGrammar => _metaGrammar.GrammarType == metaGrammar.GrammarType))
                throw new ArgumentException("Grammar already registered " + metaGrammar.Name, "metaGrammar");

            metaGrammars.Add(metaGrammar);
        }

        public void RegisterCodeGenerator(MetaCodeGenerator metaCodeGenerator)
        {
            if (metaCodeGenerators.Any(_metaCodeGenerator => _metaCodeGenerator.CodeGeneratorType == metaCodeGenerator.CodeGeneratorType))
                throw new ArgumentException("CodeGenerator already registered " + metaCodeGenerator.Name, "metaCodeGenerator");

            metaCodeGenerators.Add(metaCodeGenerator);
        }
    }
}
