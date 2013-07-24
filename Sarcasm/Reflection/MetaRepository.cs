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
    public class MetaRepository
    {
        public MetaRepository()
        {
            Domains = Util.CreateAndGetReadonlyCollection(out domains);
        }

        private ObservableCollection<Domain> domains;
        public ReadOnlyObservableCollection<Domain> Domains { get; private set; }

        public void RegisterDomain(Type domainRoot)
        {
            RegisterDomain(new Domain(domainRoot));
        }

        public void RegisterDomain(Domain domain)
        {
            domains.Add(domain);
        }

        public Domain DomainRootToDomain(Type domainRoot)
        {
            return Domains.First(domain => domain.DomainRoot == domainRoot);
        }

        public MetaGrammar GrammarTypeToMetaGrammar(Type grammarType)
        {
            return Domains
                .SelectMany(domain => domain.MetaGrammars)
                .First(metaGrammar => metaGrammar.GrammarType == grammarType);
        }

        public void RegisterAll(Assembly assembly)
        {
            RegisterDomains(assembly);
            RegisterGrammars(assembly);
            RegisterFormatters(assembly);
        }

        private void RegisterDomains(Assembly assembly)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            var newDomains = assembly
                .GetTypes()
                .Where(type => Domain.IsDomainRoot(type))
                .Select(domainRoot => new Domain(domainRoot));

            foreach (Domain newDomain in newDomains)
                RegisterDomain(newDomain);

            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        }

        private void RegisterGrammars(Assembly assembly)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            var newMetaGrammars = assembly
                .GetTypes()
                .Where(type => MetaGrammar.IsGrammarType(type))
                .Select(grammarType => new MetaGrammar(grammarType));

            foreach (MetaGrammar newMetaGrammar in newMetaGrammars)
                DomainRootToDomain(newMetaGrammar.DomainRoot).RegisterGrammar(newMetaGrammar);

            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        }

        private void RegisterFormatters(Assembly assembly)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            var newMetaFormatters = assembly
                .GetTypes()
                .Where(type => MetaFormatter.IsFormatterType(type))
                .Select(formatterType => new MetaFormatter(formatterType));

            foreach (MetaFormatter newMetaFormatter in newMetaFormatters)
                GrammarTypeToMetaGrammar(newMetaFormatter.GrammarType).RegisterFormatter(newMetaFormatter);

            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AppDomain domain = (AppDomain)sender;
            string nameWithMainVersionNumbers = GetAssemblyNameWithMainVersionNumbers(args.Name);

            foreach (var assembly in domain.GetAssemblies())
            {
                if (GetAssemblyNameWithMainVersionNumbers(assembly.FullName) == nameWithMainVersionNumbers)
                    return assembly;
            }

            return null;
        }

        private static string GetAssemblyNameWithMainVersionNumbers(string fullName)
        {
            return Regex.Match(fullName, @"(.*, Version=\d*\.\d*)").Groups[1].Value;
        }
    }

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

    public class MetaGrammar
    {
        public Type GrammarType { get; private set; }
        public GrammarAttribute GrammarAttribute { get; private set; }
        public Grammar Grammar { get; private set; }
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
            this.Grammar = (Grammar)Activator.CreateInstance(grammarType);
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
                throw new ArgumentException("Formatter already registered", "metaFormatter");

            metaFormatters.Add(metaFormatter);
        }
    }

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
