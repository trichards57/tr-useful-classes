using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsefulClasses
{
    public abstract class Parameter
    {
        public object DefaultValue { get; private set; }
        public object Value { get; protected set; }

        public bool IsDefaultValue { get; protected set; }
        public string Label { get; private set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }

        protected Parameter(string label, object defaultValue)
        {
            if (string.IsNullOrWhiteSpace(label))
                throw new ArgumentOutOfRangeException("label");

            Label = label;
            DefaultValue = defaultValue;
            Value = DefaultValue;
            IsDefaultValue = true;
            FriendlyName = label;
            Required = false;
        }

        public abstract void ParseValue(string value);

        public string ValueType { get; protected set; }
    }

    public class Parameter<T> : Parameter
    {
        public new T DefaultValue { get { return (T)base.DefaultValue; } }
        public new T Value { get { return (T)base.Value; } }

        private Func<string, T> parseFunction;

        public Parameter(string label, T defaultValue, Func<string, T> parseFunction)
            : base(label, defaultValue)
        {
            if (parseFunction == null)
                throw new ArgumentNullException("parseFunction");
            this.parseFunction = parseFunction;
            ValueType = typeof(T).Name;
        }

        public override void ParseValue(string value)
        {
            base.Value = parseFunction(value);
            IsDefaultValue = false;
        }
    }
}
