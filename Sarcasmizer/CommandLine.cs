using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarcasmizer
{
    public class CommandLine
    {
        public CommandLine()
        {
            Parameters = new List<IParameter>();
            Examples = new List<Example>();

            ShortNamePrefix = "-";
            LongNamePrefix = "--";
            FormatProvider = CultureInfo.InvariantCulture;
        }

        public string ProgramName { get; set; }
        public string Description { get; set; }
        public IList<IParameter> Parameters { get; private set; }
        public IList<Example> Examples { get; private set; }
        public string ShortNamePrefix { get; set; }
        public string LongNamePrefix { get; set; }
        public IFormatProvider FormatProvider { get; set; }

        public void ShowHelp(TextWriter tw)
        {
            if (!string.IsNullOrEmpty(Description))
            {
                tw.WriteLine(Description);
                tw.WriteLine();
            }

            ShowHelp(tw, Parameters, 0);

            if (Examples.Any())
            {
                tw.WriteLine("Examples:");

                foreach (var example in Examples)
                {
                    tw.WriteLine();

                    if (!string.IsNullOrEmpty(ProgramName))
                        tw.Write("{0} ", ProgramName);

                    tw.WriteLine(string.Join(" ", example.Parameters.Select(parameter => ToString(parameter))));
                    tw.WriteLine("\t{0}", example.Description);
                }
            }
        }

        private string ToString(ExampleParameter parameter)
        {
            return string.Format("{0}{1} {2}", LongNamePrefix, parameter.Parameter.LongName, parameter.Parameter.ValueToString(parameter.Value, FormatProvider));
        }

        private void ShowHelp(TextWriter tw, IEnumerable<IParameter> parameters, int depth)
        {
            string indent = new string('\t', depth);
            string subIndent = new string('\t', depth + 1);

            foreach (var parameter in parameters)
            {
                tw.WriteLine(indent + GetParameterNameWithValue(parameter));

                tw.WriteLine(subIndent + parameter.Description);

                if (parameter.CustomPossibleValues.Any())
                    tw.WriteLine(subIndent + "Possible values are: {0}", string.Join(", ", parameter.CustomPossibleValues.Select(value => Convert.ToString(value, FormatProvider))));
                else if (parameter.Type.IsEnum)
                    tw.WriteLine(subIndent + "Possible values are: {0}", string.Join(", ", Enum.GetNames(parameter.Type)));

                if (parameter.IsOptional)
                    tw.WriteLine(subIndent + "This parameter is optional.");

                if (parameter.IsList)
                    tw.WriteLine(subIndent + "This list parameter can hold multiple values.");

                tw.WriteLine();

                ShowHelp(tw, parameter.GetContextParameters(), depth + 1);
            }
        }

        public void FillParameterValues(IEnumerable<string> args)
        {
            /*
             * NOTE that we want to evaluate possibleParameters sequence returned by GetAllParametersRecursive each time we process a new parameter,
             * since the returned list may depend from the values of the "parent" parameters that just have been set before. Thus we do not convert
             * this sequence to list.
             * */
            var possibleParameters = GetAllParametersRecursive(Parameters);
            IParameter currentParameter = null;

            foreach (string arg in args)
            {
                if (arg.StartsWith(LongNamePrefix) || arg.StartsWith(ShortNamePrefix))
                {
                    HandleParameterIfWithoutValue(currentParameter);

                    try
                    {
                        currentParameter = arg.StartsWith(LongNamePrefix)
                            ? possibleParameters.First(possibleParameter => possibleParameter.LongName == arg.Substring(LongNamePrefix.Length))
                            : possibleParameters.First(possibleParameter => possibleParameter.ShortName == arg.Substring(ShortNamePrefix.Length));
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(string.Format("Unknown parameter name '{0}'", arg), ex);
                    }

                    if (currentParameter != null && currentParameter.IsSpecified)
                        throw new ApplicationException(string.Format("Specified parameter '{0}' more than once", GetParameterName(currentParameter)));
                }
                else
                {
                    if (currentParameter == null)
                        throw new ApplicationException(string.Format("Needs a parameter name with '{0}' or '{1}' as prefix, but found '{2}' instead", LongNamePrefix, ShortNamePrefix, arg));

                    try
                    {
                        currentParameter.SetOrAddValueByParse(arg, FormatProvider);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(string.Format("Parameter '{0}': {1}", GetParameterName(currentParameter), ex.Message), ex);
                    }
                }
            }

            HandleParameterIfWithoutValue(currentParameter);

            var missingRequiredParameters = possibleParameters.Where(parameter => !parameter.IsOptional && !parameter.IsSpecified);

            if (missingRequiredParameters.Any())
            {
                throw new ApplicationException(
                    string.Format(
                        "The following non-optional parameters has not been specified: {0}",
                        string.Join(", ", missingRequiredParameters.Select(parameter => "'" + GetParameterName(parameter) + "'"))
                        )
                    );
            }
        }

        private static IEnumerable<IParameter> GetAllParametersRecursive(IEnumerable<IParameter> parameters)
        {
            return parameters
                .Concat(
                    parameters
                        .Where(parameter => parameter.IsSpecified)
                        .SelectMany(parameter => GetAllParametersRecursive(parameter.GetContextParameters()))
                    );
        }

        private void HandleParameterIfWithoutValue(IParameter parameter)
        {
            if (parameter != null && !parameter.IsSpecified)
            {
                if (parameter.Type == typeof(bool) && !parameter.IsList)
                    ((Parameter<bool>)parameter).SetValue(true);
                else
                    throw new ApplicationException(string.Format("Parameter '{0}' needs a value", GetParameterName(parameter)));
            }
        }

        private string GetParameterName(IParameter parameter, string valueName = "")
        {
            if (parameter.LongName != null && parameter.ShortName != null)
                return string.Format("{0}{1}{4} / {2}{3}{4}", LongNamePrefix, parameter.LongName, ShortNamePrefix, parameter.ShortName, valueName);

            else if (parameter.LongName != null)
                return string.Format("{0}{1}{2}", LongNamePrefix, parameter.LongName, valueName);

            else if (parameter.ShortName != null)
                return string.Format("{0}{1}{2}", ShortNamePrefix, parameter.ShortName, valueName);

            else
                throw new ApplicationException("Neither longname nor shortname specified for parameter");
        }

        private string GetParameterNameWithValue(IParameter parameter)
        {
            string valueName;

            if (parameter.Type == typeof(bool) && !parameter.IsList)
                valueName = string.Empty;
            else
            {
                valueName = string.IsNullOrEmpty(parameter.ValueName) ? parameter.Type.Name : parameter.ValueName;
                valueName = parameter.IsList ? string.Format("{0}_1 {0}_2 ...", valueName) : valueName;
                valueName = " " + valueName;
            }

            return GetParameterName(parameter, valueName);
        }
    }
}
