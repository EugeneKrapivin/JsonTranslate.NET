using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.String.Reducers
{
    [Transformer(name: "str_join", requiresConfig: true)]
    public class StringJoinTransformer : AbstractStringReducingTransformer
    {
        private readonly StringJoinTransformerConfig _config;

        public StringJoinTransformer(JObject conf)
        {
            if (conf == null) throw new ArgumentNullException($"{nameof(ValueOfTransformer)} requires configuration");

            _config = this.GetConfig<StringJoinTransformerConfig>(conf);
        }

        public override JObject Config => JObject.FromObject(_config);

        protected override JToken Reduce(IEnumerable<string> values, TransformationContext ctx = null) => string.Join(_config.Separator, values);

        public class StringJoinTransformerConfig
        {
            [JsonProperty("separator")]
            public string Separator { get; set; }
        }
    }
}