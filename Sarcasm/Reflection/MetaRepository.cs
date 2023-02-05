#region License
/*
    This file is part of Sarcasm.

    Copyright 2012-2013 Dávid Németi

    Sarcasm is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Sarcasm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Sarcasm.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

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
            var newMetaDomains = assembly
                .GetTypes()
                .Where(type => MetaDomain.IsDomain(type))
                .Select(domain => new MetaDomain(domain));

            foreach (MetaDomain newMetaDomain in newMetaDomains)
                RegisterDomain(newMetaDomain);
        }

        public void RegisterDomain(MetaDomain metaDomain)
        {
            metaDomains.Add(metaDomain);
        }

        public void RegisterGrammars(Assembly assembly)
        {
            var newMetaGrammars = assembly
                .GetTypes()
                .Where(type => MetaGrammar.IsGrammarType(type))
                .Select(grammarType => new MetaGrammar(grammarType));

            foreach (MetaGrammar newMetaGrammar in newMetaGrammars)
                RegisterGrammar(newMetaGrammar);
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
            var newMetaCodeGenerators = assembly
                .GetTypes()
                .Where(type => MetaCodeGenerator.IsCodeGeneratorType(type))
                .Select(codeGeneratorType => new MetaCodeGenerator(codeGeneratorType));

            foreach (MetaCodeGenerator metaCodeGenerator in newMetaCodeGenerators)
                RegisterCodeGenerator(metaCodeGenerator);
        }

        public void RegisterCodeGenerator(MetaCodeGenerator metaCodeGenerator)
        {
            DomainTypeToMetaDomain(metaCodeGenerator.DomainType).RegisterCodeGenerator(metaCodeGenerator);
        }

        public void RegisterFormatters(Assembly assembly)
        {
            var newMetaFormatters = assembly
                .GetTypes()
                .Where(type => MetaFormatter.IsFormatterType(type))
                .Select(formatterType => new MetaFormatter(formatterType));

            foreach (MetaFormatter newMetaFormatter in newMetaFormatters)
                RegisterFormatter(newMetaFormatter);
        }

        public void RegisterFormatter(MetaFormatter metaFormatter)
        {
            GrammarTypeToMetaGrammar(metaFormatter.GrammarType).RegisterFormatter(metaFormatter);
        }
    }
}
