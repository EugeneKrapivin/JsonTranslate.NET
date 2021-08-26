
using System;

namespace JsonTranslate.NET.Core.Abstractions.Exceptions
{
    [Serializable]
    public class CannotBindToNullException : Exception
    {
        public CannotBindToNullException() : base("A bound transformer source cannot be `null`.") { }
        
        public CannotBindToNullException(string message, Exception inner) : base(message, inner) { }
    }
}
