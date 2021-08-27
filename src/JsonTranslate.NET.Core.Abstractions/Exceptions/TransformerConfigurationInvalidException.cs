﻿
using System;

namespace JsonTranslate.NET.Core.Abstractions.Exceptions
{
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
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}