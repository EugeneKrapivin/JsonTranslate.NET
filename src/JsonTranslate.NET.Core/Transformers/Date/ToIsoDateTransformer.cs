using System;

using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Date
{
    [Transformer("toisodate")]
    public class ToIsoDateTransformer : SinglyBoundTransformer
    {
        private readonly ToIsoDateTransformerConfig _config;

        public ToIsoDateTransformer() : this(default(JObject)) { }

        public ToIsoDateTransformer(JObject conf)
        {
            _config = conf == null
               ? new ToIsoDateTransformerConfig()
               : this.GetConfig<ToIsoDateTransformerConfig>(conf);
        }

        public ToIsoDateTransformer(ToIsoDateTransformerConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        protected override JToken TransformSingle(JToken root, TransformationContext ctx = null)
        {
            var result = _source.Transform(root, ctx);

            var unixTime = result.Value<long>();

            var dt = _config.Units == "ms"
                ? Constants.UnixEpoch.AddMilliseconds(unixTime)
                : Constants.UnixEpoch.AddSeconds(unixTime);

            return dt.ToString("O");
        }

        public class ToIsoDateTransformerConfig
        {
            [JsonProperty("units")]
            public string Units { get; set; } = "ms"; //TODO add unit validation
        }
    }
}