using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace JsonTranslate.NET.Core.Exceptions
{
    [Serializable]
    public class TransformerBindingException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public TransformerBindingException()
        {
        }

        public TransformerBindingException(string message) : base(message)
        {
        }

        public TransformerBindingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected TransformerBindingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
    [Serializable]
    public class TransformerConfigurationInvalidException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public TransformerConfigurationInvalidException()
        {
        }

        public TransformerConfigurationInvalidException(string message) : base(message)
        {
        }

        public TransformerConfigurationInvalidException(string message, Exception inner) : base(message, inner)
        {
        }

        protected TransformerConfigurationInvalidException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
    [Serializable]
    public class TransformerConfigurationMissingException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public TransformerConfigurationMissingException()
        {
        }

        public TransformerConfigurationMissingException(string message) : base(message)
        {
        }

        public TransformerConfigurationMissingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected TransformerConfigurationMissingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
    [Serializable]
    public class BadTransformerBindingException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public BadTransformerBindingException()
        {
        }

        public BadTransformerBindingException(string message) : base(message)
        {
        }

        public BadTransformerBindingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected BadTransformerBindingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
