//***********************************************************************
// Assembly         : UsefulClasses
// Author           : Tony Richards
// Created          : 08-18-2011
//
// Last Modified By : Tony Richards
// Last Modified On : 09-10-2011
// Description      : 
//
// Copyright (c) 2011, Tony Richards
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// Redistributions of source code must retain the above copyright notice, this list
// of conditions and the following disclaimer.
//
// Redistributions in binary form must reproduce the above copyright notice, this
// list of conditions and the following disclaimer in the documentation and/or other
// materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
// IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
// BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
// OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
// OF THE POSSIBILITY OF SUCH DAMAGE.
//***********************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UsefulClasses.Exceptions;

namespace UsefulClasses
{
    /// <summary>
    /// Manages all the parameters used by the program, along with generating messages explaining their use to the user.
    /// </summary>
    public class ParameterManager
    {
        private List<Parameter> parameters = new List<Parameter>();

        /// <summary>
        /// Gets or sets a value indicating whether the manager should throw on unregistered parameter.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the manager should throw a <see cref="Exceptions.InvalidParameterException"/> when encountering an unregistered; otherwise, <c>false</c>.
        /// </value>
        public bool ThrowOnUnregisteredParameter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterManager"/> class.
        /// </summary>
        public ParameterManager()
        {
            ThrowOnUnregisteredParameter = false;
        }

        /// <summary>
        /// Registers a parameter.
        /// </summary>
        /// <param name="parameter">The parameter to register.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="parameter"/> parameter is null.</exception>
        /// <exception cref="Exceptions.InvalidLabelException">The parameters label is null or empty.</exception>
        /// <exception cref="Exceptions.DuplicateLabelException">The label has already been registered.</exception>
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

        /// <summary>
        /// Processes the parameters.
        /// </summary>
        /// <param name="parameterArray">The parameter array.</param>
        /// <exception cref="Exceptions.InvalidParameterException">The parameter is the wrong format or is not registered.</exception>
        public void ProcessParameters(IEnumerable<string> parameterArray)
        {
            foreach (var parameter in parameterArray)
            {
                if (!parameter.StartsWith("/", StringComparison.InvariantCulture))
                    throw new InvalidParameterException(string.Format(CultureInfo.CurrentCulture, "Parameter does not match correct format. Initial slash not found : {0}", parameter));
                var strippedParameter = parameter.Substring(1);
                var parts = strippedParameter.Split(new[] { ":" }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                    throw new InvalidParameterException(string.Format(CultureInfo.CurrentCulture, "Parameter does not match correct format. 'Label:Value' format not found : {0}", parameter));

                var registeredParameter = parameters.FirstOrDefault(p => p.Label == parts[0]);
                if (ThrowOnUnregisteredParameter && registeredParameter == null)
                    throw new InvalidParameterException(string.Format(CultureInfo.CurrentCulture, "Parameter label does not match registered parameters : {0}", parts[0]));
                registeredParameter.ParseValue(parts[1]);
            }
        }

        /// <summary>
        /// Generates the parameter status message.
        /// </summary>
        /// <returns>A string containing the status of all the registered parameters.</returns>
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

        /// <summary>
        /// Generates the command line usage message.
        /// </summary>
        /// <param name="programName">Name of the program.</param>
        /// <returns>A string containing the command line usage for the program.</returns>
        public string GenerateCommandLineUsageMessage(string programName)
        {
            var builder = new StringBuilder();

            builder.AppendLine("Command Line Usage : ");

            var requiredParameters = string.Join(" ", parameters.Where(p => p.Required).Select(p => string.Format(CultureInfo.InvariantCulture, "/{0}:{1}", p.Label, p.ValueType)));
            var optionalParameters = string.Join(" ", parameters.Where(p => !p.Required).Select(p => string.Format(CultureInfo.InvariantCulture, "[/{0}:{1}]", p.Label, p.ValueType)));

            var str = string.Format(CultureInfo.CurrentCulture, "{0} {1} {2}", programName, requiredParameters, optionalParameters);
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
