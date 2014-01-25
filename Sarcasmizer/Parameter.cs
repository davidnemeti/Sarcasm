using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sarcasm.Utility;

namespace Sarcasmizer
{
    public interface IParameter
    {
        string LongName { get; }
        string ShortName { get; }
        string ValueName { get; }
        string Description { get; }
        object Value { get; }
        object DefaultValue { get; }
        object[] CustomPossibleValues { get; }
        Type Type { get; }
        bool IsOptional { get; }
        bool IsSpecified { get; }
        bool IsList { get; }
        IEnumerable<IParameter> GetContextParameters();
        void SetOrAddValueByParse(string valueString, IFormatProvider formatProvider);
        string ValueToString(object value, IFormatProvider formatProvider);
    }

    public abstract class ParameterBase<TValue, TDefaultValue, TItem> : IParameter
    {
        public ParameterBase()
        {
            StaticContextParameters = new List<IParameter>();
            DynamicContextParameters = value => Enumerable.Empty<IParameter>();
            PossibleValues = new List<TItem>();
        }

        public string LongName { get; set; }
        public string ShortName { get; set; }
        public string ValueName { get; set; }
        public TDefaultValue DefaultValue { get; set; }
        public string Description { get; set; }
        public bool IsOptional { get; set; }
        public bool IsSpecified { get; protected set; }

        public abstract bool IsList { get; }

        public Converter<TItem> CustomConverter { get; set; }
        public IList<TItem> PossibleValues { get; set; }
        public IList<IParameter> StaticContextParameters { get; private set; }
        public Func<TValue, IEnumerable<IParameter>> DynamicContextParameters { get; set; }

        public abstract TValue Value
        {
            get;
            protected set;
        }

        public static implicit operator TValue(ParameterBase<TValue, TDefaultValue, TItem> parameter)
        {
            return parameter.Value;
        }

        public IEnumerable<IParameter> GetContextParameters()
        {
            return StaticContextParameters.Concat(DynamicContextParameters(Value));
        }

        protected abstract void SetOrAddValueByParse(string valueString, IFormatProvider formatProvider);
        protected abstract string ValueToString(object value, IFormatProvider formatProvider);

        protected TItem Parse(string valueString, IFormatProvider formatProvider)
        {
            if (CustomConverter != null)
                return CustomConverter.Parse(valueString, formatProvider);

            else if (typeof(TItem) == typeof(FileInfo))
                return (TItem)(object)new FileInfo(valueString);

            else if (typeof(TItem) == typeof(DirectoryInfo))
                return (TItem)(object)new DirectoryInfo(valueString);

            else if (typeof(TItem) == typeof(CultureInfo))
                return (TItem)(object)new CultureInfo(valueString);

            else if (typeof(TItem).IsEnum)
                return (TItem)Enum.Parse(typeof(TItem), valueString);

            else
                return (TItem)Convert.ChangeType(valueString, typeof(TItem), formatProvider);
        }

        protected string ItemToString(TItem value, IFormatProvider formatProvider)
        {
            return CustomConverter != null
                ? CustomConverter.ToString(value, formatProvider)
                : Convert.ToString(value, formatProvider);
        }

        public Type Type
        {
            get { return typeof(TItem); }
        }

        object IParameter.Value
        {
            get { return Value; }
        }

        object IParameter.DefaultValue
        {
            get { return DefaultValue; }
        }

        void IParameter.SetOrAddValueByParse(string valueString, IFormatProvider formatProvider)
        {
            SetOrAddValueByParse(valueString, formatProvider);
        }

        string IParameter.ValueToString(object value, IFormatProvider formatProvider)
        {
            return ValueToString(value, formatProvider);
        }

        object[] IParameter.CustomPossibleValues
        {
            get
            {
                return PossibleValues.Cast<object>().ToArray();
            }
        }
    }

    public class Parameter<T> : ParameterBase<T, T, T>
    {
        public Parameter()
        {
            if (typeof(T) == typeof(bool))
                IsOptional = true;      // a bool parameter is automatically optional
        }

        public override bool IsList { get { return false; } }

        internal void SetValue(T value)
        {
            if (IsSpecified)
                throw new InvalidOperationException("Multiple set for a non-list parameter");

            if (PossibleValues.Any() && !value.EqualToAny(PossibleValues))
                throw new InvalidOperationException(string.Format("'{0}' is not a possible value", value));

            Value = value;
            IsSpecified = true;
        }

        private T _value;
        public override T Value
        {
            get { return IsSpecified ? _value : DefaultValue; }
            protected set { _value = value; }
        }

        protected override void SetOrAddValueByParse(string valueString, IFormatProvider formatProvider)
        {
            SetValue(Parse(valueString, formatProvider));
        }

        protected override string ValueToString(object value, IFormatProvider formatProvider)
        {
            return ItemToString((T)value, formatProvider);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class ParameterList<T> : ParameterBase<IReadOnlyList<T>, IList<T>, T>
    {
        public override bool IsList { get { return true; } }

        public ParameterList()
        {
            Value = new List<T>();
            DefaultValue = new List<T>();
        }

        protected void AddValue(T value)
        {
            if (PossibleValues.Any() && !value.EqualToAny(PossibleValues))
                throw new ArgumentOutOfRangeException(string.Format("'{0}' is not a possible value", value));

            _value.Add(value);
            IsSpecified = true;
        }

        private List<T> _value;
        public override IReadOnlyList<T> Value
        {
            get { return IsSpecified ? _value : (List<T>)DefaultValue; }
            protected set { _value = value.ToList(); }
        }

        protected override void SetOrAddValueByParse(string valueString, IFormatProvider formatProvider)
        {
            AddValue(Parse(valueString, formatProvider));
        }

        protected override string ValueToString(object value, IFormatProvider formatProvider)
        {
            return string.Join(" ", ((IEnumerable<T>)value).Select(item => ItemToString(item, formatProvider)));
        }

        public override string ToString()
        {
            return string.Join(", ", Value);
        }
    }

    public class Converter<T>
    {
        public Func<string, IFormatProvider, T> Parse { get; private set; }
        new public Func<T, IFormatProvider, string> ToString { get; private set; }

        public Converter(Func<string, IFormatProvider, T> parse, Func<T, IFormatProvider, string> toString)
        {
            this.Parse = parse;
            this.ToString = toString;
        }
    }
}
