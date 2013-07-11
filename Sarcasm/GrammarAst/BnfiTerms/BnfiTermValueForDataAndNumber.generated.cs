 
// GENERATED FILE

using System;
using System.Collections.Generic;
using System.Linq;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Sarcasm.GrammarAst
{
	#region Intro Object (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValueTL IntroDataLiteral(DataLiteralBase dataLiteral)
        {
            return Intro(dataLiteral, (context, parseNode) => { return (Object)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Object, object>, astForChild: false);
        }

		public static BnfiTermValueTL IntroNumberLiteral(NumberLiteral numberLiteral)
        {
            return Intro(numberLiteral, (context, parseNode) => { return (Object)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Object, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValueTL IntroDataLiteral(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteral(dataLiteral);
        }

		public static BnfiTermValueTL IntroNumberLiteral(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.IntroNumberLiteral(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValueTL CreateDataLiteral(string name = "dataliteral")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Object).IntroDataLiteral();
		}

        public static BnfiTermValueTL CreateDataLiteralDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Object).IntroDataLiteral();
		}

        public static BnfiTermValueTL CreateDataLiteralDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Object, terminator: terminator).IntroDataLiteral();
		}

        public static BnfiTermValueTL CreateDataLiteralQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Object, startEndSymbol: startEndSymbol).IntroDataLiteral();
		}

        public static BnfiTermValueTL CreateDataLiteralQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Object, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteral();
		}

        public static BnfiTermValueTL CreateDataLiteralFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Object, length: length).IntroDataLiteral();
		}

		#endregion

		#region Number

		public static BnfiTermValueTL CreateNumberLiteral(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteral();
        }

		public static BnfiTermValueTL CreateNumberLiteral(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteral();
        }

		public static BnfiTermValueTL CreateNumberLiteral(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteral();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro DBNull (Data)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<DBNull> IntroDataLiteralDBNull(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.DBNull)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a DBNull", dataLiteral.Name);

            return Intro<DBNull>(dataLiteral, (context, parseNode) => { return (DBNull)parseNode.FindToken().Value; }, IdentityFunctionForceCast<DBNull, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<DBNull> IntroDataLiteralDBNull(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteralDBNull(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<DBNull> CreateDataLiteralDBNull(string name = "dataliteralDBNull")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.DBNull).IntroDataLiteralDBNull();
		}

        public static BnfiTermValue<DBNull> CreateDataLiteralDBNullDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.DBNull).IntroDataLiteralDBNull();
		}

        public static BnfiTermValue<DBNull> CreateDataLiteralDBNullDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.DBNull, terminator: terminator).IntroDataLiteralDBNull();
		}

        public static BnfiTermValue<DBNull> CreateDataLiteralDBNullQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.DBNull, startEndSymbol: startEndSymbol).IntroDataLiteralDBNull();
		}

        public static BnfiTermValue<DBNull> CreateDataLiteralDBNullQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.DBNull, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralDBNull();
		}

        public static BnfiTermValue<DBNull> CreateDataLiteralDBNullFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.DBNull, length: length).IntroDataLiteralDBNull();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Intro Boolean (Data)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Boolean> IntroDataLiteralBoolean(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Boolean)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Boolean", dataLiteral.Name);

            return Intro<Boolean>(dataLiteral, (context, parseNode) => { return (Boolean)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Boolean, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Boolean> IntroDataLiteralBoolean(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteralBoolean(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Boolean> CreateDataLiteralBoolean(string name = "dataliteralBoolean")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Boolean).IntroDataLiteralBoolean();
		}

        public static BnfiTermValue<Boolean> CreateDataLiteralBooleanDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Boolean).IntroDataLiteralBoolean();
		}

        public static BnfiTermValue<Boolean> CreateDataLiteralBooleanDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Boolean, terminator: terminator).IntroDataLiteralBoolean();
		}

        public static BnfiTermValue<Boolean> CreateDataLiteralBooleanQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Boolean, startEndSymbol: startEndSymbol).IntroDataLiteralBoolean();
		}

        public static BnfiTermValue<Boolean> CreateDataLiteralBooleanQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Boolean, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralBoolean();
		}

        public static BnfiTermValue<Boolean> CreateDataLiteralBooleanFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Boolean, length: length).IntroDataLiteralBoolean();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Intro Char (Data)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Char> IntroDataLiteralChar(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Char)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Char", dataLiteral.Name);

            return Intro<Char>(dataLiteral, (context, parseNode) => { return (Char)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Char, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Char> IntroDataLiteralChar(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteralChar(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Char> CreateDataLiteralChar(string name = "dataliteralChar")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Char).IntroDataLiteralChar();
		}

        public static BnfiTermValue<Char> CreateDataLiteralCharDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Char).IntroDataLiteralChar();
		}

        public static BnfiTermValue<Char> CreateDataLiteralCharDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Char, terminator: terminator).IntroDataLiteralChar();
		}

        public static BnfiTermValue<Char> CreateDataLiteralCharQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Char, startEndSymbol: startEndSymbol).IntroDataLiteralChar();
		}

        public static BnfiTermValue<Char> CreateDataLiteralCharQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Char, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralChar();
		}

        public static BnfiTermValue<Char> CreateDataLiteralCharFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Char, length: length).IntroDataLiteralChar();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Intro SByte (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<SByte> IntroDataLiteralSByte(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.SByte)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a SByte", dataLiteral.Name);

            return Intro<SByte>(dataLiteral, (context, parseNode) => { return (SByte)parseNode.FindToken().Value; }, IdentityFunctionForceCast<SByte, object>, astForChild: false);
        }

		public static BnfiTermValue<SByte> IntroNumberLiteralSByte(NumberLiteral numberLiteral)
        {
            return Intro<SByte>(numberLiteral, (context, parseNode) => { return (SByte)parseNode.FindToken().Value; }, IdentityFunctionForceCast<SByte, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<SByte> IntroDataLiteralSByte(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteralSByte(dataLiteral);
        }

		public static BnfiTermValue<SByte> IntroNumberLiteralSByte(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.IntroNumberLiteralSByte(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<SByte> CreateDataLiteralSByte(string name = "dataliteralSByte")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.SByte).IntroDataLiteralSByte();
		}

        public static BnfiTermValue<SByte> CreateDataLiteralSByteDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.SByte).IntroDataLiteralSByte();
		}

        public static BnfiTermValue<SByte> CreateDataLiteralSByteDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.SByte, terminator: terminator).IntroDataLiteralSByte();
		}

        public static BnfiTermValue<SByte> CreateDataLiteralSByteQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.SByte, startEndSymbol: startEndSymbol).IntroDataLiteralSByte();
		}

        public static BnfiTermValue<SByte> CreateDataLiteralSByteQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.SByte, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralSByte();
		}

        public static BnfiTermValue<SByte> CreateDataLiteralSByteFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.SByte, length: length).IntroDataLiteralSByte();
		}

		#endregion

		#region Number

		public static BnfiTermValue<SByte> CreateNumberLiteralSByte(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralSByte();
        }

		public static BnfiTermValue<SByte> CreateNumberLiteralSByte(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralSByte();
        }

		public static BnfiTermValue<SByte> CreateNumberLiteralSByte(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralSByte();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro Byte (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Byte> IntroDataLiteralByte(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Byte)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Byte", dataLiteral.Name);

            return Intro<Byte>(dataLiteral, (context, parseNode) => { return (Byte)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Byte, object>, astForChild: false);
        }

		public static BnfiTermValue<Byte> IntroNumberLiteralByte(NumberLiteral numberLiteral)
        {
            return Intro<Byte>(numberLiteral, (context, parseNode) => { return (Byte)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Byte, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Byte> IntroDataLiteralByte(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteralByte(dataLiteral);
        }

		public static BnfiTermValue<Byte> IntroNumberLiteralByte(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.IntroNumberLiteralByte(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Byte> CreateDataLiteralByte(string name = "dataliteralByte")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Byte).IntroDataLiteralByte();
		}

        public static BnfiTermValue<Byte> CreateDataLiteralByteDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Byte).IntroDataLiteralByte();
		}

        public static BnfiTermValue<Byte> CreateDataLiteralByteDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Byte, terminator: terminator).IntroDataLiteralByte();
		}

        public static BnfiTermValue<Byte> CreateDataLiteralByteQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Byte, startEndSymbol: startEndSymbol).IntroDataLiteralByte();
		}

        public static BnfiTermValue<Byte> CreateDataLiteralByteQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Byte, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralByte();
		}

        public static BnfiTermValue<Byte> CreateDataLiteralByteFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Byte, length: length).IntroDataLiteralByte();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Byte> CreateNumberLiteralByte(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralByte();
        }

		public static BnfiTermValue<Byte> CreateNumberLiteralByte(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralByte();
        }

		public static BnfiTermValue<Byte> CreateNumberLiteralByte(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralByte();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro Int16 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Int16> IntroDataLiteralInt16(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Int16)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Int16", dataLiteral.Name);

            return Intro<Int16>(dataLiteral, (context, parseNode) => { return (Int16)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int16, object>, astForChild: false);
        }

		public static BnfiTermValue<Int16> IntroNumberLiteralInt16(NumberLiteral numberLiteral)
        {
            return Intro<Int16>(numberLiteral, (context, parseNode) => { return (Int16)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int16, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Int16> IntroDataLiteralInt16(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteralInt16(dataLiteral);
        }

		public static BnfiTermValue<Int16> IntroNumberLiteralInt16(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.IntroNumberLiteralInt16(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Int16> CreateDataLiteralInt16(string name = "dataliteralInt16")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Int16).IntroDataLiteralInt16();
		}

        public static BnfiTermValue<Int16> CreateDataLiteralInt16Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int16).IntroDataLiteralInt16();
		}

        public static BnfiTermValue<Int16> CreateDataLiteralInt16Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int16, terminator: terminator).IntroDataLiteralInt16();
		}

        public static BnfiTermValue<Int16> CreateDataLiteralInt16Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int16, startEndSymbol: startEndSymbol).IntroDataLiteralInt16();
		}

        public static BnfiTermValue<Int16> CreateDataLiteralInt16Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int16, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralInt16();
		}

        public static BnfiTermValue<Int16> CreateDataLiteralInt16FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Int16, length: length).IntroDataLiteralInt16();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Int16> CreateNumberLiteralInt16(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralInt16();
        }

		public static BnfiTermValue<Int16> CreateNumberLiteralInt16(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralInt16();
        }

		public static BnfiTermValue<Int16> CreateNumberLiteralInt16(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralInt16();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro UInt16 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<UInt16> IntroDataLiteralUInt16(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.UInt16)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a UInt16", dataLiteral.Name);

            return Intro<UInt16>(dataLiteral, (context, parseNode) => { return (UInt16)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt16, object>, astForChild: false);
        }

		public static BnfiTermValue<UInt16> IntroNumberLiteralUInt16(NumberLiteral numberLiteral)
        {
            return Intro<UInt16>(numberLiteral, (context, parseNode) => { return (UInt16)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt16, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<UInt16> IntroDataLiteralUInt16(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteralUInt16(dataLiteral);
        }

		public static BnfiTermValue<UInt16> IntroNumberLiteralUInt16(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.IntroNumberLiteralUInt16(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<UInt16> CreateDataLiteralUInt16(string name = "dataliteralUInt16")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.UInt16).IntroDataLiteralUInt16();
		}

        public static BnfiTermValue<UInt16> CreateDataLiteralUInt16Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt16).IntroDataLiteralUInt16();
		}

        public static BnfiTermValue<UInt16> CreateDataLiteralUInt16Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt16, terminator: terminator).IntroDataLiteralUInt16();
		}

        public static BnfiTermValue<UInt16> CreateDataLiteralUInt16Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt16, startEndSymbol: startEndSymbol).IntroDataLiteralUInt16();
		}

        public static BnfiTermValue<UInt16> CreateDataLiteralUInt16Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt16, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralUInt16();
		}

        public static BnfiTermValue<UInt16> CreateDataLiteralUInt16FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.UInt16, length: length).IntroDataLiteralUInt16();
		}

		#endregion

		#region Number

		public static BnfiTermValue<UInt16> CreateNumberLiteralUInt16(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralUInt16();
        }

		public static BnfiTermValue<UInt16> CreateNumberLiteralUInt16(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralUInt16();
        }

		public static BnfiTermValue<UInt16> CreateNumberLiteralUInt16(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralUInt16();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro Int32 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Int32> IntroDataLiteralInt32(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Int32)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Int32", dataLiteral.Name);

            return Intro<Int32>(dataLiteral, (context, parseNode) => { return (Int32)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int32, object>, astForChild: false);
        }

		public static BnfiTermValue<Int32> IntroNumberLiteralInt32(NumberLiteral numberLiteral)
        {
            return Intro<Int32>(numberLiteral, (context, parseNode) => { return (Int32)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int32, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Int32> IntroDataLiteralInt32(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteralInt32(dataLiteral);
        }

		public static BnfiTermValue<Int32> IntroNumberLiteralInt32(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.IntroNumberLiteralInt32(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Int32> CreateDataLiteralInt32(string name = "dataliteralInt32")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Int32).IntroDataLiteralInt32();
		}

        public static BnfiTermValue<Int32> CreateDataLiteralInt32Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int32).IntroDataLiteralInt32();
		}

        public static BnfiTermValue<Int32> CreateDataLiteralInt32Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int32, terminator: terminator).IntroDataLiteralInt32();
		}

        public static BnfiTermValue<Int32> CreateDataLiteralInt32Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int32, startEndSymbol: startEndSymbol).IntroDataLiteralInt32();
		}

        public static BnfiTermValue<Int32> CreateDataLiteralInt32Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int32, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralInt32();
		}

        public static BnfiTermValue<Int32> CreateDataLiteralInt32FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Int32, length: length).IntroDataLiteralInt32();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Int32> CreateNumberLiteralInt32(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralInt32();
        }

		public static BnfiTermValue<Int32> CreateNumberLiteralInt32(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralInt32();
        }

		public static BnfiTermValue<Int32> CreateNumberLiteralInt32(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralInt32();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro UInt32 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<UInt32> IntroDataLiteralUInt32(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.UInt32)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a UInt32", dataLiteral.Name);

            return Intro<UInt32>(dataLiteral, (context, parseNode) => { return (UInt32)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt32, object>, astForChild: false);
        }

		public static BnfiTermValue<UInt32> IntroNumberLiteralUInt32(NumberLiteral numberLiteral)
        {
            return Intro<UInt32>(numberLiteral, (context, parseNode) => { return (UInt32)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt32, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<UInt32> IntroDataLiteralUInt32(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteralUInt32(dataLiteral);
        }

		public static BnfiTermValue<UInt32> IntroNumberLiteralUInt32(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.IntroNumberLiteralUInt32(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<UInt32> CreateDataLiteralUInt32(string name = "dataliteralUInt32")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.UInt32).IntroDataLiteralUInt32();
		}

        public static BnfiTermValue<UInt32> CreateDataLiteralUInt32Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt32).IntroDataLiteralUInt32();
		}

        public static BnfiTermValue<UInt32> CreateDataLiteralUInt32Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt32, terminator: terminator).IntroDataLiteralUInt32();
		}

        public static BnfiTermValue<UInt32> CreateDataLiteralUInt32Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt32, startEndSymbol: startEndSymbol).IntroDataLiteralUInt32();
		}

        public static BnfiTermValue<UInt32> CreateDataLiteralUInt32Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt32, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralUInt32();
		}

        public static BnfiTermValue<UInt32> CreateDataLiteralUInt32FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.UInt32, length: length).IntroDataLiteralUInt32();
		}

		#endregion

		#region Number

		public static BnfiTermValue<UInt32> CreateNumberLiteralUInt32(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralUInt32();
        }

		public static BnfiTermValue<UInt32> CreateNumberLiteralUInt32(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralUInt32();
        }

		public static BnfiTermValue<UInt32> CreateNumberLiteralUInt32(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralUInt32();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro Int64 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Int64> IntroDataLiteralInt64(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Int64)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Int64", dataLiteral.Name);

            return Intro<Int64>(dataLiteral, (context, parseNode) => { return (Int64)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int64, object>, astForChild: false);
        }

		public static BnfiTermValue<Int64> IntroNumberLiteralInt64(NumberLiteral numberLiteral)
        {
            return Intro<Int64>(numberLiteral, (context, parseNode) => { return (Int64)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int64, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Int64> IntroDataLiteralInt64(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteralInt64(dataLiteral);
        }

		public static BnfiTermValue<Int64> IntroNumberLiteralInt64(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.IntroNumberLiteralInt64(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Int64> CreateDataLiteralInt64(string name = "dataliteralInt64")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Int64).IntroDataLiteralInt64();
		}

        public static BnfiTermValue<Int64> CreateDataLiteralInt64Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int64).IntroDataLiteralInt64();
		}

        public static BnfiTermValue<Int64> CreateDataLiteralInt64Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int64, terminator: terminator).IntroDataLiteralInt64();
		}

        public static BnfiTermValue<Int64> CreateDataLiteralInt64Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int64, startEndSymbol: startEndSymbol).IntroDataLiteralInt64();
		}

        public static BnfiTermValue<Int64> CreateDataLiteralInt64Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int64, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralInt64();
		}

        public static BnfiTermValue<Int64> CreateDataLiteralInt64FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Int64, length: length).IntroDataLiteralInt64();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Int64> CreateNumberLiteralInt64(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralInt64();
        }

		public static BnfiTermValue<Int64> CreateNumberLiteralInt64(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralInt64();
        }

		public static BnfiTermValue<Int64> CreateNumberLiteralInt64(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralInt64();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro UInt64 (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<UInt64> IntroDataLiteralUInt64(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.UInt64)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a UInt64", dataLiteral.Name);

            return Intro<UInt64>(dataLiteral, (context, parseNode) => { return (UInt64)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt64, object>, astForChild: false);
        }

		public static BnfiTermValue<UInt64> IntroNumberLiteralUInt64(NumberLiteral numberLiteral)
        {
            return Intro<UInt64>(numberLiteral, (context, parseNode) => { return (UInt64)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt64, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<UInt64> IntroDataLiteralUInt64(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteralUInt64(dataLiteral);
        }

		public static BnfiTermValue<UInt64> IntroNumberLiteralUInt64(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.IntroNumberLiteralUInt64(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<UInt64> CreateDataLiteralUInt64(string name = "dataliteralUInt64")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.UInt64).IntroDataLiteralUInt64();
		}

        public static BnfiTermValue<UInt64> CreateDataLiteralUInt64Dsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt64).IntroDataLiteralUInt64();
		}

        public static BnfiTermValue<UInt64> CreateDataLiteralUInt64Dsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt64, terminator: terminator).IntroDataLiteralUInt64();
		}

        public static BnfiTermValue<UInt64> CreateDataLiteralUInt64Quoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt64, startEndSymbol: startEndSymbol).IntroDataLiteralUInt64();
		}

        public static BnfiTermValue<UInt64> CreateDataLiteralUInt64Quoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt64, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralUInt64();
		}

        public static BnfiTermValue<UInt64> CreateDataLiteralUInt64FixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.UInt64, length: length).IntroDataLiteralUInt64();
		}

		#endregion

		#region Number

		public static BnfiTermValue<UInt64> CreateNumberLiteralUInt64(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralUInt64();
        }

		public static BnfiTermValue<UInt64> CreateNumberLiteralUInt64(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralUInt64();
        }

		public static BnfiTermValue<UInt64> CreateNumberLiteralUInt64(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralUInt64();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro Single (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Single> IntroDataLiteralSingle(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Single)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Single", dataLiteral.Name);

            return Intro<Single>(dataLiteral, (context, parseNode) => { return (Single)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Single, object>, astForChild: false);
        }

		public static BnfiTermValue<Single> IntroNumberLiteralSingle(NumberLiteral numberLiteral)
        {
            return Intro<Single>(numberLiteral, (context, parseNode) => { return (Single)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Single, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Single> IntroDataLiteralSingle(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteralSingle(dataLiteral);
        }

		public static BnfiTermValue<Single> IntroNumberLiteralSingle(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.IntroNumberLiteralSingle(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Single> CreateDataLiteralSingle(string name = "dataliteralSingle")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Single).IntroDataLiteralSingle();
		}

        public static BnfiTermValue<Single> CreateDataLiteralSingleDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Single).IntroDataLiteralSingle();
		}

        public static BnfiTermValue<Single> CreateDataLiteralSingleDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Single, terminator: terminator).IntroDataLiteralSingle();
		}

        public static BnfiTermValue<Single> CreateDataLiteralSingleQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Single, startEndSymbol: startEndSymbol).IntroDataLiteralSingle();
		}

        public static BnfiTermValue<Single> CreateDataLiteralSingleQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Single, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralSingle();
		}

        public static BnfiTermValue<Single> CreateDataLiteralSingleFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Single, length: length).IntroDataLiteralSingle();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Single> CreateNumberLiteralSingle(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralSingle();
        }

		public static BnfiTermValue<Single> CreateNumberLiteralSingle(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralSingle();
        }

		public static BnfiTermValue<Single> CreateNumberLiteralSingle(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralSingle();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro Double (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Double> IntroDataLiteralDouble(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Double)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Double", dataLiteral.Name);

            return Intro<Double>(dataLiteral, (context, parseNode) => { return (Double)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Double, object>, astForChild: false);
        }

		public static BnfiTermValue<Double> IntroNumberLiteralDouble(NumberLiteral numberLiteral)
        {
            return Intro<Double>(numberLiteral, (context, parseNode) => { return (Double)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Double, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Double> IntroDataLiteralDouble(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteralDouble(dataLiteral);
        }

		public static BnfiTermValue<Double> IntroNumberLiteralDouble(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.IntroNumberLiteralDouble(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Double> CreateDataLiteralDouble(string name = "dataliteralDouble")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Double).IntroDataLiteralDouble();
		}

        public static BnfiTermValue<Double> CreateDataLiteralDoubleDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Double).IntroDataLiteralDouble();
		}

        public static BnfiTermValue<Double> CreateDataLiteralDoubleDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Double, terminator: terminator).IntroDataLiteralDouble();
		}

        public static BnfiTermValue<Double> CreateDataLiteralDoubleQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Double, startEndSymbol: startEndSymbol).IntroDataLiteralDouble();
		}

        public static BnfiTermValue<Double> CreateDataLiteralDoubleQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Double, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralDouble();
		}

        public static BnfiTermValue<Double> CreateDataLiteralDoubleFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Double, length: length).IntroDataLiteralDouble();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Double> CreateNumberLiteralDouble(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralDouble();
        }

		public static BnfiTermValue<Double> CreateNumberLiteralDouble(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralDouble();
        }

		public static BnfiTermValue<Double> CreateNumberLiteralDouble(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralDouble();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro Decimal (Data and Number)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<Decimal> IntroDataLiteralDecimal(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Decimal)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Decimal", dataLiteral.Name);

            return Intro<Decimal>(dataLiteral, (context, parseNode) => { return (Decimal)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Decimal, object>, astForChild: false);
        }

		public static BnfiTermValue<Decimal> IntroNumberLiteralDecimal(NumberLiteral numberLiteral)
        {
            return Intro<Decimal>(numberLiteral, (context, parseNode) => { return (Decimal)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Decimal, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<Decimal> IntroDataLiteralDecimal(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteralDecimal(dataLiteral);
        }

		public static BnfiTermValue<Decimal> IntroNumberLiteralDecimal(this NumberLiteral numberLiteral)
        {
			return BnfiTermValue.IntroNumberLiteralDecimal(numberLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<Decimal> CreateDataLiteralDecimal(string name = "dataliteralDecimal")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Decimal).IntroDataLiteralDecimal();
		}

        public static BnfiTermValue<Decimal> CreateDataLiteralDecimalDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Decimal).IntroDataLiteralDecimal();
		}

        public static BnfiTermValue<Decimal> CreateDataLiteralDecimalDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Decimal, terminator: terminator).IntroDataLiteralDecimal();
		}

        public static BnfiTermValue<Decimal> CreateDataLiteralDecimalQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Decimal, startEndSymbol: startEndSymbol).IntroDataLiteralDecimal();
		}

        public static BnfiTermValue<Decimal> CreateDataLiteralDecimalQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Decimal, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralDecimal();
		}

        public static BnfiTermValue<Decimal> CreateDataLiteralDecimalFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Decimal, length: length).IntroDataLiteralDecimal();
		}

		#endregion

		#region Number

		public static BnfiTermValue<Decimal> CreateNumberLiteralDecimal(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralDecimal();
        }

		public static BnfiTermValue<Decimal> CreateNumberLiteralDecimal(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralDecimal();
        }

		public static BnfiTermValue<Decimal> CreateNumberLiteralDecimal(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralDecimal();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro DateTime (Data)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<DateTime> IntroDataLiteralDateTime(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.DateTime)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a DateTime", dataLiteral.Name);

            return Intro<DateTime>(dataLiteral, (context, parseNode) => { return (DateTime)parseNode.FindToken().Value; }, IdentityFunctionForceCast<DateTime, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<DateTime> IntroDataLiteralDateTime(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteralDateTime(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<DateTime> CreateDataLiteralDateTime(string name = "dataliteralDateTime")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.DateTime).IntroDataLiteralDateTime();
		}

        public static BnfiTermValue<DateTime> CreateDataLiteralDateTimeDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.DateTime).IntroDataLiteralDateTime();
		}

        public static BnfiTermValue<DateTime> CreateDataLiteralDateTimeDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.DateTime, terminator: terminator).IntroDataLiteralDateTime();
		}

        public static BnfiTermValue<DateTime> CreateDataLiteralDateTimeQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.DateTime, startEndSymbol: startEndSymbol).IntroDataLiteralDateTime();
		}

        public static BnfiTermValue<DateTime> CreateDataLiteralDateTimeQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.DateTime, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralDateTime();
		}

        public static BnfiTermValue<DateTime> CreateDataLiteralDateTimeFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.DateTime, length: length).IntroDataLiteralDateTime();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Intro String (Data)

	#region BnfiTermValue

    public partial class BnfiTermValue
	{
        public static BnfiTermValue<String> IntroDataLiteralString(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.String)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a String", dataLiteral.Name);

            return Intro<String>(dataLiteral, (context, parseNode) => { return (String)parseNode.FindToken().Value; }, IdentityFunctionForceCast<String, object>, astForChild: false);
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermValue<String> IntroDataLiteralString(this DataLiteralBase dataLiteral)
        {
			return BnfiTermValue.IntroDataLiteralString(dataLiteral);
        }
	}

	#endregion

	#region Grammar

    public partial class Grammar
	{
		#region Data

        public static BnfiTermValue<String> CreateDataLiteralString(string name = "dataliteralString")
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.String).IntroDataLiteralString();
		}

        public static BnfiTermValue<String> CreateDataLiteralStringDsv(string name)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.String).IntroDataLiteralString();
		}

        public static BnfiTermValue<String> CreateDataLiteralStringDsv(string name, string terminator)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.String, terminator: terminator).IntroDataLiteralString();
		}

        public static BnfiTermValue<String> CreateDataLiteralStringQuoted(string name, string startEndSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.String, startEndSymbol: startEndSymbol).IntroDataLiteralString();
		}

        public static BnfiTermValue<String> CreateDataLiteralStringQuoted(string name, string startSymbol, string endSymbol)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.String, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralString();
		}

        public static BnfiTermValue<String> CreateDataLiteralStringFixedLength(string name, int length)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.String, length: length).IntroDataLiteralString();
		}

		#endregion
	}

	#endregion

	#endregion

}

