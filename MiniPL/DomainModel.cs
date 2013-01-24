using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainCore;

namespace MiniPL.DomainModel
{
    public class Program
    {
        public IList<NamespaceUsing> NamespaceUsings { get; set; }
        public IList<Namespace> Namespaces { get; set; }
    }

    public class Namespace
    {
        public Name Name { get; set; }
        public IList<Definition> Definitions { get; set; }
    }

    public class NamespaceUsing
    {
        public NameRef NameRef { get; set; }
    }

    public abstract class Definition
    {
        public Name Name { get; set; }
    }
}
