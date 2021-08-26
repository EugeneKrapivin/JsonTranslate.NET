
using System;

namespace JsonTranslate.NET.Core.Abstractions.Exceptions
{
    [Serializable]
    public class TransformationResultCannotBeNullException : Exception
    {
        public TransformationResultCannotBeNullException() { }
        public TransformationResultCannotBeNullException(string message) : base(message) { }
        public TransformationResultCannotBeNullException(string message, Exception inner) : base(message, inner) { }
        protected TransformationResultCannotBeNullException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
