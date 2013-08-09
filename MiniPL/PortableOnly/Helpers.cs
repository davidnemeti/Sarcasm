#if PCL

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace System.CodeDom.Compiler
{
    public class CompilerErrorCollection : List<CompilerError>
    {
    }

    public class CompilerError
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public string ErrorNumber { get; set; }
        public bool IsWarning { get; set; }
        public string ErrorText { get; set; }
        public string FileName { get; set; }

        public CompilerError()
        {
            this.Line = 0;
            this.Column = 0;
            this.ErrorNumber = string.Empty;
            this.ErrorText = string.Empty;
            this.FileName = string.Empty;
            this.IsWarning = false;
        }

        public CompilerError(string fileName, int line, int column, string errorNumber, string errorText)
        {
            this.Line = line;
            this.Column = column;
            this.ErrorNumber = errorNumber;
            this.ErrorText = errorText;
            this.FileName = fileName;
        }

        public override string ToString()
        {
            if (this.FileName.Length > 0)
                return string.Format(CultureInfo.InvariantCulture, "{0}({1},{2}) : {3} {4}: {5}", (object)this.FileName, (object)this.Line, (object)this.Column, this.IsWarning ? (object)"warning" : (object)"error", (object)this.ErrorNumber, (object)this.ErrorText);
            else
                return string.Format(CultureInfo.InvariantCulture, "{0} {1}: {2}", this.IsWarning ? (object)"warning" : (object)"error", (object)this.ErrorNumber, (object)this.ErrorText);
        }
    }
}

#endif
