using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UsefulClasses.Exceptions;

namespace UsefulClasses
{
    public class ParameterManager
    {
        private List<Parameter> parameters = new List<Parameter>();

        public bool ThrowOnUnregisteredParameter { get; set; }

        public ParameterManager()
        {
            ThrowOnUnregisteredParameter = false;
        }

        public void RegisterParameter(Parameter parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter");
            if (string.IsNullOrEmpty(parameter.Label))
                throw new InvalidLabelException("Label must not be null or empty.");
            if (parameters.Any(p => p.Label == parameter.Label))
                throw new DuplicateLabelException("Label must be unique.");

            parameters.Add(parameter);
        }

        public void ProcessParameters(IEnumerable<string> parameterArray)
        {
            foreach (var param in parameterArray)
            {
                if (!param.StartsWith("/"))
                    throw new InvalidParameterException(string.Format("Parameter does not match correct format. Initial slash not found : {0}", param));
                var strippedParam = param.Substring(1);
                var parts = strippedParam.Split(new[] { ":" }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                    throw new InvalidParameterException(string.Format("Parameter does not match correct format. 'Label:Value' format not found : {0}", param));

                var parameter = parameters.FirstOrDefault(p => p.Label == parts[0]);
                if (parameter == null)
                    throw new InvalidParameterException(string.Format("Parameter label does not match registered parameters : {0}", parts[0]));
                parameter.ParseValue(parts[1]);
            }
        }

        public string GenerateParameterStatusMessage()
        {
            var maxLength = parameters.Max(p => p.FriendlyName.Length);
            var builder = new StringBuilder();

            foreach (var p in parameters)
            {
                builder.AppendFormat("{0} : {1}", p.FriendlyName.PadRight(maxLength), p.Value);
                builder.AppendLine();
            }
            builder.AppendLine();

            return builder.ToString();
        }

        public string GenerateCommandLineUsageMessage(string programName)
        {
            var builder = new StringBuilder();

            builder.AppendLine("Command Line Usage : ");

            var requiredParams = string.Join(" ", parameters.Where(p => p.Required).Select(p => string.Format("/{0}:{1}", p.Label, p.ValueType)));
            var optionalParams = string.Join(" ", parameters.Where(p => !p.Required).Select(p => string.Format("[/{0}:{1}]", p.Label, p.ValueType)));

            var str = string.Format("{0} {1} {2}", programName, requiredParams, optionalParams);
            var wrapLength = 80 - (programName.Length + 2);
            var usage = str.Wrap(wrapLength);

            builder.AppendLine(usage.First());
            foreach (var l in usage.Skip(1))
            {
                builder.Append(' ', programName.Length + 1);
                builder.AppendLine(l);
            }

            var labelMaxLength = parameters.Max(p => p.Label.Length);

            wrapLength = 80 - (5 + labelMaxLength + 3 + 1);

            foreach (var p in parameters)
            {
                str = p.Description;
                if (!p.Required)
                    str += " Default : " + p.DefaultValue.ToString();
                var desc = str.Wrap(wrapLength);

                builder.AppendFormat("    /{0} : {1}", p.Label.PadRight(labelMaxLength), desc.First());
                builder.AppendLine();
                foreach (var l in desc.Skip(1))
                {
                    builder.Append(' ', 5 + labelMaxLength + 3);
                    builder.Append(l);
                    builder.AppendLine();
                }
            }

            builder.AppendLine();

            return builder.ToString();
        }
    }
}
