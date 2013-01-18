 
// GENERATED FILE

using System;
using System.Collections.Generic;
using System.Linq;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Sarcasm.Ast
{
	#region Parse Object (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValueTL ParseData(DataLiteralBase dataLiteral)
        {
            return Parse(dataLiteral, (context, parseNode) => { return (Object)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Object, object>, astForChild: false);
        }

		public static BnfiTermValueTL ParseNumber(NumberLiteral numberLiteral)
        {
            return Parse(numberLiteral, (context, parseNode) => { return (Object)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Object, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValueTL ParseData(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseData(dataLiteral);
        }

		public static BnfiTermValueTL ParseNumber(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumber(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValueTL ParseData(string name = "data")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Object).ParseData();
		}

        public static BnfiTermValueTL ParseDataDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Object).ParseData();
		}

        public static BnfiTermValueTL ParseDataDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Object, terminator: terminator).ParseData();
		}

        public static BnfiTermValueTL ParseDataQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Object, startEndSymbol: startEndSymbol).ParseData();
		}

        public static BnfiTermValueTL ParseDataQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Object, startSymbol: startSymbol, endSymbol: endSymbol).ParseData();
		}

        public static BnfiTermValueTL ParseDataFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Object, length: length).ParseData();
		}

		#endregion

		#region Number

		public static BnfiTermValueTL ParseNumber(string name = "number")
        {
			return new NumberLiteral(name: name).ParseNumber();
        }

		public static BnfiTermValueTL ParseNumber(NumberOptions options)
        {
			return new NumberLiteral(name: "number", options: options).ParseNumber();
        }

		public static BnfiTermValueTL ParseNumber(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumber();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse DBNull (Data)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<DBNull> ParseDataDBNull(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.DBNull)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a DBNull", dataLiteral.Name);

            return Parse<DBNull>(dataLiteral, (context, parseNode) => { return (DBNull)parseNode.FindToken().Value; }, IdentityFunctionForceCast<DBNull, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<DBNull> ParseDataDBNull(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataDBNull(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<DBNull> ParseDataDBNull(string name = "dataDBNull")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.DBNull).ParseDataDBNull();
		}

        public static BnfiTermValue<DBNull> ParseDataDBNullDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.DBNull).ParseDataDBNull();
		}

        public static BnfiTermValue<DBNull> ParseDataDBNullDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.DBNull, terminator: terminator).ParseDataDBNull();
		}

        public static BnfiTermValue<DBNull> ParseDataDBNullQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.DBNull, startEndSymbol: startEndSymbol).ParseDataDBNull();
		}

        public static BnfiTermValue<DBNull> ParseDataDBNullQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.DBNull, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataDBNull();
		}

        public static BnfiTermValue<DBNull> ParseDataDBNullFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.DBNull, length: length).ParseDataDBNull();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Parse Boolean (Data)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Boolean> ParseDataBoolean(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Boolean)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Boolean", dataLiteral.Name);

            return Parse<Boolean>(dataLiteral, (context, parseNode) => { return (Boolean)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Boolean, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Boolean> ParseDataBoolean(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataBoolean(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Boolean> ParseDataBoolean(string name = "dataBoolean")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Boolean).ParseDataBoolean();
		}

        public static BnfiTermValue<Boolean> ParseDataBooleanDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Boolean).ParseDataBoolean();
		}

        public static BnfiTermValue<Boolean> ParseDataBooleanDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Boolean, terminator: terminator).ParseDataBoolean();
		}

        public static BnfiTermValue<Boolean> ParseDataBooleanQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Boolean, startEndSymbol: startEndSymbol).ParseDataBoolean();
		}

        public static BnfiTermValue<Boolean> ParseDataBooleanQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Boolean, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataBoolean();
		}

        public static BnfiTermValue<Boolean> ParseDataBooleanFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Boolean, length: length).ParseDataBoolean();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Parse Char (Data)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Char> ParseDataChar(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Char)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Char", dataLiteral.Name);

            return Parse<Char>(dataLiteral, (context, parseNode) => { return (Char)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Char, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Char> ParseDataChar(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataChar(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Char> ParseDataChar(string name = "dataChar")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Char).ParseDataChar();
		}

        public static BnfiTermValue<Char> ParseDataCharDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Char).ParseDataChar();
		}

        public static BnfiTermValue<Char> ParseDataCharDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Char, terminator: terminator).ParseDataChar();
		}

        public static BnfiTermValue<Char> ParseDataCharQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Char, startEndSymbol: startEndSymbol).ParseDataChar();
		}

        public static BnfiTermValue<Char> ParseDataCharQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Char, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataChar();
		}

        public static BnfiTermValue<Char> ParseDataCharFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Char, length: length).ParseDataChar();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Parse SByte (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<SByte> ParseDataSByte(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.SByte)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a SByte", dataLiteral.Name);

            return Parse<SByte>(dataLiteral, (context, parseNode) => { return (SByte)parseNode.FindToken().Value; }, IdentityFunctionForceCast<SByte, object>, astForChild: false);
        }

		public static BnfiTermValue<SByte> ParseNumberSByte(NumberLiteral numberLiteral)
        {
            return Parse<SByte>(numberLiteral, (context, parseNode) => { return (SByte)parseNode.FindToken().Value; }, IdentityFunctionForceCast<SByte, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<SByte> ParseDataSByte(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataSByte(dataLiteral);
        }

		public static BnfiTermValue<SByte> ParseNumberSByte(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberSByte(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<SByte> ParseDataSByte(string name = "dataSByte")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.SByte).ParseDataSByte();
		}

        public static BnfiTermValue<SByte> ParseDataSByteDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.SByte).ParseDataSByte();
		}

        public static BnfiTermValue<SByte> ParseDataSByteDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.SByte, terminator: terminator).ParseDataSByte();
		}

        public static BnfiTermValue<SByte> ParseDataSByteQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.SByte, startEndSymbol: startEndSymbol).ParseDataSByte();
		}

        public static BnfiTermValue<SByte> ParseDataSByteQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.SByte, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataSByte();
		}

        public static BnfiTermValue<SByte> ParseDataSByteFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.SByte, length: length).ParseDataSByte();
		}

		#endregion

		#region Number

		public static BnfiTermValue<SByte> ParseNumberSByte(string name = "number")
        {
			return new NumberLiteral(name: name).ParseNumberSByte();
        }

		public static BnfiTermValue<SByte> ParseNumberSByte(NumberOptions options)
        {
			return new NumberLiteral(name: "number", options: options).ParseNumberSByte();
        }

		public static BnfiTermValue<SByte> ParseNumberSByte(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberSByte();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse Byte (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Byte> ParseDataByte(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Byte)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Byte", dataLiteral.Name);

            return Parse<Byte>(dataLiteral, (context, parseNode) => { return (Byte)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Byte, object>, astForChild: false);
        }

		public static BnfiTermValue<Byte> ParseNumberByte(NumberLiteral numberLiteral)
        {
            return Parse<Byte>(numberLiteral, (context, parseNode) => { return (Byte)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Byte, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Byte> ParseDataByte(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataByte(dataLiteral);
        }

		public static BnfiTermValue<Byte> ParseNumberByte(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberByte(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Byte> ParseDataByte(string name = "dataByte")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Byte).ParseDataByte();
		}

        public static BnfiTermValue<Byte> ParseDataByteDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Byte).ParseDataByte();
		}

        public static BnfiTermValue<Byte> ParseDataByteDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Byte, terminator: terminator).ParseDataByte();
		}

        public static BnfiTermValue<Byte> ParseDataByteQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Byte, startEndSymbol: startEndSymbol).ParseDataByte();
		}

        public static BnfiTermValue<Byte> ParseDataByteQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Byte, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataByte();
		}

        public static BnfiTermValue<Byte> ParseDataByteFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Byte, length: length).ParseDataByte();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Byte> ParseNumberByte(string name = "number")
        {
			return new NumberLiteral(name: name).ParseNumberByte();
        }

		public static BnfiTermValue<Byte> ParseNumberByte(NumberOptions options)
        {
			return new NumberLiteral(name: "number", options: options).ParseNumberByte();
        }

		public static BnfiTermValue<Byte> ParseNumberByte(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberByte();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse Int16 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Int16> ParseDataInt16(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Int16)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Int16", dataLiteral.Name);

            return Parse<Int16>(dataLiteral, (context, parseNode) => { return (Int16)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int16, object>, astForChild: false);
        }

		public static BnfiTermValue<Int16> ParseNumberInt16(NumberLiteral numberLiteral)
        {
            return Parse<Int16>(numberLiteral, (context, parseNode) => { return (Int16)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int16, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Int16> ParseDataInt16(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataInt16(dataLiteral);
        }

		public static BnfiTermValue<Int16> ParseNumberInt16(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberInt16(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Int16> ParseDataInt16(string name = "dataInt16")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Int16).ParseDataInt16();
		}

        public static BnfiTermValue<Int16> ParseDataInt16Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int16).ParseDataInt16();
		}

        public static BnfiTermValue<Int16> ParseDataInt16Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int16, terminator: terminator).ParseDataInt16();
		}

        public static BnfiTermValue<Int16> ParseDataInt16Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int16, startEndSymbol: startEndSymbol).ParseDataInt16();
		}

        public static BnfiTermValue<Int16> ParseDataInt16Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int16, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataInt16();
		}

        public static BnfiTermValue<Int16> ParseDataInt16FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Int16, length: length).ParseDataInt16();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Int16> ParseNumberInt16(string name = "number")
        {
			return new NumberLiteral(name: name).ParseNumberInt16();
        }

		public static BnfiTermValue<Int16> ParseNumberInt16(NumberOptions options)
        {
			return new NumberLiteral(name: "number", options: options).ParseNumberInt16();
        }

		public static BnfiTermValue<Int16> ParseNumberInt16(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberInt16();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse UInt16 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<UInt16> ParseDataUInt16(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.UInt16)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a UInt16", dataLiteral.Name);

            return Parse<UInt16>(dataLiteral, (context, parseNode) => { return (UInt16)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt16, object>, astForChild: false);
        }

		public static BnfiTermValue<UInt16> ParseNumberUInt16(NumberLiteral numberLiteral)
        {
            return Parse<UInt16>(numberLiteral, (context, parseNode) => { return (UInt16)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt16, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<UInt16> ParseDataUInt16(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataUInt16(dataLiteral);
        }

		public static BnfiTermValue<UInt16> ParseNumberUInt16(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberUInt16(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<UInt16> ParseDataUInt16(string name = "dataUInt16")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.UInt16).ParseDataUInt16();
		}

        public static BnfiTermValue<UInt16> ParseDataUInt16Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt16).ParseDataUInt16();
		}

        public static BnfiTermValue<UInt16> ParseDataUInt16Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt16, terminator: terminator).ParseDataUInt16();
		}

        public static BnfiTermValue<UInt16> ParseDataUInt16Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt16, startEndSymbol: startEndSymbol).ParseDataUInt16();
		}

        public static BnfiTermValue<UInt16> ParseDataUInt16Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt16, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataUInt16();
		}

        public static BnfiTermValue<UInt16> ParseDataUInt16FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.UInt16, length: length).ParseDataUInt16();
		}

		#endregion

		#region Number

		public static BnfiTermValue<UInt16> ParseNumberUInt16(string name = "number")
        {
			return new NumberLiteral(name: name).ParseNumberUInt16();
        }

		public static BnfiTermValue<UInt16> ParseNumberUInt16(NumberOptions options)
        {
			return new NumberLiteral(name: "number", options: options).ParseNumberUInt16();
        }

		public static BnfiTermValue<UInt16> ParseNumberUInt16(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberUInt16();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse Int32 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Int32> ParseDataInt32(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Int32)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Int32", dataLiteral.Name);

            return Parse<Int32>(dataLiteral, (context, parseNode) => { return (Int32)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int32, object>, astForChild: false);
        }

		public static BnfiTermValue<Int32> ParseNumberInt32(NumberLiteral numberLiteral)
        {
            return Parse<Int32>(numberLiteral, (context, parseNode) => { return (Int32)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int32, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Int32> ParseDataInt32(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataInt32(dataLiteral);
        }

		public static BnfiTermValue<Int32> ParseNumberInt32(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberInt32(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Int32> ParseDataInt32(string name = "dataInt32")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Int32).ParseDataInt32();
		}

        public static BnfiTermValue<Int32> ParseDataInt32Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int32).ParseDataInt32();
		}

        public static BnfiTermValue<Int32> ParseDataInt32Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int32, terminator: terminator).ParseDataInt32();
		}

        public static BnfiTermValue<Int32> ParseDataInt32Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int32, startEndSymbol: startEndSymbol).ParseDataInt32();
		}

        public static BnfiTermValue<Int32> ParseDataInt32Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int32, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataInt32();
		}

        public static BnfiTermValue<Int32> ParseDataInt32FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Int32, length: length).ParseDataInt32();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Int32> ParseNumberInt32(string name = "number")
        {
			return new NumberLiteral(name: name).ParseNumberInt32();
        }

		public static BnfiTermValue<Int32> ParseNumberInt32(NumberOptions options)
        {
			return new NumberLiteral(name: "number", options: options).ParseNumberInt32();
        }

		public static BnfiTermValue<Int32> ParseNumberInt32(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberInt32();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse UInt32 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<UInt32> ParseDataUInt32(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.UInt32)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a UInt32", dataLiteral.Name);

            return Parse<UInt32>(dataLiteral, (context, parseNode) => { return (UInt32)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt32, object>, astForChild: false);
        }

		public static BnfiTermValue<UInt32> ParseNumberUInt32(NumberLiteral numberLiteral)
        {
            return Parse<UInt32>(numberLiteral, (context, parseNode) => { return (UInt32)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt32, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<UInt32> ParseDataUInt32(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataUInt32(dataLiteral);
        }

		public static BnfiTermValue<UInt32> ParseNumberUInt32(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberUInt32(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<UInt32> ParseDataUInt32(string name = "dataUInt32")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.UInt32).ParseDataUInt32();
		}

        public static BnfiTermValue<UInt32> ParseDataUInt32Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt32).ParseDataUInt32();
		}

        public static BnfiTermValue<UInt32> ParseDataUInt32Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt32, terminator: terminator).ParseDataUInt32();
		}

        public static BnfiTermValue<UInt32> ParseDataUInt32Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt32, startEndSymbol: startEndSymbol).ParseDataUInt32();
		}

        public static BnfiTermValue<UInt32> ParseDataUInt32Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt32, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataUInt32();
		}

        public static BnfiTermValue<UInt32> ParseDataUInt32FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.UInt32, length: length).ParseDataUInt32();
		}

		#endregion

		#region Number

		public static BnfiTermValue<UInt32> ParseNumberUInt32(string name = "number")
        {
			return new NumberLiteral(name: name).ParseNumberUInt32();
        }

		public static BnfiTermValue<UInt32> ParseNumberUInt32(NumberOptions options)
        {
			return new NumberLiteral(name: "number", options: options).ParseNumberUInt32();
        }

		public static BnfiTermValue<UInt32> ParseNumberUInt32(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberUInt32();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse Int64 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Int64> ParseDataInt64(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Int64)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Int64", dataLiteral.Name);

            return Parse<Int64>(dataLiteral, (context, parseNode) => { return (Int64)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int64, object>, astForChild: false);
        }

		public static BnfiTermValue<Int64> ParseNumberInt64(NumberLiteral numberLiteral)
        {
            return Parse<Int64>(numberLiteral, (context, parseNode) => { return (Int64)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int64, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Int64> ParseDataInt64(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataInt64(dataLiteral);
        }

		public static BnfiTermValue<Int64> ParseNumberInt64(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberInt64(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Int64> ParseDataInt64(string name = "dataInt64")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Int64).ParseDataInt64();
		}

        public static BnfiTermValue<Int64> ParseDataInt64Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int64).ParseDataInt64();
		}

        public static BnfiTermValue<Int64> ParseDataInt64Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int64, terminator: terminator).ParseDataInt64();
		}

        public static BnfiTermValue<Int64> ParseDataInt64Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int64, startEndSymbol: startEndSymbol).ParseDataInt64();
		}

        public static BnfiTermValue<Int64> ParseDataInt64Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int64, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataInt64();
		}

        public static BnfiTermValue<Int64> ParseDataInt64FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Int64, length: length).ParseDataInt64();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Int64> ParseNumberInt64(string name = "number")
        {
			return new NumberLiteral(name: name).ParseNumberInt64();
        }

		public static BnfiTermValue<Int64> ParseNumberInt64(NumberOptions options)
        {
			return new NumberLiteral(name: "number", options: options).ParseNumberInt64();
        }

		public static BnfiTermValue<Int64> ParseNumberInt64(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberInt64();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse UInt64 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<UInt64> ParseDataUInt64(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.UInt64)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a UInt64", dataLiteral.Name);

            return Parse<UInt64>(dataLiteral, (context, parseNode) => { return (UInt64)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt64, object>, astForChild: false);
        }

		public static BnfiTermValue<UInt64> ParseNumberUInt64(NumberLiteral numberLiteral)
        {
            return Parse<UInt64>(numberLiteral, (context, parseNode) => { return (UInt64)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt64, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<UInt64> ParseDataUInt64(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataUInt64(dataLiteral);
        }

		public static BnfiTermValue<UInt64> ParseNumberUInt64(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberUInt64(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<UInt64> ParseDataUInt64(string name = "dataUInt64")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.UInt64).ParseDataUInt64();
		}

        public static BnfiTermValue<UInt64> ParseDataUInt64Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt64).ParseDataUInt64();
		}

        public static BnfiTermValue<UInt64> ParseDataUInt64Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt64, terminator: terminator).ParseDataUInt64();
		}

        public static BnfiTermValue<UInt64> ParseDataUInt64Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt64, startEndSymbol: startEndSymbol).ParseDataUInt64();
		}

        public static BnfiTermValue<UInt64> ParseDataUInt64Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt64, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataUInt64();
		}

        public static BnfiTermValue<UInt64> ParseDataUInt64FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.UInt64, length: length).ParseDataUInt64();
		}

		#endregion

		#region Number

		public static BnfiTermValue<UInt64> ParseNumberUInt64(string name = "number")
        {
			return new NumberLiteral(name: name).ParseNumberUInt64();
        }

		public static BnfiTermValue<UInt64> ParseNumberUInt64(NumberOptions options)
        {
			return new NumberLiteral(name: "number", options: options).ParseNumberUInt64();
        }

		public static BnfiTermValue<UInt64> ParseNumberUInt64(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberUInt64();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse Single (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Single> ParseDataSingle(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Single)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Single", dataLiteral.Name);

            return Parse<Single>(dataLiteral, (context, parseNode) => { return (Single)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Single, object>, astForChild: false);
        }

		public static BnfiTermValue<Single> ParseNumberSingle(NumberLiteral numberLiteral)
        {
            return Parse<Single>(numberLiteral, (context, parseNode) => { return (Single)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Single, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Single> ParseDataSingle(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataSingle(dataLiteral);
        }

		public static BnfiTermValue<Single> ParseNumberSingle(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberSingle(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Single> ParseDataSingle(string name = "dataSingle")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Single).ParseDataSingle();
		}

        public static BnfiTermValue<Single> ParseDataSingleDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Single).ParseDataSingle();
		}

        public static BnfiTermValue<Single> ParseDataSingleDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Single, terminator: terminator).ParseDataSingle();
		}

        public static BnfiTermValue<Single> ParseDataSingleQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Single, startEndSymbol: startEndSymbol).ParseDataSingle();
		}

        public static BnfiTermValue<Single> ParseDataSingleQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Single, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataSingle();
		}

        public static BnfiTermValue<Single> ParseDataSingleFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Single, length: length).ParseDataSingle();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Single> ParseNumberSingle(string name = "number")
        {
			return new NumberLiteral(name: name).ParseNumberSingle();
        }

		public static BnfiTermValue<Single> ParseNumberSingle(NumberOptions options)
        {
			return new NumberLiteral(name: "number", options: options).ParseNumberSingle();
        }

		public static BnfiTermValue<Single> ParseNumberSingle(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberSingle();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse Double (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Double> ParseDataDouble(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Double)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Double", dataLiteral.Name);

            return Parse<Double>(dataLiteral, (context, parseNode) => { return (Double)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Double, object>, astForChild: false);
        }

		public static BnfiTermValue<Double> ParseNumberDouble(NumberLiteral numberLiteral)
        {
            return Parse<Double>(numberLiteral, (context, parseNode) => { return (Double)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Double, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Double> ParseDataDouble(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataDouble(dataLiteral);
        }

		public static BnfiTermValue<Double> ParseNumberDouble(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberDouble(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Double> ParseDataDouble(string name = "dataDouble")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Double).ParseDataDouble();
		}

        public static BnfiTermValue<Double> ParseDataDoubleDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Double).ParseDataDouble();
		}

        public static BnfiTermValue<Double> ParseDataDoubleDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Double, terminator: terminator).ParseDataDouble();
		}

        public static BnfiTermValue<Double> ParseDataDoubleQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Double, startEndSymbol: startEndSymbol).ParseDataDouble();
		}

        public static BnfiTermValue<Double> ParseDataDoubleQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Double, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataDouble();
		}

        public static BnfiTermValue<Double> ParseDataDoubleFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Double, length: length).ParseDataDouble();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Double> ParseNumberDouble(string name = "number")
        {
			return new NumberLiteral(name: name).ParseNumberDouble();
        }

		public static BnfiTermValue<Double> ParseNumberDouble(NumberOptions options)
        {
			return new NumberLiteral(name: "number", options: options).ParseNumberDouble();
        }

		public static BnfiTermValue<Double> ParseNumberDouble(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberDouble();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse Decimal (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Decimal> ParseDataDecimal(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Decimal)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Decimal", dataLiteral.Name);

            return Parse<Decimal>(dataLiteral, (context, parseNode) => { return (Decimal)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Decimal, object>, astForChild: false);
        }

		public static BnfiTermValue<Decimal> ParseNumberDecimal(NumberLiteral numberLiteral)
        {
            return Parse<Decimal>(numberLiteral, (context, parseNode) => { return (Decimal)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Decimal, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Decimal> ParseDataDecimal(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataDecimal(dataLiteral);
        }

		public static BnfiTermValue<Decimal> ParseNumberDecimal(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.ParseNumberDecimal(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Decimal> ParseDataDecimal(string name = "dataDecimal")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Decimal).ParseDataDecimal();
		}

        public static BnfiTermValue<Decimal> ParseDataDecimalDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Decimal).ParseDataDecimal();
		}

        public static BnfiTermValue<Decimal> ParseDataDecimalDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Decimal, terminator: terminator).ParseDataDecimal();
		}

        public static BnfiTermValue<Decimal> ParseDataDecimalQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Decimal, startEndSymbol: startEndSymbol).ParseDataDecimal();
		}

        public static BnfiTermValue<Decimal> ParseDataDecimalQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Decimal, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataDecimal();
		}

        public static BnfiTermValue<Decimal> ParseDataDecimalFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Decimal, length: length).ParseDataDecimal();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Decimal> ParseNumberDecimal(string name = "number")
        {
			return new NumberLiteral(name: name).ParseNumberDecimal();
        }

		public static BnfiTermValue<Decimal> ParseNumberDecimal(NumberOptions options)
        {
			return new NumberLiteral(name: "number", options: options).ParseNumberDecimal();
        }

		public static BnfiTermValue<Decimal> ParseNumberDecimal(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).ParseNumberDecimal();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Parse DateTime (Data)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<DateTime> ParseDataDateTime(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.DateTime)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a DateTime", dataLiteral.Name);

            return Parse<DateTime>(dataLiteral, (context, parseNode) => { return (DateTime)parseNode.FindToken().Value; }, IdentityFunctionForceCast<DateTime, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<DateTime> ParseDataDateTime(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataDateTime(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<DateTime> ParseDataDateTime(string name = "dataDateTime")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.DateTime).ParseDataDateTime();
		}

        public static BnfiTermValue<DateTime> ParseDataDateTimeDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.DateTime).ParseDataDateTime();
		}

        public static BnfiTermValue<DateTime> ParseDataDateTimeDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.DateTime, terminator: terminator).ParseDataDateTime();
		}

        public static BnfiTermValue<DateTime> ParseDataDateTimeQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.DateTime, startEndSymbol: startEndSymbol).ParseDataDateTime();
		}

        public static BnfiTermValue<DateTime> ParseDataDateTimeQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.DateTime, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataDateTime();
		}

        public static BnfiTermValue<DateTime> ParseDataDateTimeFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.DateTime, length: length).ParseDataDateTime();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Parse String (Data)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<String> ParseDataString(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.String)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a String", dataLiteral.Name);

            return Parse<String>(dataLiteral, (context, parseNode) => { return (String)parseNode.FindToken().Value; }, IdentityFunctionForceCast<String, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<String> ParseDataString(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.ParseDataString(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<String> ParseDataString(string name = "dataString")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.String).ParseDataString();
		}

        public static BnfiTermValue<String> ParseDataStringDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.String).ParseDataString();
		}

        public static BnfiTermValue<String> ParseDataStringDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.String, terminator: terminator).ParseDataString();
		}

        public static BnfiTermValue<String> ParseDataStringQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.String, startEndSymbol: startEndSymbol).ParseDataString();
		}

        public static BnfiTermValue<String> ParseDataStringQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.String, startSymbol: startSymbol, endSymbol: endSymbol).ParseDataString();
		}

        public static BnfiTermValue<String> ParseDataStringFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.String, length: length).ParseDataString();
		}

		#endregion
	}

	#endregion

	#endregion

}

