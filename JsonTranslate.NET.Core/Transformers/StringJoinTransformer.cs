using System;
using System.Collections.Generic;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers
{
    [Transformer(name: "str_join", requiresConfig: true)]
    public class StringJoinTransformer : IJTokenTransformer
    {
        static StringJoinTransformer()
        {
            TransformerFactory.RegisterTransformer<StringToStringLookupTransformer>();
        }

        public string SourceType => "string";

        public string TargetType => "string";

        private readonly Config _config;

        private readonly List<IJTokenTransformer> _sources = new();

        public StringJoinTransformer(JObject conf)
        {
            if (conf == null) throw new ArgumentNullException($"{nameof(ValueOfTransformer)} requires configuration");

            _config = this.GetConfig<Config>(conf);
        }

        public IJTokenTransformer Bind(params IJTokenTransformer[] sources)
        {
            // todo: validate all sources have target types string
            _sources.AddRange(sources);

            return this;
        }

        public JToken Transform(JToken root)
        {
            // TODO: validate at least 1 source
            var values = _sources.Select(x => Extensions.Value<string>(x.Transform(root)));

            return string.Join(_config.Separator, values);
        }

        private class Config
        {
            public string Separator { get; set; }
        }
    }

    [Transformer(name: "sum", requiresConfig: false)]
    public class ArrSumTransformer : IJTokenTransformer
    {
        static ArrSumTransformer()
        {
            TransformerFactory.RegisterTransformer<ArrSumTransformer>();
        }

        public string SourceType => "number";

        public string TargetType => "number";

        private readonly List<IJTokenTransformer> _sources = new();

        public ArrSumTransformer()
        {
        }

        public IJTokenTransformer Bind(params IJTokenTransformer[] sources)
        {
            // todo: validate all sources have target types string
            _sources.AddRange(sources);

            return this;
        }

        public JToken Transform(JToken root)
        {
            var arr = _sources.Single().Transform(root);
            
            // find best suiting type => get value
            if (arr.All(x => x.Type == JTokenType.Integer))
                return (arr as JArray).Select(x => x.Value<int>()).Sum();

            return (arr as JArray).Select(x => x.Value<decimal>()).Sum();
        }
    }
}