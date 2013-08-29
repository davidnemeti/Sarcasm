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

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversionTL IntroDataLiteral(DataLiteralBase dataLiteral)
        {
            return Intro(dataLiteral, (context, parseNode) => { return (Object)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Object, object>, astForChild: false).MarkLiteral();
        }

		public static BnfiTermConversionTL IntroNumberLiteral(NumberLiteral numberLiteral)
        {
            return Intro(numberLiteral, (context, parseNode) => { return (Object)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Object, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversionTL IntroDataLiteral(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteral(dataLiteral);
        }

		public static BnfiTermConversionTL IntroNumberLiteral(this NumberLiteral numberLiteral)
        {
			return BnfiTermConversion.IntroNumberLiteral(numberLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversionTL CreateDataLiteral(string name = "dataliteral", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Object).IntroDataLiteral();
		}

        public static BnfiTermConversionTL CreateDataLiteralDsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Object).IntroDataLiteral();
		}

        public static BnfiTermConversionTL CreateDataLiteralDsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Object, terminator: terminator).IntroDataLiteral();
		}

        public static BnfiTermConversionTL CreateDataLiteralQuoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Object, startEndSymbol: startEndSymbol).IntroDataLiteral();
		}

        public static BnfiTermConversionTL CreateDataLiteralQuoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Object, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteral();
		}

        public static BnfiTermConversionTL CreateDataLiteralFixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Object, length: length).IntroDataLiteral();
		}

		#endregion

		#region Number

		public static BnfiTermConversionTL CreateNumberLiteral(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteral();
        }

		public static BnfiTermConversionTL CreateNumberLiteral(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteral();
        }

		public static BnfiTermConversionTL CreateNumberLiteral(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteral();
        }

		#endregion
	}

	#endregion

	#endregion

#if !PCL
	#region Intro DBNull (Data)

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversion<DBNull> IntroDataLiteralDBNull(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.DBNull)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a DBNull", dataLiteral.Name);

            return Intro<DBNull>(dataLiteral, (context, parseNode) => { return (DBNull)parseNode.FindToken().Value; }, IdentityFunctionForceCast<DBNull, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversion<DBNull> IntroDataLiteralDBNull(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteralDBNull(dataLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversion<DBNull> CreateDataLiteralDBNull(string name = "dataliteralDBNull", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.DBNull).IntroDataLiteralDBNull();
		}

        public static BnfiTermConversion<DBNull> CreateDataLiteralDBNullDsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.DBNull).IntroDataLiteralDBNull();
		}

        public static BnfiTermConversion<DBNull> CreateDataLiteralDBNullDsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.DBNull, terminator: terminator).IntroDataLiteralDBNull();
		}

        public static BnfiTermConversion<DBNull> CreateDataLiteralDBNullQuoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.DBNull, startEndSymbol: startEndSymbol).IntroDataLiteralDBNull();
		}

        public static BnfiTermConversion<DBNull> CreateDataLiteralDBNullQuoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.DBNull, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralDBNull();
		}

        public static BnfiTermConversion<DBNull> CreateDataLiteralDBNullFixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.DBNull, length: length).IntroDataLiteralDBNull();
		}

		#endregion
	}

	#endregion

	#endregion

