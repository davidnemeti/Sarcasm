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

namespace Sarcasm.Reflection
{
    public class MetaRepository
    {
        public MetaRepository()
        {
            domains = new ObservableCollectionAndReadOnly<Domain>();
            domainRoots = new ObservableCollectionAndReadOnly<Type>();
            domainToMetaGrammars = new Dictionary<Domain, ObservableCollectionAndReadOnly<MetaGrammar>>();
            domainRootToDomain = new Dictionary<Type, Domain>();
            grammarTypeToMetaGrammar = new Dictionary<Type, MetaGrammar>();
            metaGrammarsWithNonRegisteredDomains = new ObservableCollectionAndReadOnly<MetaGrammar>();
        }

        public IReadOnlyCollection<Domain> Domains { get { return domains.ReadOnlyItems; } }
        public IReadOnlyCollection<Type> DomainRoots { get { return domainRoots.ReadOnlyItems; } }

        private ObservableCollectionAndReadOnly<Domain> domains;
        private ObservableCollectionAndReadOnly<Type> domainRoots;

        private Dictionary<Domain, ObservableCollectionAndReadOnly<MetaGrammar>> domainToMetaGrammars;
        private Dictionary<Type, Domain> domainRootToDomain;
        private Dictionary<Type, MetaGrammar> grammarTypeToMetaGrammar;

        private ObservableCollectionAndReadOnly<MetaGrammar> metaGrammarsWithNonRegisteredDomains;

        public ReadOnlyObservableCollection<MetaGrammar> GetGrammars(Domain domain)
        {
            return domainToMetaGrammars[domain].ReadOnlyItems;
        }

        public ReadOnlyObservableCollection<MetaGrammar> GetGrammars(Type domainRoot)
        {
            return domainToMetaGrammars[domainRootToDomain[domainRoot]].ReadOnlyItems;
        }

        private class ObservableCollectionAndReadOnly<T>
        {
            public readonly ReadOnlyObservableCollection<T> ReadOnlyItems;
            public readonly ObservableCollection<T> Items;

            public ObservableCollectionAndReadOnly()
            {
                Items = new ObservableCollection<T>();
                ReadOnlyItems = new ReadOnlyObservableCollection<T>(Items);
            }
        }

        public void RegisterDomain(Type domainRoot)
        {
            RegisterDomain(new Domain(domainRoot));
        }

        public void RegisterDomain(Domain domain)
        {
            domainToMetaGrammars.Add(domain, new ObservableCollectionAndReadOnly<MetaGrammar>());
            domainRootToDomain.Add(domain.DomainRoot, domain);

            domains.Items.Add(domain);
            domainRoots.Items.Add(domain.DomainRoot);

            var registeredMetaGrammarsForThisDomain = metaGrammarsWithNonRegisteredDomains.Items.FirstOrDefault(metaGrammar => metaGrammar.DomainRoot == domain.DomainRoot);

            if (registeredMetaGrammarsForThisDomain != null)
            {
                metaGrammarsWithNonRegisteredDomains.Items.Remove(registeredMetaGrammarsForThisDomain);
                domainToMetaGrammars[domain].Items.Add(registeredMetaGrammarsForThisDomain);
            }

        }

        public bool IsDomainRegistered(Type domainRoot)
        {
            return domainRootToDomain.ContainsKey(domainRoot);
        }

        public bool IsDomainRegistered(Domain domain)
        {
            return IsDomainRegistered(domain.DomainRoot);
        }

        public void RegisterGrammar(Type grammarType)
        {
            RegisterGrammar(new MetaGrammar(grammarType));
        }

        public void RegisterGrammar(MetaGrammar metaGrammar)
        {
            if (IsGrammarRegistered(metaGrammar))
                throw new ArgumentException("Grammar already registered", "metaGrammar");

            if (IsDomainRegistered(metaGrammar.DomainRoot))
                domainToMetaGrammars[domainRootToDomain[metaGrammar.DomainRoot]].Items.Add(metaGrammar);
            else
                metaGrammarsWithNonRegisteredDomains.Items.Add(metaGrammar);

            grammarTypeToMetaGrammar.Add(metaGrammar.GrammarType, metaGrammar);
        }

        public bool IsGrammarRegistered(Type grammarType)
        {
            return grammarTypeToMetaGrammar.ContainsKey(grammarType) || metaGrammarsWithNonRegisteredDomains.Items.Any(metaGrammar => metaGrammar.GrammarType == grammarType);
        }

        public bool IsGrammarRegistered(MetaGrammar metaGrammar)
        {
            return IsGrammarRegistered(metaGrammar.GrammarType);
        }

        public void RegisterDomains(Assembly assembly)
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

        public void RegisterGrammars(Assembly assembly)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            var newMetaGrammars = assembly
                .GetTypes()
                .Where(type => MetaGrammar.IsGrammarType(type))
                .Select(grammarType => new MetaGrammar(grammarType));

            foreach (MetaGrammar newMetaGrammar in newMetaGrammars)
                RegisterGrammar(newMetaGrammar);

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

        public string Name { get { return DomainRootAttribute.Name; } }

        public Domain(Type domainRoot)
        {
            var domainRootAttribute = domainRoot.GetCustomAttribute<DomainRootAttribute>();

            if (!IsDomainRoot(domainRoot))
                throw new ArgumentException("Type should be a domain root, i.e. a type with DomainRootAttribute", "type");

            this.DomainRoot = domainRoot;
            this.DomainRootAttribute = domainRootAttribute;
        }

        public static bool IsDomainRoot(Type type)
        {
            return type.GetCustomAttribute<DomainRootAttribute>() != null;
        }
    }

    public class MetaGrammar
    {
        public Type GrammarType { get; private set; }
        public GrammarAttribute GrammarAttribute { get; private set; }
        public Grammar Grammar { get; private set; }
        public Type DomainRoot { get; private set; }

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
        }

        public static bool IsGrammarType(Type type)
        {
            return type.IsSubclassOf(typeof(Grammar)) && type.GetCustomAttribute<GrammarAttribute>() != null;
        }
    }
}
