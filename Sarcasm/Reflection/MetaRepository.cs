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
            UniversalMetaGrammars = Util.CreateAndGetReadonlyCollection(out universalMetaGrammars);
        }

        private ObservableCollection<Domain> domains;
        public ReadOnlyObservableCollection<Domain> Domains { get; private set; }

        private ObservableCollection<MetaGrammar> universalMetaGrammars;
        public ReadOnlyObservableCollection<MetaGrammar> UniversalMetaGrammars { get; private set; }

        public Domain DomainRootToDomain(Type domainRoot)
        {
            return Domains.First(domain => domain.DomainRoot == domainRoot);
        }

        public MetaGrammar GrammarTypeToMetaGrammar(Type grammarType)
        {
            return Domains
                .SelectMany(domain => domain.MetaGrammars)
                .Concat(UniversalMetaGrammars)
                .First(metaGrammar => metaGrammar.GrammarType == grammarType);
        }

        public void RegisterAll(Assembly assembly)
        {
            RegisterDomains(assembly);
            RegisterGrammars(assembly);
            RegisterFormatters(assembly);
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

        public void RegisterDomain(Domain domain)
        {
            domains.Add(domain);
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

        public void RegisterGrammar(MetaGrammar metaGrammar)
        {
            if (metaGrammar.IsUniversalGrammar())
                RegisterUniversalGrammar(metaGrammar);
            else
                DomainRootToDomain(metaGrammar.DomainRoot).RegisterGrammar(metaGrammar);
        }

        private void RegisterUniversalGrammar(MetaGrammar metaGrammar)
        {
            if (universalMetaGrammars.Any(_metaGrammar => _metaGrammar.GrammarType == metaGrammar.GrammarType))
                throw new ArgumentException("Grammar already registered", "metaGrammar");

            universalMetaGrammars.Add(metaGrammar);
        }

        public void RegisterFormatters(Assembly assembly)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            var newMetaFormatters = assembly
                .GetTypes()
                .Where(type => MetaFormatter.IsFormatterType(type))
                .Select(formatterType => new MetaFormatter(formatterType));

            foreach (MetaFormatter newMetaFormatter in newMetaFormatters)
                RegisterFormatter(newMetaFormatter);

            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        }

        public void RegisterFormatter(MetaFormatter metaFormatter)
        {
            GrammarTypeToMetaGrammar(metaFormatter.GrammarType).RegisterFormatter(metaFormatter);
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
}