#endif
	#region Intro Boolean (Data)

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversion<Boolean> IntroDataLiteralBoolean(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Boolean)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Boolean", dataLiteral.Name);

            return Intro<Boolean>(dataLiteral, (context, parseNode) => { return (Boolean)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Boolean, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversion<Boolean> IntroDataLiteralBoolean(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteralBoolean(dataLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversion<Boolean> CreateDataLiteralBoolean(string name = "dataliteralBoolean", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Boolean).IntroDataLiteralBoolean();
		}

        public static BnfiTermConversion<Boolean> CreateDataLiteralBooleanDsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Boolean).IntroDataLiteralBoolean();
		}

        public static BnfiTermConversion<Boolean> CreateDataLiteralBooleanDsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Boolean, terminator: terminator).IntroDataLiteralBoolean();
		}

        public static BnfiTermConversion<Boolean> CreateDataLiteralBooleanQuoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Boolean, startEndSymbol: startEndSymbol).IntroDataLiteralBoolean();
		}

        public static BnfiTermConversion<Boolean> CreateDataLiteralBooleanQuoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Boolean, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralBoolean();
		}

        public static BnfiTermConversion<Boolean> CreateDataLiteralBooleanFixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Boolean, length: length).IntroDataLiteralBoolean();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Intro Char (Data)

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversion<Char> IntroDataLiteralChar(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Char)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Char", dataLiteral.Name);

            return Intro<Char>(dataLiteral, (context, parseNode) => { return (Char)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Char, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversion<Char> IntroDataLiteralChar(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteralChar(dataLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversion<Char> CreateDataLiteralChar(string name = "dataliteralChar", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Char).IntroDataLiteralChar();
		}

        public static BnfiTermConversion<Char> CreateDataLiteralCharDsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Char).IntroDataLiteralChar();
		}

        public static BnfiTermConversion<Char> CreateDataLiteralCharDsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Char, terminator: terminator).IntroDataLiteralChar();
		}

        public static BnfiTermConversion<Char> CreateDataLiteralCharQuoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Char, startEndSymbol: startEndSymbol).IntroDataLiteralChar();
		}

        public static BnfiTermConversion<Char> CreateDataLiteralCharQuoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Char, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralChar();
		}

        public static BnfiTermConversion<Char> CreateDataLiteralCharFixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Char, length: length).IntroDataLiteralChar();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Intro SByte (Data and Number)

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversion<SByte> IntroDataLiteralSByte(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.SByte)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a SByte", dataLiteral.Name);

            return Intro<SByte>(dataLiteral, (context, parseNode) => { return (SByte)parseNode.FindToken().Value; }, IdentityFunctionForceCast<SByte, object>, astForChild: false).MarkLiteral();
        }

		public static BnfiTermConversion<SByte> IntroNumberLiteralSByte(NumberLiteral numberLiteral)
        {
            return Intro<SByte>(numberLiteral, (context, parseNode) => { return (SByte)parseNode.FindToken().Value; }, IdentityFunctionForceCast<SByte, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversion<SByte> IntroDataLiteralSByte(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteralSByte(dataLiteral);
        }

		public static BnfiTermConversion<SByte> IntroNumberLiteralSByte(this NumberLiteral numberLiteral)
        {
			return BnfiTermConversion.IntroNumberLiteralSByte(numberLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversion<SByte> CreateDataLiteralSByte(string name = "dataliteralSByte", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.SByte).IntroDataLiteralSByte();
		}

        public static BnfiTermConversion<SByte> CreateDataLiteralSByteDsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.SByte).IntroDataLiteralSByte();
		}

        public static BnfiTermConversion<SByte> CreateDataLiteralSByteDsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.SByte, terminator: terminator).IntroDataLiteralSByte();
		}

        public static BnfiTermConversion<SByte> CreateDataLiteralSByteQuoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.SByte, startEndSymbol: startEndSymbol).IntroDataLiteralSByte();
		}

        public static BnfiTermConversion<SByte> CreateDataLiteralSByteQuoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.SByte, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralSByte();
		}

        public static BnfiTermConversion<SByte> CreateDataLiteralSByteFixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.SByte, length: length).IntroDataLiteralSByte();
		}

		#endregion

		#region Number

		public static BnfiTermConversion<SByte> CreateNumberLiteralSByte(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralSByte();
        }

		public static BnfiTermConversion<SByte> CreateNumberLiteralSByte(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralSByte();
        }

		public static BnfiTermConversion<SByte> CreateNumberLiteralSByte(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralSByte();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro Byte (Data and Number)

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversion<Byte> IntroDataLiteralByte(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Byte)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Byte", dataLiteral.Name);

            return Intro<Byte>(dataLiteral, (context, parseNode) => { return (Byte)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Byte, object>, astForChild: false).MarkLiteral();
        }

		public static BnfiTermConversion<Byte> IntroNumberLiteralByte(NumberLiteral numberLiteral)
        {
            return Intro<Byte>(numberLiteral, (context, parseNode) => { return (Byte)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Byte, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversion<Byte> IntroDataLiteralByte(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteralByte(dataLiteral);
        }

		public static BnfiTermConversion<Byte> IntroNumberLiteralByte(this NumberLiteral numberLiteral)
        {
			return BnfiTermConversion.IntroNumberLiteralByte(numberLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversion<Byte> CreateDataLiteralByte(string name = "dataliteralByte", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Byte).IntroDataLiteralByte();
		}

        public static BnfiTermConversion<Byte> CreateDataLiteralByteDsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Byte).IntroDataLiteralByte();
		}

        public static BnfiTermConversion<Byte> CreateDataLiteralByteDsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Byte, terminator: terminator).IntroDataLiteralByte();
		}

        public static BnfiTermConversion<Byte> CreateDataLiteralByteQuoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Byte, startEndSymbol: startEndSymbol).IntroDataLiteralByte();
		}

        public static BnfiTermConversion<Byte> CreateDataLiteralByteQuoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Byte, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralByte();
		}

        public static BnfiTermConversion<Byte> CreateDataLiteralByteFixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Byte, length: length).IntroDataLiteralByte();
		}

		#endregion

		#region Number

		public static BnfiTermConversion<Byte> CreateNumberLiteralByte(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralByte();
        }

		public static BnfiTermConversion<Byte> CreateNumberLiteralByte(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralByte();
        }

		public static BnfiTermConversion<Byte> CreateNumberLiteralByte(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralByte();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro Int16 (Data and Number)

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversion<Int16> IntroDataLiteralInt16(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Int16)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Int16", dataLiteral.Name);

            return Intro<Int16>(dataLiteral, (context, parseNode) => { return (Int16)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int16, object>, astForChild: false).MarkLiteral();
        }

		public static BnfiTermConversion<Int16> IntroNumberLiteralInt16(NumberLiteral numberLiteral)
        {
            return Intro<Int16>(numberLiteral, (context, parseNode) => { return (Int16)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int16, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversion<Int16> IntroDataLiteralInt16(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteralInt16(dataLiteral);
        }

		public static BnfiTermConversion<Int16> IntroNumberLiteralInt16(this NumberLiteral numberLiteral)
        {
			return BnfiTermConversion.IntroNumberLiteralInt16(numberLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversion<Int16> CreateDataLiteralInt16(string name = "dataliteralInt16", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Int16).IntroDataLiteralInt16();
		}

        public static BnfiTermConversion<Int16> CreateDataLiteralInt16Dsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int16).IntroDataLiteralInt16();
		}

        public static BnfiTermConversion<Int16> CreateDataLiteralInt16Dsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int16, terminator: terminator).IntroDataLiteralInt16();
		}

        public static BnfiTermConversion<Int16> CreateDataLiteralInt16Quoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int16, startEndSymbol: startEndSymbol).IntroDataLiteralInt16();
		}

        public static BnfiTermConversion<Int16> CreateDataLiteralInt16Quoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int16, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralInt16();
		}

        public static BnfiTermConversion<Int16> CreateDataLiteralInt16FixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Int16, length: length).IntroDataLiteralInt16();
		}

		#endregion

		#region Number

		public static BnfiTermConversion<Int16> CreateNumberLiteralInt16(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralInt16();
        }

		public static BnfiTermConversion<Int16> CreateNumberLiteralInt16(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralInt16();
        }

		public static BnfiTermConversion<Int16> CreateNumberLiteralInt16(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralInt16();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro UInt16 (Data and Number)

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversion<UInt16> IntroDataLiteralUInt16(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.UInt16)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a UInt16", dataLiteral.Name);

            return Intro<UInt16>(dataLiteral, (context, parseNode) => { return (UInt16)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt16, object>, astForChild: false).MarkLiteral();
        }

		public static BnfiTermConversion<UInt16> IntroNumberLiteralUInt16(NumberLiteral numberLiteral)
        {
            return Intro<UInt16>(numberLiteral, (context, parseNode) => { return (UInt16)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt16, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversion<UInt16> IntroDataLiteralUInt16(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteralUInt16(dataLiteral);
        }

		public static BnfiTermConversion<UInt16> IntroNumberLiteralUInt16(this NumberLiteral numberLiteral)
        {
			return BnfiTermConversion.IntroNumberLiteralUInt16(numberLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversion<UInt16> CreateDataLiteralUInt16(string name = "dataliteralUInt16", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.UInt16).IntroDataLiteralUInt16();
		}

        public static BnfiTermConversion<UInt16> CreateDataLiteralUInt16Dsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt16).IntroDataLiteralUInt16();
		}

        public static BnfiTermConversion<UInt16> CreateDataLiteralUInt16Dsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt16, terminator: terminator).IntroDataLiteralUInt16();
		}

        public static BnfiTermConversion<UInt16> CreateDataLiteralUInt16Quoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt16, startEndSymbol: startEndSymbol).IntroDataLiteralUInt16();
		}

        public static BnfiTermConversion<UInt16> CreateDataLiteralUInt16Quoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt16, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralUInt16();
		}

        public static BnfiTermConversion<UInt16> CreateDataLiteralUInt16FixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.UInt16, length: length).IntroDataLiteralUInt16();
		}

		#endregion

		#region Number

		public static BnfiTermConversion<UInt16> CreateNumberLiteralUInt16(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralUInt16();
        }

		public static BnfiTermConversion<UInt16> CreateNumberLiteralUInt16(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralUInt16();
        }

		public static BnfiTermConversion<UInt16> CreateNumberLiteralUInt16(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralUInt16();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro Int32 (Data and Number)

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversion<Int32> IntroDataLiteralInt32(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Int32)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Int32", dataLiteral.Name);

            return Intro<Int32>(dataLiteral, (context, parseNode) => { return (Int32)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int32, object>, astForChild: false).MarkLiteral();
        }

		public static BnfiTermConversion<Int32> IntroNumberLiteralInt32(NumberLiteral numberLiteral)
        {
            return Intro<Int32>(numberLiteral, (context, parseNode) => { return (Int32)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int32, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversion<Int32> IntroDataLiteralInt32(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteralInt32(dataLiteral);
        }

		public static BnfiTermConversion<Int32> IntroNumberLiteralInt32(this NumberLiteral numberLiteral)
        {
			return BnfiTermConversion.IntroNumberLiteralInt32(numberLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversion<Int32> CreateDataLiteralInt32(string name = "dataliteralInt32", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Int32).IntroDataLiteralInt32();
		}

        public static BnfiTermConversion<Int32> CreateDataLiteralInt32Dsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int32).IntroDataLiteralInt32();
		}

        public static BnfiTermConversion<Int32> CreateDataLiteralInt32Dsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int32, terminator: terminator).IntroDataLiteralInt32();
		}

        public static BnfiTermConversion<Int32> CreateDataLiteralInt32Quoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int32, startEndSymbol: startEndSymbol).IntroDataLiteralInt32();
		}

        public static BnfiTermConversion<Int32> CreateDataLiteralInt32Quoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int32, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralInt32();
		}

        public static BnfiTermConversion<Int32> CreateDataLiteralInt32FixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Int32, length: length).IntroDataLiteralInt32();
		}

		#endregion

		#region Number

		public static BnfiTermConversion<Int32> CreateNumberLiteralInt32(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralInt32();
        }

		public static BnfiTermConversion<Int32> CreateNumberLiteralInt32(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralInt32();
        }

		public static BnfiTermConversion<Int32> CreateNumberLiteralInt32(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralInt32();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro UInt32 (Data and Number)

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversion<UInt32> IntroDataLiteralUInt32(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.UInt32)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a UInt32", dataLiteral.Name);

            return Intro<UInt32>(dataLiteral, (context, parseNode) => { return (UInt32)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt32, object>, astForChild: false).MarkLiteral();
        }

		public static BnfiTermConversion<UInt32> IntroNumberLiteralUInt32(NumberLiteral numberLiteral)
        {
            return Intro<UInt32>(numberLiteral, (context, parseNode) => { return (UInt32)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt32, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversion<UInt32> IntroDataLiteralUInt32(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteralUInt32(dataLiteral);
        }

		public static BnfiTermConversion<UInt32> IntroNumberLiteralUInt32(this NumberLiteral numberLiteral)
        {
			return BnfiTermConversion.IntroNumberLiteralUInt32(numberLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversion<UInt32> CreateDataLiteralUInt32(string name = "dataliteralUInt32", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.UInt32).IntroDataLiteralUInt32();
		}

        public static BnfiTermConversion<UInt32> CreateDataLiteralUInt32Dsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt32).IntroDataLiteralUInt32();
		}

        public static BnfiTermConversion<UInt32> CreateDataLiteralUInt32Dsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt32, terminator: terminator).IntroDataLiteralUInt32();
		}

        public static BnfiTermConversion<UInt32> CreateDataLiteralUInt32Quoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt32, startEndSymbol: startEndSymbol).IntroDataLiteralUInt32();
		}

        public static BnfiTermConversion<UInt32> CreateDataLiteralUInt32Quoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt32, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralUInt32();
		}

        public static BnfiTermConversion<UInt32> CreateDataLiteralUInt32FixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.UInt32, length: length).IntroDataLiteralUInt32();
		}

		#endregion

		#region Number

		public static BnfiTermConversion<UInt32> CreateNumberLiteralUInt32(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralUInt32();
        }

		public static BnfiTermConversion<UInt32> CreateNumberLiteralUInt32(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralUInt32();
        }

		public static BnfiTermConversion<UInt32> CreateNumberLiteralUInt32(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralUInt32();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro Int64 (Data and Number)

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversion<Int64> IntroDataLiteralInt64(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Int64)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Int64", dataLiteral.Name);

            return Intro<Int64>(dataLiteral, (context, parseNode) => { return (Int64)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int64, object>, astForChild: false).MarkLiteral();
        }

		public static BnfiTermConversion<Int64> IntroNumberLiteralInt64(NumberLiteral numberLiteral)
        {
            return Intro<Int64>(numberLiteral, (context, parseNode) => { return (Int64)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Int64, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversion<Int64> IntroDataLiteralInt64(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteralInt64(dataLiteral);
        }

		public static BnfiTermConversion<Int64> IntroNumberLiteralInt64(this NumberLiteral numberLiteral)
        {
			return BnfiTermConversion.IntroNumberLiteralInt64(numberLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversion<Int64> CreateDataLiteralInt64(string name = "dataliteralInt64", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Int64).IntroDataLiteralInt64();
		}

        public static BnfiTermConversion<Int64> CreateDataLiteralInt64Dsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int64).IntroDataLiteralInt64();
		}

        public static BnfiTermConversion<Int64> CreateDataLiteralInt64Dsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Int64, terminator: terminator).IntroDataLiteralInt64();
		}

        public static BnfiTermConversion<Int64> CreateDataLiteralInt64Quoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int64, startEndSymbol: startEndSymbol).IntroDataLiteralInt64();
		}

        public static BnfiTermConversion<Int64> CreateDataLiteralInt64Quoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Int64, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralInt64();
		}

        public static BnfiTermConversion<Int64> CreateDataLiteralInt64FixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Int64, length: length).IntroDataLiteralInt64();
		}

		#endregion

		#region Number

		public static BnfiTermConversion<Int64> CreateNumberLiteralInt64(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralInt64();
        }

		public static BnfiTermConversion<Int64> CreateNumberLiteralInt64(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralInt64();
        }

		public static BnfiTermConversion<Int64> CreateNumberLiteralInt64(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralInt64();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro UInt64 (Data and Number)

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversion<UInt64> IntroDataLiteralUInt64(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.UInt64)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a UInt64", dataLiteral.Name);

            return Intro<UInt64>(dataLiteral, (context, parseNode) => { return (UInt64)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt64, object>, astForChild: false).MarkLiteral();
        }

		public static BnfiTermConversion<UInt64> IntroNumberLiteralUInt64(NumberLiteral numberLiteral)
        {
            return Intro<UInt64>(numberLiteral, (context, parseNode) => { return (UInt64)parseNode.FindToken().Value; }, IdentityFunctionForceCast<UInt64, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversion<UInt64> IntroDataLiteralUInt64(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteralUInt64(dataLiteral);
        }

		public static BnfiTermConversion<UInt64> IntroNumberLiteralUInt64(this NumberLiteral numberLiteral)
        {
			return BnfiTermConversion.IntroNumberLiteralUInt64(numberLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversion<UInt64> CreateDataLiteralUInt64(string name = "dataliteralUInt64", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.UInt64).IntroDataLiteralUInt64();
		}

        public static BnfiTermConversion<UInt64> CreateDataLiteralUInt64Dsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt64).IntroDataLiteralUInt64();
		}

        public static BnfiTermConversion<UInt64> CreateDataLiteralUInt64Dsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.UInt64, terminator: terminator).IntroDataLiteralUInt64();
		}

        public static BnfiTermConversion<UInt64> CreateDataLiteralUInt64Quoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt64, startEndSymbol: startEndSymbol).IntroDataLiteralUInt64();
		}

        public static BnfiTermConversion<UInt64> CreateDataLiteralUInt64Quoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.UInt64, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralUInt64();
		}

        public static BnfiTermConversion<UInt64> CreateDataLiteralUInt64FixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.UInt64, length: length).IntroDataLiteralUInt64();
		}

		#endregion

		#region Number

		public static BnfiTermConversion<UInt64> CreateNumberLiteralUInt64(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralUInt64();
        }

		public static BnfiTermConversion<UInt64> CreateNumberLiteralUInt64(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralUInt64();
        }

		public static BnfiTermConversion<UInt64> CreateNumberLiteralUInt64(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralUInt64();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro Single (Data and Number)

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversion<Single> IntroDataLiteralSingle(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Single)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Single", dataLiteral.Name);

            return Intro<Single>(dataLiteral, (context, parseNode) => { return (Single)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Single, object>, astForChild: false).MarkLiteral();
        }

		public static BnfiTermConversion<Single> IntroNumberLiteralSingle(NumberLiteral numberLiteral)
        {
            return Intro<Single>(numberLiteral, (context, parseNode) => { return (Single)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Single, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversion<Single> IntroDataLiteralSingle(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteralSingle(dataLiteral);
        }

		public static BnfiTermConversion<Single> IntroNumberLiteralSingle(this NumberLiteral numberLiteral)
        {
			return BnfiTermConversion.IntroNumberLiteralSingle(numberLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversion<Single> CreateDataLiteralSingle(string name = "dataliteralSingle", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Single).IntroDataLiteralSingle();
		}

        public static BnfiTermConversion<Single> CreateDataLiteralSingleDsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Single).IntroDataLiteralSingle();
		}

        public static BnfiTermConversion<Single> CreateDataLiteralSingleDsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Single, terminator: terminator).IntroDataLiteralSingle();
		}

        public static BnfiTermConversion<Single> CreateDataLiteralSingleQuoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Single, startEndSymbol: startEndSymbol).IntroDataLiteralSingle();
		}

        public static BnfiTermConversion<Single> CreateDataLiteralSingleQuoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Single, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralSingle();
		}

        public static BnfiTermConversion<Single> CreateDataLiteralSingleFixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Single, length: length).IntroDataLiteralSingle();
		}

		#endregion

		#region Number

		public static BnfiTermConversion<Single> CreateNumberLiteralSingle(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralSingle();
        }

		public static BnfiTermConversion<Single> CreateNumberLiteralSingle(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralSingle();
        }

		public static BnfiTermConversion<Single> CreateNumberLiteralSingle(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralSingle();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro Double (Data and Number)

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversion<Double> IntroDataLiteralDouble(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Double)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Double", dataLiteral.Name);

            return Intro<Double>(dataLiteral, (context, parseNode) => { return (Double)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Double, object>, astForChild: false).MarkLiteral();
        }

		public static BnfiTermConversion<Double> IntroNumberLiteralDouble(NumberLiteral numberLiteral)
        {
            return Intro<Double>(numberLiteral, (context, parseNode) => { return (Double)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Double, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversion<Double> IntroDataLiteralDouble(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteralDouble(dataLiteral);
        }

		public static BnfiTermConversion<Double> IntroNumberLiteralDouble(this NumberLiteral numberLiteral)
        {
			return BnfiTermConversion.IntroNumberLiteralDouble(numberLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversion<Double> CreateDataLiteralDouble(string name = "dataliteralDouble", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Double).IntroDataLiteralDouble();
		}

        public static BnfiTermConversion<Double> CreateDataLiteralDoubleDsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Double).IntroDataLiteralDouble();
		}

        public static BnfiTermConversion<Double> CreateDataLiteralDoubleDsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Double, terminator: terminator).IntroDataLiteralDouble();
		}

        public static BnfiTermConversion<Double> CreateDataLiteralDoubleQuoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Double, startEndSymbol: startEndSymbol).IntroDataLiteralDouble();
		}

        public static BnfiTermConversion<Double> CreateDataLiteralDoubleQuoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Double, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralDouble();
		}

        public static BnfiTermConversion<Double> CreateDataLiteralDoubleFixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Double, length: length).IntroDataLiteralDouble();
		}

		#endregion

		#region Number

		public static BnfiTermConversion<Double> CreateNumberLiteralDouble(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralDouble();
        }

		public static BnfiTermConversion<Double> CreateNumberLiteralDouble(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralDouble();
        }

		public static BnfiTermConversion<Double> CreateNumberLiteralDouble(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralDouble();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro Decimal (Data and Number)

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversion<Decimal> IntroDataLiteralDecimal(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.Decimal)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a Decimal", dataLiteral.Name);

            return Intro<Decimal>(dataLiteral, (context, parseNode) => { return (Decimal)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Decimal, object>, astForChild: false).MarkLiteral();
        }

		public static BnfiTermConversion<Decimal> IntroNumberLiteralDecimal(NumberLiteral numberLiteral)
        {
            return Intro<Decimal>(numberLiteral, (context, parseNode) => { return (Decimal)parseNode.FindToken().Value; }, IdentityFunctionForceCast<Decimal, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversion<Decimal> IntroDataLiteralDecimal(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteralDecimal(dataLiteral);
        }

		public static BnfiTermConversion<Decimal> IntroNumberLiteralDecimal(this NumberLiteral numberLiteral)
        {
			return BnfiTermConversion.IntroNumberLiteralDecimal(numberLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversion<Decimal> CreateDataLiteralDecimal(string name = "dataliteralDecimal", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.Decimal).IntroDataLiteralDecimal();
		}

        public static BnfiTermConversion<Decimal> CreateDataLiteralDecimalDsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Decimal).IntroDataLiteralDecimal();
		}

        public static BnfiTermConversion<Decimal> CreateDataLiteralDecimalDsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.Decimal, terminator: terminator).IntroDataLiteralDecimal();
		}

        public static BnfiTermConversion<Decimal> CreateDataLiteralDecimalQuoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Decimal, startEndSymbol: startEndSymbol).IntroDataLiteralDecimal();
		}

        public static BnfiTermConversion<Decimal> CreateDataLiteralDecimalQuoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.Decimal, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralDecimal();
		}

        public static BnfiTermConversion<Decimal> CreateDataLiteralDecimalFixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.Decimal, length: length).IntroDataLiteralDecimal();
		}

		#endregion

		#region Number

		public static BnfiTermConversion<Decimal> CreateNumberLiteralDecimal(string name = "numberliteral")
        {
			return new NumberLiteral(name: name).IntroNumberLiteralDecimal();
        }

		public static BnfiTermConversion<Decimal> CreateNumberLiteralDecimal(NumberOptions options)
        {
			return new NumberLiteral(name: "numberliteral", options: options).IntroNumberLiteralDecimal();
        }

		public static BnfiTermConversion<Decimal> CreateNumberLiteralDecimal(string name, NumberOptions options)
        {
			return new NumberLiteral(name: name, options: options).IntroNumberLiteralDecimal();
        }

		#endregion
	}

	#endregion

	#endregion

	#region Intro DateTime (Data)

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversion<DateTime> IntroDataLiteralDateTime(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.DateTime)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a DateTime", dataLiteral.Name);

            return Intro<DateTime>(dataLiteral, (context, parseNode) => { return (DateTime)parseNode.FindToken().Value; }, IdentityFunctionForceCast<DateTime, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversion<DateTime> IntroDataLiteralDateTime(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteralDateTime(dataLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversion<DateTime> CreateDataLiteralDateTime(string name = "dataliteralDateTime", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.DateTime).IntroDataLiteralDateTime();
		}

        public static BnfiTermConversion<DateTime> CreateDataLiteralDateTimeDsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.DateTime).IntroDataLiteralDateTime();
		}

        public static BnfiTermConversion<DateTime> CreateDataLiteralDateTimeDsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.DateTime, terminator: terminator).IntroDataLiteralDateTime();
		}

        public static BnfiTermConversion<DateTime> CreateDataLiteralDateTimeQuoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.DateTime, startEndSymbol: startEndSymbol).IntroDataLiteralDateTime();
		}

        public static BnfiTermConversion<DateTime> CreateDataLiteralDateTimeQuoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.DateTime, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralDateTime();
		}

        public static BnfiTermConversion<DateTime> CreateDataLiteralDateTimeFixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.DateTime, length: length).IntroDataLiteralDateTime();
		}

		#endregion
	}

	#endregion

	#endregion

	#region Intro String (Data)

	#region BnfiTermConversion

    public partial class BnfiTermConversion
	{
        public static BnfiTermConversion<String> IntroDataLiteralString(DataLiteralBase dataLiteral)
        {
            if (dataLiteral.DataType != TypeCode.String)
                GrammarHelper.ThrowGrammarErrorException(GrammarErrorLevel.Error, "terminal '{0}' should be a String", dataLiteral.Name);

            return Intro<String>(dataLiteral, (context, parseNode) => { return (String)parseNode.FindToken().Value; }, IdentityFunctionForceCast<String, object>, astForChild: false).MarkLiteral();
        }
	}

	#endregion

	#region GrammarHelper

    public static partial class GrammarHelper
	{
        public static BnfiTermConversion<String> IntroDataLiteralString(this DataLiteralBase dataLiteral)
        {
			return BnfiTermConversion.IntroDataLiteralString(dataLiteral);
        }
	}

	#endregion

	#region TerminalFactoryS

    public partial class TerminalFactoryS
	{
		#region Data

        public static BnfiTermConversion<String> CreateDataLiteralString(string name = "dataliteralString", string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DataLiteralBase(name: name, dataType: TypeCode.String).IntroDataLiteralString();
		}

        public static BnfiTermConversion<String> CreateDataLiteralStringDsv(string name, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.String).IntroDataLiteralString();
		}

        public static BnfiTermConversion<String> CreateDataLiteralStringDsv(string name, string terminator, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new DsvLiteral(name: name, dataType: TypeCode.String, terminator: terminator).IntroDataLiteralString();
		}

        public static BnfiTermConversion<String> CreateDataLiteralStringQuoted(string name, string startEndSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.String, startEndSymbol: startEndSymbol).IntroDataLiteralString();
		}

        public static BnfiTermConversion<String> CreateDataLiteralStringQuoted(string name, string startSymbol, string endSymbol, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new QuotedValueLiteral(name: name, dataType: TypeCode.String, startSymbol: startSymbol, endSymbol: endSymbol).IntroDataLiteralString();
		}

        public static BnfiTermConversion<String> CreateDataLiteralStringFixedLength(string name, int length, string dateTimeFormat = defaultDateTimeFormat, int intRadix = defaultIntRadix)
		{
			return new FixedLengthLiteral(name: name, dataType: TypeCode.String, length: length).IntroDataLiteralString();
		}

		#endregion
	}

	#endregion

	#endregion

}

