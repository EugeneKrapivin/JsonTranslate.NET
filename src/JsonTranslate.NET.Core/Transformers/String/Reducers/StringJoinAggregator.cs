using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.String.Reducers
{
    [Transformer(name: "str_join", requiresConfig: true)]
    public class StringJoinAggregator : AbstractStringReducingTransformer
    {
        private readonly StringJoinAggregatorConfig _config;

        public StringJoinAggregator(JObject conf)
        {
            if (conf == null) throw new ArgumentNullException($"{nameof(ValueOfTransformer)} requires configuration");

            _config = this.GetConfig<StringJoinAggregatorConfig>(conf);
        }

        protected override JToken Reduce(IEnumerable<string> values, TransformationContext ctx = null) => string.Join(_config.Separator, values);

        public class StringJoinAggregatorConfig
        {
            public string Separator { get; set; }
        }
    }
}