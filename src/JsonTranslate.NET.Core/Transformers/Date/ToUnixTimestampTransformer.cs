using System;

using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Date
{
    [Transformer("toepoch")]
    public class ToUnixTimestampTransformer : SinglyBoundTransformer
    {
        private readonly ToUnixTimestampTransformerConfig _config;

        public ToUnixTimestampTransformer() : this(default(JObject)) { }

        public ToUnixTimestampTransformer(JObject conf)
        {
            _config = conf == null
               ? new ToUnixTimestampTransformerConfig()
               : this.GetConfig<ToUnixTimestampTransformerConfig>(conf);
        }

        public ToUnixTimestampTransformer(ToUnixTimestampTransformerConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        protected override JToken TransformSingle(JToken root, TransformationContext ctx = null)
        {
            var result = _source.Transform(root, ctx);

            var dts = result.Value<string>();

            if (!DateTime.TryParse(dts, out var dt))
            {
                throw new ArgumentException($"Failed to parse string \"{dts}\" to date");
            }

            dt = dt.ToUniversalTime();
            if (dt.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException($"Failed to determine date time kind for \"{dts}\"");
            }

            var response = dt - Constants.UnixEpoch;

            return _config.Units == "ms"
                ? response.TotalMilliseconds
                : response.TotalSeconds;
        }

        public class ToUnixTimestampTransformerConfig
        {
            [JsonProperty("units")]
            public string Units { get; set; } = "ms"; // todo: validate units
        }
    }
}