
using System;

namespace JsonTranslate.NET.Core.Abstractions.Exceptions
{
    [Serializable]
    public class InvalidTransformerSetupException : Exception
    {
        public InvalidTransformerSetupException(string message) : base(message) { }
        public InvalidTransformerSetupException(string message, Exception inner) : base(message, inner) { }
        protected InvalidTransformerSetupException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
