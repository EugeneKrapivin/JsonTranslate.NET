
using System;

namespace JsonTranslate.NET.Core.Transformers.Date
{
    public static class Constants
    {
        public static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, 0, kind: DateTimeKind.Utc);
    }
}