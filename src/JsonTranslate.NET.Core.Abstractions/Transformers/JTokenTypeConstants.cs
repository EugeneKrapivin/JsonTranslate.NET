using System.Collections.Generic;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Abstractions.Transformers
{
    public static class JTokenTypeConstants
    {
        public static IReadOnlyCollection<JTokenType> Any => new[]
        {
            JTokenType.Float, JTokenType.Boolean, JTokenType.Date,
            JTokenType.TimeSpan, JTokenType.Guid, JTokenType.Integer,
            JTokenType.String
        };

        public static IReadOnlyCollection<JTokenType> Array => new[] { JTokenType.Array };

        public static IReadOnlyCollection<JTokenType> Boolean => new[] { JTokenType.Boolean };

        public static IReadOnlyCollection<JTokenType> String => new[] { JTokenType.String };

        public static IReadOnlyCollection<JTokenType> Numeric => new[] { JTokenType.Integer, JTokenType.Float };

        public static IReadOnlyCollection<JTokenType> Primitive => new[]
        {
            JTokenType.Boolean, JTokenType.Float, JTokenType.Guid, JTokenType.TimeSpan, JTokenType.Integer,
            JTokenType.String
        };

        public static IEnumerable<JTokenType> None => Enumerable.Empty<JTokenType>();

        public static bool IsNumeric(this JToken token) => Numeric.Contains(token.Type);

        public static void EnsureNumeric(this JToken token)
        {
            if (!Numeric.Contains(token.Type))
            {
                throw new IncompatibleTypeException(JTokenType.Float, token.Type);
            }
        }
    }
}