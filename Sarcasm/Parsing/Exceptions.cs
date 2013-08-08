using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.GrammarAst;

using Grammar = Sarcasm.GrammarAst.Grammar;
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
