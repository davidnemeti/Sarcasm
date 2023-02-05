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

using Irony;
using Irony.Parsing;

using System.Runtime.Serialization;

namespace Sarcasm.Parsing
{
    [Serializable]
    public class AstException : Exception
    {
        public AstException()
        {
        }

        public AstException(string message)
            : base(message)
        {
        }

        protected AstException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        internal readonly static ErrorLevel ErrorLevel = ErrorLevel.Error;
    }

    [Serializable]
    public class FatalAstException : Exception
    {
        // NOTE: Irony.Parsing.SourceLocation struct is not serializable (we serialize it manually)
        [NonSerialized]
        private SourceLocation location;
        public SourceLocation Location { get { return location; } internal set { location = value; } }

        public FatalAstException()
        {
        }

        public FatalAstException(string message)
            : base(message)
        {
        }

        protected FatalAstException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            location = new SourceLocation(
                position: info.GetInt32(locationPositionStr),
                line: info.GetInt32(locationLineStr),
                column: info.GetInt32(locationColumnStr)
                );
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(locationPositionStr, location.Position);
            info.AddValue(locationLineStr, location.Line);
            info.AddValue(locationColumnStr, location.Column);
        }

        private const string locationPositionStr = "Position";
        private const string locationLineStr = "Line";
        private const string locationColumnStr = "Column";

        internal readonly static ErrorLevel ErrorLevel = ErrorLevel.Error;
    }
}
