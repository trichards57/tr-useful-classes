using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsefulClasses.Exceptions
{
    [Serializable]
    public class InvalidLabelException : Exception
    {
        public InvalidLabelException() { }
        public InvalidLabelException(string message) : base(message) { }
        public InvalidLabelException(string message, Exception inner) : base(message, inner) { }
        protected InvalidLabelException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class DuplicateLabelException : Exception
    {
        public DuplicateLabelException() { }
        public DuplicateLabelException(string message) : base(message) { }
        public DuplicateLabelException(string message, Exception inner) : base(message, inner) { }
        protected DuplicateLabelException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException() { }
        public InvalidParameterException(string message) : base(message) { }
        public InvalidParameterException(string message, Exception inner) : base(message, inner) { }
        protected InvalidParameterException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
