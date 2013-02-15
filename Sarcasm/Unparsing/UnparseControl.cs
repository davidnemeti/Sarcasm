using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grammar = Sarcasm.Ast.Grammar;

namespace Sarcasm.Unparsing
{
    public class UnparseControl
    {
        private readonly Grammar grammar;

        public UnparseControl(Grammar grammar)
        {
            this.grammar = grammar;
        }

        private Formatting _defaultFormatting;
        public Formatting DefaultFormatting
        {
            get
            {
                if (_defaultFormatting == null)
                    _defaultFormatting = new Formatting(grammar);

                return _defaultFormatting;
            }
        }
    }
}
