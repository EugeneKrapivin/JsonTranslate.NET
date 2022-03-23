using System;
using System.Globalization;

using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Date
{
    [Transformer("fmtdate")]
    public class DateToDateTransformer : SinglyBoundTransformer
    {
        private readonly DateFormatTransformerConfig _config;

        public DateToDateTransformer(JObject conf)
        {
            if (conf == null)
            {
                throw new ArgumentNullException(nameof(conf));
            }
            _config = this.GetConfig<DateFormatTransformerConfig>(conf);
        }

        public DateToDateTransformer(DateFormatTransformerConfig conf)
        {
            _config = conf ?? throw new ArgumentNullException(nameof(conf));
        }

        protected override JToken TransformSingle(JToken root, TransformationContext ctx = null)
        {
            var result = _source.Transform(root, ctx);

            var wasParsed = DateTimeOffset.TryParseExact(
                result.Value<string>(),
                _config.SourceFormat,
                DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.AssumeUniversal,
                out var parsedDate);

            if (!wasParsed)
            {
                throw new ArgumentException($"Failed to parse string \"{result}\" to date format \"{_config.SourceFormat}\"");
            }

            return parsedDate.ToString(_config.TargetFormat, DateTimeFormatInfo.InvariantInfo);
        }

        public class DateFormatTransformerConfig
        {
            [JsonProperty("srcFmt")]
            public string SourceFormat { get; set; }

            [JsonProperty("trgFmt")]
            public string TargetFormat { get; set; }
        }
    }
}