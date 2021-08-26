
using System;

namespace JsonTranslate.NET.Core.Abstractions.Exceptions
{
    [Serializable]
    public class BindingCreateGraphCycleException : Exception
    {
        public BindingCreateGraphCycleException(string transformerName) : base ($"Binding the `{transformerName}` transformer will cause a cycle in a the transformation tree.") { }
       
        protected BindingCreateGraphCycleException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
