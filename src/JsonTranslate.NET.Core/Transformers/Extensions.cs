using System;

using JsonTranslate.NET.Core.Abstractions.Exceptions;

using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers
{
    public static class Extensions
    {
        public static JToken ValidateNonNull(this JToken tok) => tok ?? throw new TransformationResultCannotBeNullException();

        public static JToken ValidateType(this JToken tok, JTokenType type) => 
            tok.Type == type 
                ? tok 
                : throw new IncompatibleTypeException(type, tok.Type);

        public static T As<T>(this JToken tok) where T : JToken =>
            tok as T; // todo: blasphemy 
    }
}