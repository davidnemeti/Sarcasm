 
// GENERATED FILE

using System;
using System.Collections.Generic;
using System.Linq;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Sarcasm.GrammarAst
{
	#region Parse Object (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValueTL ParseDataLiteral(DataLiteralBase dataLiteral)
        {
            return Parse(dataLiteral, (context, parseNode) => { return (Object)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Object, object>, astForChild: false).MakeUncontractible();
        }

		public static BnfiTermValueTL ParseNumberLiteral(NumberLiteral numberLiteral)
        {
            return Parse(numberLiteral, (context, parseNode) => { return (Object)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Object, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValueTL ParseDataLiteral(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteral(dataLiteral);
        }

		public static BnfiTermValueTL ParseNumberLiteral(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberLiteral(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValueTL ParseDataLiteral(string name = "dataliteral")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Object).ParseDataLiteral();
		}

        public static BnfiTermValueTL ParseDataLiteralDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Object).ParseDataLiteral();
		}

        public static BnfiTermValueTL ParseDataLiteralDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Object, terminator: terminator).ParseDataLiteral();
		}

        public static BnfiTermValueTL ParseDataLiteralQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Object, startEndSymbol: startEndSymbol).ParseDataLiteral();
		}

        public static BnfiTermValueTL ParseDataLiteralQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Object, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteral();
		}

        public static BnfiTermValueTL ParseDataLiteralFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Object, length: length).ParseDataLiteral();
		}

		#endregion

		#region Number

		public static BnfiTermValueTL CreateNumberLiteral(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).ParseNumberLiteral();
        }

		public static BnfiTermValueTL CreateNumberLiteral(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).ParseNumberLiteral();
        }

		public static BnfiTermValueTL CreateNumberLiteral(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberLiteral();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse DBNull (Data)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<DBNull> ParseDataLiteralDBNull(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.DBNull)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a DBNull", dataLiteral.Name);

            return Parse<DBNull>(dataLiteral, (context, parseNode) => { return (DBNull)parseNode.FindToken().Value; }, IdentityFunctionForceCast<DBNull, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<DBNull> ParseDataLiteralDBNull(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteralDBNull(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<DBNull> ParseDataLiteralDBNull(string name = "dataliteralDBNull")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.DBNull).ParseDataLiteralDBNull();
		}

        public static BnfiTermValue<DBNull> ParseDataLiteralDBNullDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.DBNull).ParseDataLiteralDBNull();
		}

        public static BnfiTermValue<DBNull> ParseDataLiteralDBNullDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.DBNull, terminator: terminator).ParseDataLiteralDBNull();
		}

        public static BnfiTermValue<DBNull> ParseDataLiteralDBNullQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.DBNull, startEndSymbol: startEndSymbol).ParseDataLiteralDBNull();
		}

        public static BnfiTermValue<DBNull> ParseDataLiteralDBNullQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.DBNull, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteralDBNull();
		}

        public static BnfiTermValue<DBNull> ParseDataLiteralDBNullFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.DBNull, length: length).ParseDataLiteralDBNull();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Parse Boolean (Data)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Boolean> ParseDataLiteralBoolean(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Boolean)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Boolean", dataLiteral.Name);

            return Parse<Boolean>(dataLiteral, (context, parseNode) => { return (Boolean)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Boolean, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Boolean> ParseDataLiteralBoolean(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteralBoolean(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Boolean> ParseDataLiteralBoolean(string name = "dataliteralBoolean")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Boolean).ParseDataLiteralBoolean();
		}

        public static BnfiTermValue<Boolean> ParseDataLiteralBooleanDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Boolean).ParseDataLiteralBoolean();
		}

        public static BnfiTermValue<Boolean> ParseDataLiteralBooleanDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Boolean, terminator: terminator).ParseDataLiteralBoolean();
		}

        public static BnfiTermValue<Boolean> ParseDataLiteralBooleanQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Boolean, startEndSymbol: startEndSymbol).ParseDataLiteralBoolean();
		}

        public static BnfiTermValue<Boolean> ParseDataLiteralBooleanQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Boolean, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteralBoolean();
		}

        public static BnfiTermValue<Boolean> ParseDataLiteralBooleanFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Boolean, length: length).ParseDataLiteralBoolean();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Parse Char (Data)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Char> ParseDataLiteralChar(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Char)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Char", dataLiteral.Name);

            return Parse<Char>(dataLiteral, (context, parseNode) => { return (Char)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Char, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Char> ParseDataLiteralChar(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteralChar(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Char> ParseDataLiteralChar(string name = "dataliteralChar")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Char).ParseDataLiteralChar();
		}

        public static BnfiTermValue<Char> ParseDataLiteralCharDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Char).ParseDataLiteralChar();
		}

        public static BnfiTermValue<Char> ParseDataLiteralCharDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Char, terminator: terminator).ParseDataLiteralChar();
		}

        public static BnfiTermValue<Char> ParseDataLiteralCharQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Char, startEndSymbol: startEndSymbol).ParseDataLiteralChar();
		}

        public static BnfiTermValue<Char> ParseDataLiteralCharQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Char, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteralChar();
		}

        public static BnfiTermValue<Char> ParseDataLiteralCharFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Char, length: length).ParseDataLiteralChar();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Parse SByte (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<SByte> ParseDataLiteralSByte(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.SByte)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a SByte", dataLiteral.Name);

            return Parse<SByte>(dataLiteral, (context, parseNode) => { return (SByte)parseNode.FindToken().Value; }, IdentityFunctionForceCast<SByte, object>, astForChild: false).MakeUncontractible();
        }

		public static BnfiTermValue<SByte> ParseNumberLiteralSByte(NumberLiteral numberLiteral)
        {
            return Parse<SByte>(numberLiteral, (context, parseNode) => { return (SByte)parseNode.FindToken().Value; }, IdentityFunctionForceCast<SByte, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<SByte> ParseDataLiteralSByte(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteralSByte(dataLiteral);
        }

		public static BnfiTermValue<SByte> ParseNumberLiteralSByte(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberLiteralSByte(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<SByte> ParseDataLiteralSByte(string name = "dataliteralSByte")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.SByte).ParseDataLiteralSByte();
		}

        public static BnfiTermValue<SByte> ParseDataLiteralSByteDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.SByte).ParseDataLiteralSByte();
		}

        public static BnfiTermValue<SByte> ParseDataLiteralSByteDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.SByte, terminator: terminator).ParseDataLiteralSByte();
		}

        public static BnfiTermValue<SByte> ParseDataLiteralSByteQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.SByte, startEndSymbol: startEndSymbol).ParseDataLiteralSByte();
		}

        public static BnfiTermValue<SByte> ParseDataLiteralSByteQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.SByte, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteralSByte();
		}

        public static BnfiTermValue<SByte> ParseDataLiteralSByteFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.SByte, length: length).ParseDataLiteralSByte();
		}

		#endregion

		#region Number

		public static BnfiTermValue<SByte> CreateNumberLiteralSByte(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).ParseNumberLiteralSByte();
        }

		public static BnfiTermValue<SByte> CreateNumberLiteralSByte(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).ParseNumberLiteralSByte();
        }

		public static BnfiTermValue<SByte> CreateNumberLiteralSByte(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberLiteralSByte();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse Byte (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Byte> ParseDataLiteralByte(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Byte)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Byte", dataLiteral.Name);

            return Parse<Byte>(dataLiteral, (context, parseNode) => { return (Byte)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Byte, object>, astForChild: false).MakeUncontractible();
        }

		public static BnfiTermValue<Byte> ParseNumberLiteralByte(NumberLiteral numberLiteral)
        {
            return Parse<Byte>(numberLiteral, (context, parseNode) => { return (Byte)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Byte, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Byte> ParseDataLiteralByte(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteralByte(dataLiteral);
        }

		public static BnfiTermValue<Byte> ParseNumberLiteralByte(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberLiteralByte(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Byte> ParseDataLiteralByte(string name = "dataliteralByte")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Byte).ParseDataLiteralByte();
		}

        public static BnfiTermValue<Byte> ParseDataLiteralByteDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Byte).ParseDataLiteralByte();
		}

        public static BnfiTermValue<Byte> ParseDataLiteralByteDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Byte, terminator: terminator).ParseDataLiteralByte();
		}

        public static BnfiTermValue<Byte> ParseDataLiteralByteQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Byte, startEndSymbol: startEndSymbol).ParseDataLiteralByte();
		}

        public static BnfiTermValue<Byte> ParseDataLiteralByteQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Byte, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteralByte();
		}

        public static BnfiTermValue<Byte> ParseDataLiteralByteFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Byte, length: length).ParseDataLiteralByte();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Byte> CreateNumberLiteralByte(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).ParseNumberLiteralByte();
        }

		public static BnfiTermValue<Byte> CreateNumberLiteralByte(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).ParseNumberLiteralByte();
        }

		public static BnfiTermValue<Byte> CreateNumberLiteralByte(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberLiteralByte();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse Int16 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Int16> ParseDataLiteralInt16(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Int16)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Int16", dataLiteral.Name);

            return Parse<Int16>(dataLiteral, (context, parseNode) => { return (Int16)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int16, object>, astForChild: false).MakeUncontractible();
        }

		public static BnfiTermValue<Int16> ParseNumberLiteralInt16(NumberLiteral numberLiteral)
        {
            return Parse<Int16>(numberLiteral, (context, parseNode) => { return (Int16)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int16, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Int16> ParseDataLiteralInt16(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteralInt16(dataLiteral);
        }

		public static BnfiTermValue<Int16> ParseNumberLiteralInt16(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberLiteralInt16(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Int16> ParseDataLiteralInt16(string name = "dataliteralInt16")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Int16).ParseDataLiteralInt16();
		}

        public static BnfiTermValue<Int16> ParseDataLiteralInt16Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int16).ParseDataLiteralInt16();
		}

        public static BnfiTermValue<Int16> ParseDataLiteralInt16Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int16, terminator: terminator).ParseDataLiteralInt16();
		}

        public static BnfiTermValue<Int16> ParseDataLiteralInt16Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int16, startEndSymbol: startEndSymbol).ParseDataLiteralInt16();
		}

        public static BnfiTermValue<Int16> ParseDataLiteralInt16Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int16, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteralInt16();
		}

        public static BnfiTermValue<Int16> ParseDataLiteralInt16FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Int16, length: length).ParseDataLiteralInt16();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Int16> CreateNumberLiteralInt16(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).ParseNumberLiteralInt16();
        }

		public static BnfiTermValue<Int16> CreateNumberLiteralInt16(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).ParseNumberLiteralInt16();
        }

		public static BnfiTermValue<Int16> CreateNumberLiteralInt16(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberLiteralInt16();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse UInt16 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<UInt16> ParseDataLiteralUInt16(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.UInt16)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a UInt16", dataLiteral.Name);

            return Parse<UInt16>(dataLiteral, (context, parseNode) => { return (UInt16)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt16, object>, astForChild: false).MakeUncontractible();
        }

		public static BnfiTermValue<UInt16> ParseNumberLiteralUInt16(NumberLiteral numberLiteral)
        {
            return Parse<UInt16>(numberLiteral, (context, parseNode) => { return (UInt16)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt16, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<UInt16> ParseDataLiteralUInt16(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteralUInt16(dataLiteral);
        }

		public static BnfiTermValue<UInt16> ParseNumberLiteralUInt16(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberLiteralUInt16(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<UInt16> ParseDataLiteralUInt16(string name = "dataliteralUInt16")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.UInt16).ParseDataLiteralUInt16();
		}

        public static BnfiTermValue<UInt16> ParseDataLiteralUInt16Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt16).ParseDataLiteralUInt16();
		}

        public static BnfiTermValue<UInt16> ParseDataLiteralUInt16Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt16, terminator: terminator).ParseDataLiteralUInt16();
		}

        public static BnfiTermValue<UInt16> ParseDataLiteralUInt16Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt16, startEndSymbol: startEndSymbol).ParseDataLiteralUInt16();
		}

        public static BnfiTermValue<UInt16> ParseDataLiteralUInt16Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt16, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteralUInt16();
		}

        public static BnfiTermValue<UInt16> ParseDataLiteralUInt16FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.UInt16, length: length).ParseDataLiteralUInt16();
		}

		#endregion

		#region Number

		public static BnfiTermValue<UInt16> CreateNumberLiteralUInt16(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).ParseNumberLiteralUInt16();
        }

		public static BnfiTermValue<UInt16> CreateNumberLiteralUInt16(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).ParseNumberLiteralUInt16();
        }

		public static BnfiTermValue<UInt16> CreateNumberLiteralUInt16(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberLiteralUInt16();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse Int32 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Int32> ParseDataLiteralInt32(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Int32)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Int32", dataLiteral.Name);

            return Parse<Int32>(dataLiteral, (context, parseNode) => { return (Int32)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int32, object>, astForChild: false).MakeUncontractible();
        }

		public static BnfiTermValue<Int32> ParseNumberLiteralInt32(NumberLiteral numberLiteral)
        {
            return Parse<Int32>(numberLiteral, (context, parseNode) => { return (Int32)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int32, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Int32> ParseDataLiteralInt32(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteralInt32(dataLiteral);
        }

		public static BnfiTermValue<Int32> ParseNumberLiteralInt32(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberLiteralInt32(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Int32> ParseDataLiteralInt32(string name = "dataliteralInt32")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Int32).ParseDataLiteralInt32();
		}

        public static BnfiTermValue<Int32> ParseDataLiteralInt32Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int32).ParseDataLiteralInt32();
		}

        public static BnfiTermValue<Int32> ParseDataLiteralInt32Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int32, terminator: terminator).ParseDataLiteralInt32();
		}

        public static BnfiTermValue<Int32> ParseDataLiteralInt32Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int32, startEndSymbol: startEndSymbol).ParseDataLiteralInt32();
		}

        public static BnfiTermValue<Int32> ParseDataLiteralInt32Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int32, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteralInt32();
		}

        public static BnfiTermValue<Int32> ParseDataLiteralInt32FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Int32, length: length).ParseDataLiteralInt32();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Int32> CreateNumberLiteralInt32(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).ParseNumberLiteralInt32();
        }

		public static BnfiTermValue<Int32> CreateNumberLiteralInt32(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).ParseNumberLiteralInt32();
        }

		public static BnfiTermValue<Int32> CreateNumberLiteralInt32(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberLiteralInt32();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse UInt32 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<UInt32> ParseDataLiteralUInt32(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.UInt32)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a UInt32", dataLiteral.Name);

            return Parse<UInt32>(dataLiteral, (context, parseNode) => { return (UInt32)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt32, object>, astForChild: false).MakeUncontractible();
        }

		public static BnfiTermValue<UInt32> ParseNumberLiteralUInt32(NumberLiteral numberLiteral)
        {
            return Parse<UInt32>(numberLiteral, (context, parseNode) => { return (UInt32)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt32, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<UInt32> ParseDataLiteralUInt32(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteralUInt32(dataLiteral);
        }

		public static BnfiTermValue<UInt32> ParseNumberLiteralUInt32(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberLiteralUInt32(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<UInt32> ParseDataLiteralUInt32(string name = "dataliteralUInt32")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.UInt32).ParseDataLiteralUInt32();
		}

        public static BnfiTermValue<UInt32> ParseDataLiteralUInt32Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt32).ParseDataLiteralUInt32();
		}

        public static BnfiTermValue<UInt32> ParseDataLiteralUInt32Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt32, terminator: terminator).ParseDataLiteralUInt32();
		}

        public static BnfiTermValue<UInt32> ParseDataLiteralUInt32Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt32, startEndSymbol: startEndSymbol).ParseDataLiteralUInt32();
		}

        public static BnfiTermValue<UInt32> ParseDataLiteralUInt32Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt32, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteralUInt32();
		}

        public static BnfiTermValue<UInt32> ParseDataLiteralUInt32FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.UInt32, length: length).ParseDataLiteralUInt32();
		}

		#endregion

		#region Number

		public static BnfiTermValue<UInt32> CreateNumberLiteralUInt32(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).ParseNumberLiteralUInt32();
        }

		public static BnfiTermValue<UInt32> CreateNumberLiteralUInt32(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).ParseNumberLiteralUInt32();
        }

		public static BnfiTermValue<UInt32> CreateNumberLiteralUInt32(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberLiteralUInt32();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse Int64 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Int64> ParseDataLiteralInt64(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Int64)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Int64", dataLiteral.Name);

            return Parse<Int64>(dataLiteral, (context, parseNode) => { return (Int64)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int64, object>, astForChild: false).MakeUncontractible();
        }

		public static BnfiTermValue<Int64> ParseNumberLiteralInt64(NumberLiteral numberLiteral)
        {
            return Parse<Int64>(numberLiteral, (context, parseNode) => { return (Int64)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int64, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Int64> ParseDataLiteralInt64(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteralInt64(dataLiteral);
        }

		public static BnfiTermValue<Int64> ParseNumberLiteralInt64(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberLiteralInt64(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Int64> ParseDataLiteralInt64(string name = "dataliteralInt64")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Int64).ParseDataLiteralInt64();
		}

        public static BnfiTermValue<Int64> ParseDataLiteralInt64Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int64).ParseDataLiteralInt64();
		}

        public static BnfiTermValue<Int64> ParseDataLiteralInt64Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int64, terminator: terminator).ParseDataLiteralInt64();
		}

        public static BnfiTermValue<Int64> ParseDataLiteralInt64Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int64, startEndSymbol: startEndSymbol).ParseDataLiteralInt64();
		}

        public static BnfiTermValue<Int64> ParseDataLiteralInt64Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int64, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteralInt64();
		}

        public static BnfiTermValue<Int64> ParseDataLiteralInt64FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Int64, length: length).ParseDataLiteralInt64();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Int64> CreateNumberLiteralInt64(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).ParseNumberLiteralInt64();
        }

		public static BnfiTermValue<Int64> CreateNumberLiteralInt64(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).ParseNumberLiteralInt64();
        }

		public static BnfiTermValue<Int64> CreateNumberLiteralInt64(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberLiteralInt64();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse UInt64 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<UInt64> ParseDataLiteralUInt64(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.UInt64)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a UInt64", dataLiteral.Name);

            return Parse<UInt64>(dataLiteral, (context, parseNode) => { return (UInt64)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt64, object>, astForChild: false).MakeUncontractible();
        }

		public static BnfiTermValue<UInt64> ParseNumberLiteralUInt64(NumberLiteral numberLiteral)
        {
            return Parse<UInt64>(numberLiteral, (context, parseNode) => { return (UInt64)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt64, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<UInt64> ParseDataLiteralUInt64(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteralUInt64(dataLiteral);
        }

		public static BnfiTermValue<UInt64> ParseNumberLiteralUInt64(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberLiteralUInt64(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<UInt64> ParseDataLiteralUInt64(string name = "dataliteralUInt64")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.UInt64).ParseDataLiteralUInt64();
		}

        public static BnfiTermValue<UInt64> ParseDataLiteralUInt64Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt64).ParseDataLiteralUInt64();
		}

        public static BnfiTermValue<UInt64> ParseDataLiteralUInt64Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt64, terminator: terminator).ParseDataLiteralUInt64();
		}

        public static BnfiTermValue<UInt64> ParseDataLiteralUInt64Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt64, startEndSymbol: startEndSymbol).ParseDataLiteralUInt64();
		}

        public static BnfiTermValue<UInt64> ParseDataLiteralUInt64Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt64, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteralUInt64();
		}

        public static BnfiTermValue<UInt64> ParseDataLiteralUInt64FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.UInt64, length: length).ParseDataLiteralUInt64();
		}

		#endregion

		#region Number

		public static BnfiTermValue<UInt64> CreateNumberLiteralUInt64(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).ParseNumberLiteralUInt64();
        }

		public static BnfiTermValue<UInt64> CreateNumberLiteralUInt64(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).ParseNumberLiteralUInt64();
        }

		public static BnfiTermValue<UInt64> CreateNumberLiteralUInt64(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberLiteralUInt64();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse Single (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Single> ParseDataLiteralSingle(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Single)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Single", dataLiteral.Name);

            return Parse<Single>(dataLiteral, (context, parseNode) => { return (Single)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Single, object>, astForChild: false).MakeUncontractible();
        }

		public static BnfiTermValue<Single> ParseNumberLiteralSingle(NumberLiteral numberLiteral)
        {
            return Parse<Single>(numberLiteral, (context, parseNode) => { return (Single)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Single, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Single> ParseDataLiteralSingle(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteralSingle(dataLiteral);
        }

		public static BnfiTermValue<Single> ParseNumberLiteralSingle(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberLiteralSingle(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Single> ParseDataLiteralSingle(string name = "dataliteralSingle")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Single).ParseDataLiteralSingle();
		}

        public static BnfiTermValue<Single> ParseDataLiteralSingleDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Single).ParseDataLiteralSingle();
		}

        public static BnfiTermValue<Single> ParseDataLiteralSingleDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Single, terminator: terminator).ParseDataLiteralSingle();
		}

        public static BnfiTermValue<Single> ParseDataLiteralSingleQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Single, startEndSymbol: startEndSymbol).ParseDataLiteralSingle();
		}

        public static BnfiTermValue<Single> ParseDataLiteralSingleQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Single, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteralSingle();
		}

        public static BnfiTermValue<Single> ParseDataLiteralSingleFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Single, length: length).ParseDataLiteralSingle();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Single> CreateNumberLiteralSingle(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).ParseNumberLiteralSingle();
        }

		public static BnfiTermValue<Single> CreateNumberLiteralSingle(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).ParseNumberLiteralSingle();
        }

		public static BnfiTermValue<Single> CreateNumberLiteralSingle(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberLiteralSingle();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse Double (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Double> ParseDataLiteralDouble(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Double)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Double", dataLiteral.Name);

            return Parse<Double>(dataLiteral, (context, parseNode) => { return (Double)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Double, object>, astForChild: false).MakeUncontractible();
        }

		public static BnfiTermValue<Double> ParseNumberLiteralDouble(NumberLiteral numberLiteral)
        {
            return Parse<Double>(numberLiteral, (context, parseNode) => { return (Double)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Double, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Double> ParseDataLiteralDouble(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteralDouble(dataLiteral);
        }

		public static BnfiTermValue<Double> ParseNumberLiteralDouble(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberLiteralDouble(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Double> ParseDataLiteralDouble(string name = "dataliteralDouble")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Double).ParseDataLiteralDouble();
		}

        public static BnfiTermValue<Double> ParseDataLiteralDoubleDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Double).ParseDataLiteralDouble();
		}

        public static BnfiTermValue<Double> ParseDataLiteralDoubleDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Double, terminator: terminator).ParseDataLiteralDouble();
		}

        public static BnfiTermValue<Double> ParseDataLiteralDoubleQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Double, startEndSymbol: startEndSymbol).ParseDataLiteralDouble();
		}

        public static BnfiTermValue<Double> ParseDataLiteralDoubleQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Double, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteralDouble();
		}

        public static BnfiTermValue<Double> ParseDataLiteralDoubleFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Double, length: length).ParseDataLiteralDouble();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Double> CreateNumberLiteralDouble(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).ParseNumberLiteralDouble();
        }

		public static BnfiTermValue<Double> CreateNumberLiteralDouble(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).ParseNumberLiteralDouble();
        }

		public static BnfiTermValue<Double> CreateNumberLiteralDouble(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberLiteralDouble();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse Decimal (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Decimal> ParseDataLiteralDecimal(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Decimal)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Decimal", dataLiteral.Name);

            return Parse<Decimal>(dataLiteral, (context, parseNode) => { return (Decimal)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Decimal, object>, astForChild: false).MakeUncontractible();
        }

		public static BnfiTermValue<Decimal> ParseNumberLiteralDecimal(NumberLiteral numberLiteral)
        {
            return Parse<Decimal>(numberLiteral, (context, parseNode) => { return (Decimal)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Decimal, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Decimal> ParseDataLiteralDecimal(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteralDecimal(dataLiteral);
        }

		public static BnfiTermValue<Decimal> ParseNumberLiteralDecimal(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberLiteralDecimal(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Decimal> ParseDataLiteralDecimal(string name = "dataliteralDecimal")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Decimal).ParseDataLiteralDecimal();
		}

        public static BnfiTermValue<Decimal> ParseDataLiteralDecimalDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Decimal).ParseDataLiteralDecimal();
		}

        public static BnfiTermValue<Decimal> ParseDataLiteralDecimalDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Decimal, terminator: terminator).ParseDataLiteralDecimal();
		}

        public static BnfiTermValue<Decimal> ParseDataLiteralDecimalQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Decimal, startEndSymbol: startEndSymbol).ParseDataLiteralDecimal();
		}

        public static BnfiTermValue<Decimal> ParseDataLiteralDecimalQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Decimal, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteralDecimal();
		}

        public static BnfiTermValue<Decimal> ParseDataLiteralDecimalFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Decimal, length: length).ParseDataLiteralDecimal();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Decimal> CreateNumberLiteralDecimal(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).ParseNumberLiteralDecimal();
        }

		public static BnfiTermValue<Decimal> CreateNumberLiteralDecimal(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).ParseNumberLiteralDecimal();
        }

		public static BnfiTermValue<Decimal> CreateNumberLiteralDecimal(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberLiteralDecimal();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse DateTime (Data)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<DateTime> ParseDataLiteralDateTime(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.DateTime)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a DateTime", dataLiteral.Name);

            return Parse<DateTime>(dataLiteral, (context, parseNode) => { return (DateTime)parseNode.FindToken().Value; }, IdentityFunctionForceCast<DateTime, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<DateTime> ParseDataLiteralDateTime(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteralDateTime(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<DateTime> ParseDataLiteralDateTime(string name = "dataliteralDateTime")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.DateTime).ParseDataLiteralDateTime();
		}

        public static BnfiTermValue<DateTime> ParseDataLiteralDateTimeDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.DateTime).ParseDataLiteralDateTime();
		}

        public static BnfiTermValue<DateTime> ParseDataLiteralDateTimeDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.DateTime, terminator: terminator).ParseDataLiteralDateTime();
		}

        public static BnfiTermValue<DateTime> ParseDataLiteralDateTimeQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.DateTime, startEndSymbol: startEndSymbol).ParseDataLiteralDateTime();
		}

        public static BnfiTermValue<DateTime> ParseDataLiteralDateTimeQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.DateTime, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteralDateTime();
		}

        public static BnfiTermValue<DateTime> ParseDataLiteralDateTimeFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.DateTime, length: length).ParseDataLiteralDateTime();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Parse String (Data)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<String> ParseDataLiteralString(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.String)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a String", dataLiteral.Name);

            return Parse<String>(dataLiteral, (context, parseNode) => { return (String)parseNode.FindToken().Value; }, IdentityFunctionForceCast<String, object>, astForChild: false).MakeUncontractible();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<String> ParseDataLiteralString(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataLiteralString(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<String> ParseDataLiteralString(string name = "dataliteralString")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.String).ParseDataLiteralString();
		}

        public static BnfiTermValue<String> ParseDataLiteralStringDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.String).ParseDataLiteralString();
		}

        public static BnfiTermValue<String> ParseDataLiteralStringDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.String, terminator: terminator).ParseDataLiteralString();
		}

        public static BnfiTermValue<String> ParseDataLiteralStringQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.String, startEndSymbol: startEndSymbol).ParseDataLiteralString();
		}

        public static BnfiTermValue<String> ParseDataLiteralStringQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.String, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataLiteralString();
		}

        public static BnfiTermValue<String> ParseDataLiteralStringFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.String, length: length).ParseDataLiteralString();
		}

		#endregion
	}

	#endregion

	#endregion

}

