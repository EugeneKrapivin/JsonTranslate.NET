using Newtonsoft.Json.Linq;

using System;

namespace JsonTranslate.NET.Core.Abstractions.Exceptions
{


    [Serializable]
    public class IncompatibleTypeException : Exception
    {
        public IncompatibleTypeException(JTokenType expected, JTokenType actual) 
            : base($"Transformer expected result of type `{expected}` but got `{actual}`"){ }
        
        protected IncompatibleTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
