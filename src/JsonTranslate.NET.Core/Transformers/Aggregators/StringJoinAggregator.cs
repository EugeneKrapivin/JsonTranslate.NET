using System;
using System.Collections.Generic;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Aggregators
{
    [Transformer(name: "str_join", requiresConfig: true)]
    public class StringJoinAggregator : IJTokenTransformer
    {
        static StringJoinAggregator()
        {
            TransformerFactory.RegisterTransformer<LookupTransformer>();
        }

        public string SourceType => "string";

        public string TargetType => "string";

        private readonly Config _config;

        private readonly List<IJTokenTransformer> _sources = new();

        public StringJoinAggregator(JObject conf)
        {
            if (conf == null) throw new ArgumentNullException($"{nameof(ValueOfTransformer)} requires configuration");

            _config = this.GetConfig<Config>(conf);
        }

        public IJTokenTransformer Bind(IJTokenTransformer sources)
        {
            // todo: validate all sources have target types string
            _sources.Add(sources);

            return this;
        }

        public JToken Transform(JToken root)
        {
            // TODO: validate at least 1 source
            var values = _sources.Select(x => x.Transform(root).Value<string>());

            return string.Join(_config.Separator, values);
        }

        private class Config
        {
            public string Separator { get; set; }
        }
    }
}