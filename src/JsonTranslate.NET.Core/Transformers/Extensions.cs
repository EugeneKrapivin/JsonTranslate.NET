using System;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers
{
    public static class Extensions
    {
        internal static Type GetType(JTokenType jType)
        {
            return jType switch
            {
                JTokenType.Object => typeof(object),
                JTokenType.Array => typeof(Array),
                JTokenType.Integer => typeof(int),
                JTokenType.Float => typeof(float),
                JTokenType.String => typeof(string),
                JTokenType.Boolean => typeof(bool),
                JTokenType.Date => typeof(DateTime),
                JTokenType.Bytes => typeof(byte),
                JTokenType.Guid => typeof(Guid),
                JTokenType.TimeSpan => typeof(TimeSpan),
                _ => null
            };
        }
    }
}