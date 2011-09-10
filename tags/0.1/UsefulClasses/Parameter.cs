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

namespace UsefulClasses
{
    /// <summary>
    /// The base class for all types of Parameter to allow all generic implementations to be stored in a single collection.
    /// </summary>
    public abstract class Parameter
    {
        /// <summary>
        /// Gets the default value for the parameter.
        /// </summary>
        public object DefaultValue { get; private set; }
        /// <summary>
        /// Gets or sets the input value of the parameter.
        /// </summary>
        /// <value>
        /// The value of the parameter.
        /// </value>
        public object Value { get; protected set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance's value is currently at the default value.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this value is the default value; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefaultValue { get; protected set; }
        /// <summary>
        /// Gets the label assigned to the parameter.
        /// </summary>
        public string Label { get; private set; }
        /// <summary>
        /// Gets or sets the friendly name of the parameter.
        /// </summary>
        /// <value>
        /// The friendly name.
        /// </value>
        public string FriendlyName { get; set; }
        /// <summary>
        /// Gets or sets the description of the parameter.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Parameter"/> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        public bool Required { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameter"/> class.
        /// </summary>
        /// <param name="label">The parameter's label.</param>
        /// <param name="defaultValue">The parameter's default value.</param>
        /// <exception cref="ArgumentException">The <paramref name="label"/> parameter is null or whitespace.</exception>
        protected Parameter(string label, object defaultValue)
        {
            if (string.IsNullOrWhiteSpace(label))
                throw new ArgumentException("label");

            Label = label;
            DefaultValue = defaultValue;
            Value = DefaultValue;
            IsDefaultValue = true;
            FriendlyName = label;
            Required = false;
        }

        /// <summary>
        /// Function to parse the parameter value from the command line string.
        /// </summary>
        /// <param name="value">The value to parse.</param>
        public abstract void ParseValue(string value);

        /// <summary>
        /// Gets or sets the name of the type of the value.
        /// </summary>
        /// <value>
        /// The type of the name of the type of the value.
        /// </value>
        public string ValueType { get; protected set; }
    }

    /// <summary>
    /// Generic <see cref="Parameter"/> to allow type-safe parameters.
    /// </summary>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    public class Parameter<T> : Parameter
    {
        /// <summary>
        /// Gets the default value for the parameter.
        /// </summary>
        public new T DefaultValue { get { return (T)base.DefaultValue; } }
        /// <summary>
        /// Gets the input value of the parameter.
        /// </summary>
        /// <value>
        /// The value of the parameter.
        /// </value>
        public new T Value { get { return (T)base.Value; } }

        private Func<string, T> parseFunction;

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameter&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="label">The parameter's label.</param>
        /// <param name="defaultValue">The parameter's default value.</param>
        /// <param name="parseFunction">The function to parse from a string to <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="parseFunction"/> is null.</exception>
        public Parameter(string label, T defaultValue, Func<string, T> parseFunction)
            : base(label, defaultValue)
        {
            if (parseFunction == null)
                throw new ArgumentNullException("parseFunction");
            this.parseFunction = parseFunction;
            ValueType = typeof(T).Name;
        }

        /// <summary>
        /// Function to parse the parameter value from the command line string.
        /// </summary>
        /// <param name="value">The value to parse.</param>
        public override void ParseValue(string value)
        {
            base.Value = parseFunction(value);
            IsDefaultValue = false;
        }
    }
}
