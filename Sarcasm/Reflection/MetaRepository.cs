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
            MetaDomains = Util.CreateAndGetReadonlyCollection(out metaDomains);
            UniversalMetaGrammars = Util.CreateAndGetReadonlyCollection(out universalMetaGrammars);
        }

        private ObservableCollection<MetaDomain> metaDomains;
        public ReadOnlyObservableCollection<MetaDomain> MetaDomains { get; private set; }

        private ObservableCollection<MetaGrammar> universalMetaGrammars;
        public ReadOnlyObservableCollection<MetaGrammar> UniversalMetaGrammars { get; private set; }

        public MetaDomain DomainTypeToMetaDomain(Type domainType)
        {
            return MetaDomains.First(metaDomain => metaDomain.DomainType == domainType);
        }

        public MetaGrammar GrammarTypeToMetaGrammar(Type grammarType)
        {
            return MetaDomains
                .SelectMany(domain => domain.MetaGrammars)
                .Concat(UniversalMetaGrammars)
                .First(metaGrammar => metaGrammar.GrammarType == grammarType);
        }

        public void RegisterAll(Assembly assembly)
        {
            RegisterDomains(assembly);
            RegisterCodeGenerators(assembly);
            RegisterGrammars(assembly);
            RegisterFormatters(assembly);
        }

        public void RegisterDomains(Assembly assembly)
        {
            RegisterAssemblyResolver();

            var newMetaDomains = assembly
                .GetTypes()
                .Where(type => MetaDomain.IsDomain(type))
                .Select(domain => new MetaDomain(domain));

            foreach (MetaDomain newMetaDomain in newMetaDomains)
                RegisterDomain(newMetaDomain);

            UnregisterAssemblyResolver();
        }

        public void RegisterDomain(MetaDomain metaDomain)
        {
            metaDomains.Add(metaDomain);
        }

        public void RegisterGrammars(Assembly assembly)
        {
            RegisterAssemblyResolver();

            var newMetaGrammars = assembly
                .GetTypes()
                .Where(type => MetaGrammar.IsGrammarType(type))
                .Select(grammarType => new MetaGrammar(grammarType));

            foreach (MetaGrammar newMetaGrammar in newMetaGrammars)
                RegisterGrammar(newMetaGrammar);

            UnregisterAssemblyResolver();
        }

        public void RegisterGrammar(MetaGrammar metaGrammar)
        {
            if (metaGrammar.IsUniversalGrammar())
                RegisterUniversalGrammar(metaGrammar);
            else
                DomainTypeToMetaDomain(metaGrammar.DomainType).RegisterGrammar(metaGrammar);
        }

        private void RegisterUniversalGrammar(MetaGrammar metaGrammar)
        {
            if (universalMetaGrammars.Any(_metaGrammar => _metaGrammar.GrammarType == metaGrammar.GrammarType))
                throw new ArgumentException("Grammar already registered " + metaGrammar.Name, "metaGrammar");

            universalMetaGrammars.Add(metaGrammar);
        }

        public void RegisterCodeGenerators(Assembly assembly)
        {
            RegisterAssemblyResolver();

            var newMetaCodeGenerators = assembly
                .GetTypes()
                .Where(type => MetaCodeGenerator.IsCodeGeneratorType(type))
                .Select(codeGeneratorType => new MetaCodeGenerator(codeGeneratorType));

            foreach (MetaCodeGenerator metaCodeGenerator in newMetaCodeGenerators)
                RegisterCodeGenerator(metaCodeGenerator);

            UnregisterAssemblyResolver();
        }

        public void RegisterCodeGenerator(MetaCodeGenerator metaCodeGenerator)
        {
            DomainTypeToMetaDomain(metaCodeGenerator.DomainType).RegisterCodeGenerator(metaCodeGenerator);
        }

        public void RegisterFormatters(Assembly assembly)
        {
            RegisterAssemblyResolver();

            var newMetaFormatters = assembly
                .GetTypes()
                .Where(type => MetaFormatter.IsFormatterType(type))
                .Select(formatterType => new MetaFormatter(formatterType));

            foreach (MetaFormatter newMetaFormatter in newMetaFormatters)
                RegisterFormatter(newMetaFormatter);

            UnregisterAssemblyResolver();
        }

        public void RegisterFormatter(MetaFormatter metaFormatter)
        {
            GrammarTypeToMetaGrammar(metaFormatter.GrammarType).RegisterFormatter(metaFormatter);
        }

        internal static void EnsureThatAssemblyResolverIsRegistered()
        {
            if (assemblyResolverRegistrationCount == 0)
                RegisterAssemblyResolver();
        }

        internal static void RegisterAssemblyResolver()
        {
#if !PCL
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            assemblyResolverRegistrationCount++;
#endif
        }

        internal static void UnregisterAssemblyResolver()
        {
#if !PCL
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
            assemblyResolverRegistrationCount--;
#endif
        }

        private static int assemblyResolverRegistrationCount = 0;

#if !PCL
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
#endif
    }
}
