﻿
using System;

namespace JsonTranslate.NET.Core.Abstractions.Exceptions
{
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
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
