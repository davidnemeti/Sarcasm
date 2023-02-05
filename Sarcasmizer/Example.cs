using System.Collections.Generic;

namespace Sarcasmizer
{
    public class Example
    {
        public Example()
        {
            Parameters = new List<ExampleParameter>();
        }

        public IList<ExampleParameter> Parameters { get; private set; }
        public string Description { get; set; }
    }

    public class ExampleParameter
    {
        public IParameter Parameter { get; private set; }
        public object Value { get; private set; }

        private ExampleParameter(IParameter parameter, object value)
        {
            this.Parameter = parameter;
            this.Value = value;
        }

        public static ExampleParameter CreateFrom<T>(Parameter<T> parameter, T value)
        {
            return new ExampleParameter(parameter, value);
        }

        public static ExampleParameter CreateFrom<T>(ParameterList<T> parameter, params T[] values)
        {
            return new ExampleParameter(parameter, values);
        }
    }

    public static class ParameterExtensions
    {
        public static ExampleParameter WithValue<T>(this Parameter<T> parameter, T value)
        {
            return ExampleParameter.CreateFrom(parameter, value);
        }

        public static ExampleParameter WithValue<T>(this ParameterList<T> parameter, params T[] values)
        {
            return ExampleParameter.CreateFrom(parameter, values);
        }
    }
}
