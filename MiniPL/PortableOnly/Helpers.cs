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
