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
using System.Reflection;

using Sarcasm.Unparsing;

namespace Sarcasm.Reflection
{
    public class MetaFormatter
    {
        public Type FormatterType { get; private set; }
        public FormatterAttribute FormatterAttribute { get; private set; }
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

        public Formatter CreateFormatter()
        {
            return (Formatter)Activator.CreateInstance(FormatterType);
        }
    }
}
