
using System;

namespace JsonTranslate.NET.Core.Abstractions.Exceptions
{
    [Serializable]
    public class ValueProvidersCannotBeBoundException : Exception
    {
        public ValueProvidersCannotBeBoundException(string transformerName) : base ($"Transformer `{transformerName}` is a value provider and can not have bindings.") { }
        
        protected ValueProvidersCannotBeBoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
