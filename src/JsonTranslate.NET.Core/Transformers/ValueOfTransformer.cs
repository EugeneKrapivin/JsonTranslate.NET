using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers
{
    [Transformer(name: "valueof", requiresConfig: true)]
    public class ValueOfTransformer : ValueProvidingTransformer
    {
        private readonly ValueOfTransformerConfig _valueOfTransformerConfig;

        public ValueOfTransformer(JObject conf)
        {
            _valueOfTransformerConfig = conf == null 
                    ? throw new ArgumentNullException($"{nameof(ValueOfTransformer)} requires configuration")
                    : this.GetConfig<ValueOfTransformerConfig>(conf);
        }

        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            return root.SelectToken(_valueOfTransformerConfig.Path);
        }

        public class ValueOfTransformerConfig
        {
            public string Path { get; set; }
        }
    }
}