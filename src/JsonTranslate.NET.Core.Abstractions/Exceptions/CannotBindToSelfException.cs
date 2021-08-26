
using System;

namespace JsonTranslate.NET.Core.Abstractions.Exceptions
{
    [Serializable]
    public class CannotBindToSelfException : Exception
    {
        public CannotBindToSelfException() : this ("A bound transformer can not be the same instance.") { }
        
        public CannotBindToSelfException(string message) : base(message) { }
    }
}
