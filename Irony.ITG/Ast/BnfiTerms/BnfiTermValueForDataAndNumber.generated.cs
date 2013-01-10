 
// GENERATED FILE

using System;
using System.Collections.Generic;
using System.Linq;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.ITG.Ast
{
	#region Create Object (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Object> CreateDataObject(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Object)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Object", dataLiteral.Name);

            BnfiTermValue<Object> bnfiTermValue = Create<Object>(dataLiteral, (context, parseNode) => { return (Object)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Object, object>;
            return bnfiTermValue;
        }

		public static BnfiTermValue<Object> CreateNumberObject(NumberLiteral numberLiteral)
        {
            BnfiTermValue<Object> bnfiTermValue = Create<Object>(numberLiteral, (context, parseNode) => { return (Object)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Object, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Object> CreateDataObject(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataObject(dataLiteral);
        }

		public static BnfiTermValue<Object> CreateNumberObject(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.CreateNumberObject(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Object> CreateDataObject()
		{
			return new DataLiteralBase(name: "Object", dataType: TypeCode.Object).CreateDataObject();
		}

        public static BnfiTermValue<Object> CreateDataObjectDsv()
		{
			return new DsvLiteral(name: "ObjectDsv", dataType: TypeCode.Object).CreateDataObject();
		}

        public static BnfiTermValue<Object> CreateDataObjectDsv(string terminator)
		{
			return new DsvLiteral(name: "ObjectDsv", dataType: TypeCode.Object, terminator: terminator).CreateDataObject();
		}

        public static BnfiTermValue<Object> CreateDataObjectQuoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "ObjectQuoted", dataType: TypeCode.Object, startEndSymbol: startEndSymbol).CreateDataObject();
		}

        public static BnfiTermValue<Object> CreateDataObjectQuoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "ObjectQuoted", dataType: TypeCode.Object, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataObject();
		}

        public static BnfiTermValue<Object> CreateDataObjectFixedLength(int length)
		{
			return new FixedLengthLiteral(name: "ObjectFixedLength", dataType: TypeCode.Object, length: length).CreateDataObject();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Object> CreateNumberObject()
        {
			return new NumberLiteral(name: null).CreateNumberObject();
        }

		public static BnfiTermValue<Object> CreateNumberObject(NumberOptions options)
        {
			return new NumberLiteral(name: null, options: options).CreateNumberObject();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Create DBNull (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<DBNull> CreateDataDBNull(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.DBNull)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a DBNull", dataLiteral.Name);

            BnfiTermValue<DBNull> bnfiTermValue = Create<DBNull>(dataLiteral, (context, parseNode) => { return (DBNull)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<DBNull, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<DBNull> CreateDataDBNull(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataDBNull(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<DBNull> CreateDataDBNull()
		{
			return new DataLiteralBase(name: "DBNull", dataType: TypeCode.DBNull).CreateDataDBNull();
		}

        public static BnfiTermValue<DBNull> CreateDataDBNullDsv()
		{
			return new DsvLiteral(name: "DBNullDsv", dataType: TypeCode.DBNull).CreateDataDBNull();
		}

        public static BnfiTermValue<DBNull> CreateDataDBNullDsv(string terminator)
		{
			return new DsvLiteral(name: "DBNullDsv", dataType: TypeCode.DBNull, terminator: terminator).CreateDataDBNull();
		}

        public static BnfiTermValue<DBNull> CreateDataDBNullQuoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "DBNullQuoted", dataType: TypeCode.DBNull, startEndSymbol: startEndSymbol).CreateDataDBNull();
		}

        public static BnfiTermValue<DBNull> CreateDataDBNullQuoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "DBNullQuoted", dataType: TypeCode.DBNull, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataDBNull();
		}

        public static BnfiTermValue<DBNull> CreateDataDBNullFixedLength(int length)
		{
			return new FixedLengthLiteral(name: "DBNullFixedLength", dataType: TypeCode.DBNull, length: length).CreateDataDBNull();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Create Boolean (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Boolean> CreateDataBoolean(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Boolean)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Boolean", dataLiteral.Name);

            BnfiTermValue<Boolean> bnfiTermValue = Create<Boolean>(dataLiteral, (context, parseNode) => { return (Boolean)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Boolean, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Boolean> CreateDataBoolean(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataBoolean(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Boolean> CreateDataBoolean()
		{
			return new DataLiteralBase(name: "Boolean", dataType: TypeCode.Boolean).CreateDataBoolean();
		}

        public static BnfiTermValue<Boolean> CreateDataBooleanDsv()
		{
			return new DsvLiteral(name: "BooleanDsv", dataType: TypeCode.Boolean).CreateDataBoolean();
		}

        public static BnfiTermValue<Boolean> CreateDataBooleanDsv(string terminator)
		{
			return new DsvLiteral(name: "BooleanDsv", dataType: TypeCode.Boolean, terminator: terminator).CreateDataBoolean();
		}

        public static BnfiTermValue<Boolean> CreateDataBooleanQuoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "BooleanQuoted", dataType: TypeCode.Boolean, startEndSymbol: startEndSymbol).CreateDataBoolean();
		}

        public static BnfiTermValue<Boolean> CreateDataBooleanQuoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "BooleanQuoted", dataType: TypeCode.Boolean, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataBoolean();
		}

        public static BnfiTermValue<Boolean> CreateDataBooleanFixedLength(int length)
		{
			return new FixedLengthLiteral(name: "BooleanFixedLength", dataType: TypeCode.Boolean, length: length).CreateDataBoolean();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Create Char (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Char> CreateDataChar(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Char)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Char", dataLiteral.Name);

            BnfiTermValue<Char> bnfiTermValue = Create<Char>(dataLiteral, (context, parseNode) => { return (Char)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Char, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Char> CreateDataChar(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataChar(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Char> CreateDataChar()
		{
			return new DataLiteralBase(name: "Char", dataType: TypeCode.Char).CreateDataChar();
		}

        public static BnfiTermValue<Char> CreateDataCharDsv()
		{
			return new DsvLiteral(name: "CharDsv", dataType: TypeCode.Char).CreateDataChar();
		}

        public static BnfiTermValue<Char> CreateDataCharDsv(string terminator)
		{
			return new DsvLiteral(name: "CharDsv", dataType: TypeCode.Char, terminator: terminator).CreateDataChar();
		}

        public static BnfiTermValue<Char> CreateDataCharQuoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "CharQuoted", dataType: TypeCode.Char, startEndSymbol: startEndSymbol).CreateDataChar();
		}

        public static BnfiTermValue<Char> CreateDataCharQuoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "CharQuoted", dataType: TypeCode.Char, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataChar();
		}

        public static BnfiTermValue<Char> CreateDataCharFixedLength(int length)
		{
			return new FixedLengthLiteral(name: "CharFixedLength", dataType: TypeCode.Char, length: length).CreateDataChar();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Create SByte (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<SByte> CreateDataSByte(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.SByte)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a SByte", dataLiteral.Name);

            BnfiTermValue<SByte> bnfiTermValue = Create<SByte>(dataLiteral, (context, parseNode) => { return (SByte)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<SByte, object>;
            return bnfiTermValue;
        }

		public static BnfiTermValue<SByte> CreateNumberSByte(NumberLiteral numberLiteral)
        {
            BnfiTermValue<SByte> bnfiTermValue = Create<SByte>(numberLiteral, (context, parseNode) => { return (SByte)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<SByte, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<SByte> CreateDataSByte(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataSByte(dataLiteral);
        }

		public static BnfiTermValue<SByte> CreateNumberSByte(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.CreateNumberSByte(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<SByte> CreateDataSByte()
		{
			return new DataLiteralBase(name: "SByte", dataType: TypeCode.SByte).CreateDataSByte();
		}

        public static BnfiTermValue<SByte> CreateDataSByteDsv()
		{
			return new DsvLiteral(name: "SByteDsv", dataType: TypeCode.SByte).CreateDataSByte();
		}

        public static BnfiTermValue<SByte> CreateDataSByteDsv(string terminator)
		{
			return new DsvLiteral(name: "SByteDsv", dataType: TypeCode.SByte, terminator: terminator).CreateDataSByte();
		}

        public static BnfiTermValue<SByte> CreateDataSByteQuoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "SByteQuoted", dataType: TypeCode.SByte, startEndSymbol: startEndSymbol).CreateDataSByte();
		}

        public static BnfiTermValue<SByte> CreateDataSByteQuoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "SByteQuoted", dataType: TypeCode.SByte, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataSByte();
		}

        public static BnfiTermValue<SByte> CreateDataSByteFixedLength(int length)
		{
			return new FixedLengthLiteral(name: "SByteFixedLength", dataType: TypeCode.SByte, length: length).CreateDataSByte();
		}

		#endregion

		#region Number

		public static BnfiTermValue<SByte> CreateNumberSByte()
        {
			return new NumberLiteral(name: null).CreateNumberSByte();
        }

		public static BnfiTermValue<SByte> CreateNumberSByte(NumberOptions options)
        {
			return new NumberLiteral(name: null, options: options).CreateNumberSByte();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Create Byte (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Byte> CreateDataByte(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Byte)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Byte", dataLiteral.Name);

            BnfiTermValue<Byte> bnfiTermValue = Create<Byte>(dataLiteral, (context, parseNode) => { return (Byte)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Byte, object>;
            return bnfiTermValue;
        }

		public static BnfiTermValue<Byte> CreateNumberByte(NumberLiteral numberLiteral)
        {
            BnfiTermValue<Byte> bnfiTermValue = Create<Byte>(numberLiteral, (context, parseNode) => { return (Byte)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Byte, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Byte> CreateDataByte(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataByte(dataLiteral);
        }

		public static BnfiTermValue<Byte> CreateNumberByte(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.CreateNumberByte(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Byte> CreateDataByte()
		{
			return new DataLiteralBase(name: "Byte", dataType: TypeCode.Byte).CreateDataByte();
		}

        public static BnfiTermValue<Byte> CreateDataByteDsv()
		{
			return new DsvLiteral(name: "ByteDsv", dataType: TypeCode.Byte).CreateDataByte();
		}

        public static BnfiTermValue<Byte> CreateDataByteDsv(string terminator)
		{
			return new DsvLiteral(name: "ByteDsv", dataType: TypeCode.Byte, terminator: terminator).CreateDataByte();
		}

        public static BnfiTermValue<Byte> CreateDataByteQuoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "ByteQuoted", dataType: TypeCode.Byte, startEndSymbol: startEndSymbol).CreateDataByte();
		}

        public static BnfiTermValue<Byte> CreateDataByteQuoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "ByteQuoted", dataType: TypeCode.Byte, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataByte();
		}

        public static BnfiTermValue<Byte> CreateDataByteFixedLength(int length)
		{
			return new FixedLengthLiteral(name: "ByteFixedLength", dataType: TypeCode.Byte, length: length).CreateDataByte();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Byte> CreateNumberByte()
        {
			return new NumberLiteral(name: null).CreateNumberByte();
        }

		public static BnfiTermValue<Byte> CreateNumberByte(NumberOptions options)
        {
			return new NumberLiteral(name: null, options: options).CreateNumberByte();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Create Int16 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Int16> CreateDataInt16(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Int16)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Int16", dataLiteral.Name);

            BnfiTermValue<Int16> bnfiTermValue = Create<Int16>(dataLiteral, (context, parseNode) => { return (Int16)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Int16, object>;
            return bnfiTermValue;
        }

		public static BnfiTermValue<Int16> CreateNumberInt16(NumberLiteral numberLiteral)
        {
            BnfiTermValue<Int16> bnfiTermValue = Create<Int16>(numberLiteral, (context, parseNode) => { return (Int16)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Int16, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Int16> CreateDataInt16(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataInt16(dataLiteral);
        }

		public static BnfiTermValue<Int16> CreateNumberInt16(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.CreateNumberInt16(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Int16> CreateDataInt16()
		{
			return new DataLiteralBase(name: "Int16", dataType: TypeCode.Int16).CreateDataInt16();
		}

        public static BnfiTermValue<Int16> CreateDataInt16Dsv()
		{
			return new DsvLiteral(name: "Int16Dsv", dataType: TypeCode.Int16).CreateDataInt16();
		}

        public static BnfiTermValue<Int16> CreateDataInt16Dsv(string terminator)
		{
			return new DsvLiteral(name: "Int16Dsv", dataType: TypeCode.Int16, terminator: terminator).CreateDataInt16();
		}

        public static BnfiTermValue<Int16> CreateDataInt16Quoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "Int16Quoted", dataType: TypeCode.Int16, startEndSymbol: startEndSymbol).CreateDataInt16();
		}

        public static BnfiTermValue<Int16> CreateDataInt16Quoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "Int16Quoted", dataType: TypeCode.Int16, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataInt16();
		}

        public static BnfiTermValue<Int16> CreateDataInt16FixedLength(int length)
		{
			return new FixedLengthLiteral(name: "Int16FixedLength", dataType: TypeCode.Int16, length: length).CreateDataInt16();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Int16> CreateNumberInt16()
        {
			return new NumberLiteral(name: null).CreateNumberInt16();
        }

		public static BnfiTermValue<Int16> CreateNumberInt16(NumberOptions options)
        {
			return new NumberLiteral(name: null, options: options).CreateNumberInt16();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Create UInt16 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<UInt16> CreateDataUInt16(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.UInt16)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a UInt16", dataLiteral.Name);

            BnfiTermValue<UInt16> bnfiTermValue = Create<UInt16>(dataLiteral, (context, parseNode) => { return (UInt16)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<UInt16, object>;
            return bnfiTermValue;
        }

		public static BnfiTermValue<UInt16> CreateNumberUInt16(NumberLiteral numberLiteral)
        {
            BnfiTermValue<UInt16> bnfiTermValue = Create<UInt16>(numberLiteral, (context, parseNode) => { return (UInt16)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<UInt16, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<UInt16> CreateDataUInt16(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataUInt16(dataLiteral);
        }

		public static BnfiTermValue<UInt16> CreateNumberUInt16(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.CreateNumberUInt16(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<UInt16> CreateDataUInt16()
		{
			return new DataLiteralBase(name: "UInt16", dataType: TypeCode.UInt16).CreateDataUInt16();
		}

        public static BnfiTermValue<UInt16> CreateDataUInt16Dsv()
		{
			return new DsvLiteral(name: "UInt16Dsv", dataType: TypeCode.UInt16).CreateDataUInt16();
		}

        public static BnfiTermValue<UInt16> CreateDataUInt16Dsv(string terminator)
		{
			return new DsvLiteral(name: "UInt16Dsv", dataType: TypeCode.UInt16, terminator: terminator).CreateDataUInt16();
		}

        public static BnfiTermValue<UInt16> CreateDataUInt16Quoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "UInt16Quoted", dataType: TypeCode.UInt16, startEndSymbol: startEndSymbol).CreateDataUInt16();
		}

        public static BnfiTermValue<UInt16> CreateDataUInt16Quoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "UInt16Quoted", dataType: TypeCode.UInt16, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataUInt16();
		}

        public static BnfiTermValue<UInt16> CreateDataUInt16FixedLength(int length)
		{
			return new FixedLengthLiteral(name: "UInt16FixedLength", dataType: TypeCode.UInt16, length: length).CreateDataUInt16();
		}

		#endregion

		#region Number

		public static BnfiTermValue<UInt16> CreateNumberUInt16()
        {
			return new NumberLiteral(name: null).CreateNumberUInt16();
        }

		public static BnfiTermValue<UInt16> CreateNumberUInt16(NumberOptions options)
        {
			return new NumberLiteral(name: null, options: options).CreateNumberUInt16();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Create Int32 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Int32> CreateDataInt32(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Int32)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Int32", dataLiteral.Name);

            BnfiTermValue<Int32> bnfiTermValue = Create<Int32>(dataLiteral, (context, parseNode) => { return (Int32)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Int32, object>;
            return bnfiTermValue;
        }

		public static BnfiTermValue<Int32> CreateNumberInt32(NumberLiteral numberLiteral)
        {
            BnfiTermValue<Int32> bnfiTermValue = Create<Int32>(numberLiteral, (context, parseNode) => { return (Int32)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Int32, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Int32> CreateDataInt32(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataInt32(dataLiteral);
        }

		public static BnfiTermValue<Int32> CreateNumberInt32(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.CreateNumberInt32(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Int32> CreateDataInt32()
		{
			return new DataLiteralBase(name: "Int32", dataType: TypeCode.Int32).CreateDataInt32();
		}

        public static BnfiTermValue<Int32> CreateDataInt32Dsv()
		{
			return new DsvLiteral(name: "Int32Dsv", dataType: TypeCode.Int32).CreateDataInt32();
		}

        public static BnfiTermValue<Int32> CreateDataInt32Dsv(string terminator)
		{
			return new DsvLiteral(name: "Int32Dsv", dataType: TypeCode.Int32, terminator: terminator).CreateDataInt32();
		}

        public static BnfiTermValue<Int32> CreateDataInt32Quoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "Int32Quoted", dataType: TypeCode.Int32, startEndSymbol: startEndSymbol).CreateDataInt32();
		}

        public static BnfiTermValue<Int32> CreateDataInt32Quoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "Int32Quoted", dataType: TypeCode.Int32, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataInt32();
		}

        public static BnfiTermValue<Int32> CreateDataInt32FixedLength(int length)
		{
			return new FixedLengthLiteral(name: "Int32FixedLength", dataType: TypeCode.Int32, length: length).CreateDataInt32();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Int32> CreateNumberInt32()
        {
			return new NumberLiteral(name: null).CreateNumberInt32();
        }

		public static BnfiTermValue<Int32> CreateNumberInt32(NumberOptions options)
        {
			return new NumberLiteral(name: null, options: options).CreateNumberInt32();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Create UInt32 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<UInt32> CreateDataUInt32(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.UInt32)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a UInt32", dataLiteral.Name);

            BnfiTermValue<UInt32> bnfiTermValue = Create<UInt32>(dataLiteral, (context, parseNode) => { return (UInt32)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<UInt32, object>;
            return bnfiTermValue;
        }

		public static BnfiTermValue<UInt32> CreateNumberUInt32(NumberLiteral numberLiteral)
        {
            BnfiTermValue<UInt32> bnfiTermValue = Create<UInt32>(numberLiteral, (context, parseNode) => { return (UInt32)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<UInt32, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<UInt32> CreateDataUInt32(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataUInt32(dataLiteral);
        }

		public static BnfiTermValue<UInt32> CreateNumberUInt32(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.CreateNumberUInt32(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<UInt32> CreateDataUInt32()
		{
			return new DataLiteralBase(name: "UInt32", dataType: TypeCode.UInt32).CreateDataUInt32();
		}

        public static BnfiTermValue<UInt32> CreateDataUInt32Dsv()
		{
			return new DsvLiteral(name: "UInt32Dsv", dataType: TypeCode.UInt32).CreateDataUInt32();
		}

        public static BnfiTermValue<UInt32> CreateDataUInt32Dsv(string terminator)
		{
			return new DsvLiteral(name: "UInt32Dsv", dataType: TypeCode.UInt32, terminator: terminator).CreateDataUInt32();
		}

        public static BnfiTermValue<UInt32> CreateDataUInt32Quoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "UInt32Quoted", dataType: TypeCode.UInt32, startEndSymbol: startEndSymbol).CreateDataUInt32();
		}

        public static BnfiTermValue<UInt32> CreateDataUInt32Quoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "UInt32Quoted", dataType: TypeCode.UInt32, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataUInt32();
		}

        public static BnfiTermValue<UInt32> CreateDataUInt32FixedLength(int length)
		{
			return new FixedLengthLiteral(name: "UInt32FixedLength", dataType: TypeCode.UInt32, length: length).CreateDataUInt32();
		}

		#endregion

		#region Number

		public static BnfiTermValue<UInt32> CreateNumberUInt32()
        {
			return new NumberLiteral(name: null).CreateNumberUInt32();
        }

		public static BnfiTermValue<UInt32> CreateNumberUInt32(NumberOptions options)
        {
			return new NumberLiteral(name: null, options: options).CreateNumberUInt32();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Create Int64 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Int64> CreateDataInt64(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Int64)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Int64", dataLiteral.Name);

            BnfiTermValue<Int64> bnfiTermValue = Create<Int64>(dataLiteral, (context, parseNode) => { return (Int64)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Int64, object>;
            return bnfiTermValue;
        }

		public static BnfiTermValue<Int64> CreateNumberInt64(NumberLiteral numberLiteral)
        {
            BnfiTermValue<Int64> bnfiTermValue = Create<Int64>(numberLiteral, (context, parseNode) => { return (Int64)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Int64, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Int64> CreateDataInt64(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataInt64(dataLiteral);
        }

		public static BnfiTermValue<Int64> CreateNumberInt64(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.CreateNumberInt64(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Int64> CreateDataInt64()
		{
			return new DataLiteralBase(name: "Int64", dataType: TypeCode.Int64).CreateDataInt64();
		}

        public static BnfiTermValue<Int64> CreateDataInt64Dsv()
		{
			return new DsvLiteral(name: "Int64Dsv", dataType: TypeCode.Int64).CreateDataInt64();
		}

        public static BnfiTermValue<Int64> CreateDataInt64Dsv(string terminator)
		{
			return new DsvLiteral(name: "Int64Dsv", dataType: TypeCode.Int64, terminator: terminator).CreateDataInt64();
		}

        public static BnfiTermValue<Int64> CreateDataInt64Quoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "Int64Quoted", dataType: TypeCode.Int64, startEndSymbol: startEndSymbol).CreateDataInt64();
		}

        public static BnfiTermValue<Int64> CreateDataInt64Quoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "Int64Quoted", dataType: TypeCode.Int64, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataInt64();
		}

        public static BnfiTermValue<Int64> CreateDataInt64FixedLength(int length)
		{
			return new FixedLengthLiteral(name: "Int64FixedLength", dataType: TypeCode.Int64, length: length).CreateDataInt64();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Int64> CreateNumberInt64()
        {
			return new NumberLiteral(name: null).CreateNumberInt64();
        }

		public static BnfiTermValue<Int64> CreateNumberInt64(NumberOptions options)
        {
			return new NumberLiteral(name: null, options: options).CreateNumberInt64();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Create UInt64 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<UInt64> CreateDataUInt64(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.UInt64)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a UInt64", dataLiteral.Name);

            BnfiTermValue<UInt64> bnfiTermValue = Create<UInt64>(dataLiteral, (context, parseNode) => { return (UInt64)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<UInt64, object>;
            return bnfiTermValue;
        }

		public static BnfiTermValue<UInt64> CreateNumberUInt64(NumberLiteral numberLiteral)
        {
            BnfiTermValue<UInt64> bnfiTermValue = Create<UInt64>(numberLiteral, (context, parseNode) => { return (UInt64)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<UInt64, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<UInt64> CreateDataUInt64(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataUInt64(dataLiteral);
        }

		public static BnfiTermValue<UInt64> CreateNumberUInt64(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.CreateNumberUInt64(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<UInt64> CreateDataUInt64()
		{
			return new DataLiteralBase(name: "UInt64", dataType: TypeCode.UInt64).CreateDataUInt64();
		}

        public static BnfiTermValue<UInt64> CreateDataUInt64Dsv()
		{
			return new DsvLiteral(name: "UInt64Dsv", dataType: TypeCode.UInt64).CreateDataUInt64();
		}

        public static BnfiTermValue<UInt64> CreateDataUInt64Dsv(string terminator)
		{
			return new DsvLiteral(name: "UInt64Dsv", dataType: TypeCode.UInt64, terminator: terminator).CreateDataUInt64();
		}

        public static BnfiTermValue<UInt64> CreateDataUInt64Quoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "UInt64Quoted", dataType: TypeCode.UInt64, startEndSymbol: startEndSymbol).CreateDataUInt64();
		}

        public static BnfiTermValue<UInt64> CreateDataUInt64Quoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "UInt64Quoted", dataType: TypeCode.UInt64, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataUInt64();
		}

        public static BnfiTermValue<UInt64> CreateDataUInt64FixedLength(int length)
		{
			return new FixedLengthLiteral(name: "UInt64FixedLength", dataType: TypeCode.UInt64, length: length).CreateDataUInt64();
		}

		#endregion

		#region Number

		public static BnfiTermValue<UInt64> CreateNumberUInt64()
        {
			return new NumberLiteral(name: null).CreateNumberUInt64();
        }

		public static BnfiTermValue<UInt64> CreateNumberUInt64(NumberOptions options)
        {
			return new NumberLiteral(name: null, options: options).CreateNumberUInt64();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Create Single (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Single> CreateDataSingle(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Single)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Single", dataLiteral.Name);

            BnfiTermValue<Single> bnfiTermValue = Create<Single>(dataLiteral, (context, parseNode) => { return (Single)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Single, object>;
            return bnfiTermValue;
        }

		public static BnfiTermValue<Single> CreateNumberSingle(NumberLiteral numberLiteral)
        {
            BnfiTermValue<Single> bnfiTermValue = Create<Single>(numberLiteral, (context, parseNode) => { return (Single)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Single, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Single> CreateDataSingle(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataSingle(dataLiteral);
        }

		public static BnfiTermValue<Single> CreateNumberSingle(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.CreateNumberSingle(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Single> CreateDataSingle()
		{
			return new DataLiteralBase(name: "Single", dataType: TypeCode.Single).CreateDataSingle();
		}

        public static BnfiTermValue<Single> CreateDataSingleDsv()
		{
			return new DsvLiteral(name: "SingleDsv", dataType: TypeCode.Single).CreateDataSingle();
		}

        public static BnfiTermValue<Single> CreateDataSingleDsv(string terminator)
		{
			return new DsvLiteral(name: "SingleDsv", dataType: TypeCode.Single, terminator: terminator).CreateDataSingle();
		}

        public static BnfiTermValue<Single> CreateDataSingleQuoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "SingleQuoted", dataType: TypeCode.Single, startEndSymbol: startEndSymbol).CreateDataSingle();
		}

        public static BnfiTermValue<Single> CreateDataSingleQuoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "SingleQuoted", dataType: TypeCode.Single, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataSingle();
		}

        public static BnfiTermValue<Single> CreateDataSingleFixedLength(int length)
		{
			return new FixedLengthLiteral(name: "SingleFixedLength", dataType: TypeCode.Single, length: length).CreateDataSingle();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Single> CreateNumberSingle()
        {
			return new NumberLiteral(name: null).CreateNumberSingle();
        }

		public static BnfiTermValue<Single> CreateNumberSingle(NumberOptions options)
        {
			return new NumberLiteral(name: null, options: options).CreateNumberSingle();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Create Double (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Double> CreateDataDouble(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Double)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Double", dataLiteral.Name);

            BnfiTermValue<Double> bnfiTermValue = Create<Double>(dataLiteral, (context, parseNode) => { return (Double)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Double, object>;
            return bnfiTermValue;
        }

		public static BnfiTermValue<Double> CreateNumberDouble(NumberLiteral numberLiteral)
        {
            BnfiTermValue<Double> bnfiTermValue = Create<Double>(numberLiteral, (context, parseNode) => { return (Double)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Double, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Double> CreateDataDouble(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataDouble(dataLiteral);
        }

		public static BnfiTermValue<Double> CreateNumberDouble(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.CreateNumberDouble(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Double> CreateDataDouble()
		{
			return new DataLiteralBase(name: "Double", dataType: TypeCode.Double).CreateDataDouble();
		}

        public static BnfiTermValue<Double> CreateDataDoubleDsv()
		{
			return new DsvLiteral(name: "DoubleDsv", dataType: TypeCode.Double).CreateDataDouble();
		}

        public static BnfiTermValue<Double> CreateDataDoubleDsv(string terminator)
		{
			return new DsvLiteral(name: "DoubleDsv", dataType: TypeCode.Double, terminator: terminator).CreateDataDouble();
		}

        public static BnfiTermValue<Double> CreateDataDoubleQuoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "DoubleQuoted", dataType: TypeCode.Double, startEndSymbol: startEndSymbol).CreateDataDouble();
		}

        public static BnfiTermValue<Double> CreateDataDoubleQuoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "DoubleQuoted", dataType: TypeCode.Double, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataDouble();
		}

        public static BnfiTermValue<Double> CreateDataDoubleFixedLength(int length)
		{
			return new FixedLengthLiteral(name: "DoubleFixedLength", dataType: TypeCode.Double, length: length).CreateDataDouble();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Double> CreateNumberDouble()
        {
			return new NumberLiteral(name: null).CreateNumberDouble();
        }

		public static BnfiTermValue<Double> CreateNumberDouble(NumberOptions options)
        {
			return new NumberLiteral(name: null, options: options).CreateNumberDouble();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Create Decimal (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Decimal> CreateDataDecimal(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Decimal)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Decimal", dataLiteral.Name);

            BnfiTermValue<Decimal> bnfiTermValue = Create<Decimal>(dataLiteral, (context, parseNode) => { return (Decimal)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Decimal, object>;
            return bnfiTermValue;
        }

		public static BnfiTermValue<Decimal> CreateNumberDecimal(NumberLiteral numberLiteral)
        {
            BnfiTermValue<Decimal> bnfiTermValue = Create<Decimal>(numberLiteral, (context, parseNode) => { return (Decimal)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<Decimal, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Decimal> CreateDataDecimal(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataDecimal(dataLiteral);
        }

		public static BnfiTermValue<Decimal> CreateNumberDecimal(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.CreateNumberDecimal(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Decimal> CreateDataDecimal()
		{
			return new DataLiteralBase(name: "Decimal", dataType: TypeCode.Decimal).CreateDataDecimal();
		}

        public static BnfiTermValue<Decimal> CreateDataDecimalDsv()
		{
			return new DsvLiteral(name: "DecimalDsv", dataType: TypeCode.Decimal).CreateDataDecimal();
		}

        public static BnfiTermValue<Decimal> CreateDataDecimalDsv(string terminator)
		{
			return new DsvLiteral(name: "DecimalDsv", dataType: TypeCode.Decimal, terminator: terminator).CreateDataDecimal();
		}

        public static BnfiTermValue<Decimal> CreateDataDecimalQuoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "DecimalQuoted", dataType: TypeCode.Decimal, startEndSymbol: startEndSymbol).CreateDataDecimal();
		}

        public static BnfiTermValue<Decimal> CreateDataDecimalQuoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "DecimalQuoted", dataType: TypeCode.Decimal, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataDecimal();
		}

        public static BnfiTermValue<Decimal> CreateDataDecimalFixedLength(int length)
		{
			return new FixedLengthLiteral(name: "DecimalFixedLength", dataType: TypeCode.Decimal, length: length).CreateDataDecimal();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Decimal> CreateNumberDecimal()
        {
			return new NumberLiteral(name: null).CreateNumberDecimal();
        }

		public static BnfiTermValue<Decimal> CreateNumberDecimal(NumberOptions options)
        {
			return new NumberLiteral(name: null, options: options).CreateNumberDecimal();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Create DateTime (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<DateTime> CreateDataDateTime(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.DateTime)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a DateTime", dataLiteral.Name);

            BnfiTermValue<DateTime> bnfiTermValue = Create<DateTime>(dataLiteral, (context, parseNode) => { return (DateTime)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<DateTime, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<DateTime> CreateDataDateTime(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataDateTime(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<DateTime> CreateDataDateTime()
		{
			return new DataLiteralBase(name: "DateTime", dataType: TypeCode.DateTime).CreateDataDateTime();
		}

        public static BnfiTermValue<DateTime> CreateDataDateTimeDsv()
		{
			return new DsvLiteral(name: "DateTimeDsv", dataType: TypeCode.DateTime).CreateDataDateTime();
		}

        public static BnfiTermValue<DateTime> CreateDataDateTimeDsv(string terminator)
		{
			return new DsvLiteral(name: "DateTimeDsv", dataType: TypeCode.DateTime, terminator: terminator).CreateDataDateTime();
		}

        public static BnfiTermValue<DateTime> CreateDataDateTimeQuoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "DateTimeQuoted", dataType: TypeCode.DateTime, startEndSymbol: startEndSymbol).CreateDataDateTime();
		}

        public static BnfiTermValue<DateTime> CreateDataDateTimeQuoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "DateTimeQuoted", dataType: TypeCode.DateTime, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataDateTime();
		}

        public static BnfiTermValue<DateTime> CreateDataDateTimeFixedLength(int length)
		{
			return new FixedLengthLiteral(name: "DateTimeFixedLength", dataType: TypeCode.DateTime, length: length).CreateDataDateTime();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Create String (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<String> CreateDataString(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.String)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a String", dataLiteral.Name);

            BnfiTermValue<String> bnfiTermValue = Create<String>(dataLiteral, (context, parseNode) => { return (String)parseNode.FindToken().Value; }, astForChild: false);
            bnfiTermValue.InverseValueConverterForUnparse = IdentityFunctionForceCast<String, object>;
            return bnfiTermValue;
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<String> CreateDataString(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.CreateDataString(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<String> CreateDataString()
		{
			return new DataLiteralBase(name: "String", dataType: TypeCode.String).CreateDataString();
		}

        public static BnfiTermValue<String> CreateDataStringDsv()
		{
			return new DsvLiteral(name: "StringDsv", dataType: TypeCode.String).CreateDataString();
		}

        public static BnfiTermValue<String> CreateDataStringDsv(string terminator)
		{
			return new DsvLiteral(name: "StringDsv", dataType: TypeCode.String, terminator: terminator).CreateDataString();
		}

        public static BnfiTermValue<String> CreateDataStringQuoted(string startEndSymbol)
		{
			return new QuotedValueLiteral(name: "StringQuoted", dataType: TypeCode.String, startEndSymbol: startEndSymbol).CreateDataString();
		}

        public static BnfiTermValue<String> CreateDataStringQuoted(string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: "StringQuoted", dataType: TypeCode.String, startSymbol: startSymbol, endSymbol: endSymbol).CreateDataString();
		}

        public static BnfiTermValue<String> CreateDataStringFixedLength(int length)
		{
			return new FixedLengthLiteral(name: "StringFixedLength", dataType: TypeCode.String, length: length).CreateDataString();
		}

		#endregion
	}

	#endregion

	#endregion

}

