using System;
using System.Collections.Generic;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Collections
{
    [Transformer("deconstruct")]
    public sealed class DeconstructTransformer : SinglyBoundTransformer
    {
        private readonly DeconstructTransformerConfig _config;

        public override IEnumerable<JTokenType> SupportedTypes => new[] { JTokenType.Object };
        public override IEnumerable<JTokenType> SupportedResults => JTokenTypeConstants.Array;

        public DeconstructTransformer() : this(default(JObject))
        {

        }
        public DeconstructTransformer(JObject conf)
        {
            _config = conf == null 
                ? new DeconstructTransformerConfig() 
                : this.GetConfig<DeconstructTransformerConfig>(conf);
        }

        public DeconstructTransformer(DeconstructTransformerConfig conf)
        {
            _config = conf ?? throw new ArgumentNullException(nameof(conf));
        }

        protected override JToken TransformSingle(JToken root, TransformationContext ctx = null) =>
            _source.Transform(root, ctx)
                .ValidateNonNull()
                .ValidateType(JTokenType.Object)
                .As<JObject>()
                .Properties()
                .Aggregate(new JArray(), (array, property) =>
                {
                    array.Add(new JObject
                    {
                        [_config.Key] = property.Name,
                        [_config.Value] = property.Value
                    });

                    return array;
                });

        public class DeconstructTransformerConfig
        {
            public string Key { get; set; } = "key";
            public string Value { get; set; } = "value";
        }
    }
}