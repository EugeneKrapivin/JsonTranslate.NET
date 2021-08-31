using System;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers
{
    [Transformer(name: "valueof", requiresConfig: true)]
    public class ValueOfTransformer : ValueProvidingTransformer
    {
        private readonly ValueOfTransformerConfig _valueOfTransformerConfig;

        private static readonly JsonSelectSettings JsonSelectSettings = new()
        {
            ErrorWhenNoMatch = true,
            RegexMatchTimeout = TimeSpan.FromMilliseconds(25)
        };

        public override JObject Config => JObject.FromObject(_valueOfTransformerConfig);

        public ValueOfTransformer(JObject conf)
        {
            _valueOfTransformerConfig = conf == null 
                    ? throw new ArgumentNullException($"{nameof(ValueOfTransformer)} requires configuration")
                    : this.GetConfig<ValueOfTransformerConfig>(conf);
        }

        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var result = root.SelectToken(_valueOfTransformerConfig.Path, JsonSelectSettings);
            if (result == null) throw new Exception($"Path `{_valueOfTransformerConfig.Path}` didn't yield a value");

            return result;
        }

        public class ValueOfTransformerConfig
        {
            [JsonProperty("path")]
            public string Path { get; set; }
        }
    }
}