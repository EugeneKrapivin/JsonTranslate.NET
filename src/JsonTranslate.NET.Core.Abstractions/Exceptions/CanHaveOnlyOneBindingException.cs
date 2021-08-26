
using System;

namespace JsonTranslate.NET.Core.Abstractions.Exceptions
{
    [Serializable]
    public class CanHaveOnlyOneBindingException : Exception
    {
        public CanHaveOnlyOneBindingException(string transformerName) : base($"Transformer `{transformerName}` is a singly bound transformer.") { }
        
        protected CanHaveOnlyOneBindingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
