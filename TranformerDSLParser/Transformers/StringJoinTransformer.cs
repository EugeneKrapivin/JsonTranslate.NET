using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using TranformerDSLParser.Core;

namespace TranformerDSLParser.Transformers
{
    [Transformer(name: "str_join", requiresConfig: true)]
    public class StringJoinTransformer : IJTokenTransformer
    {
        public string SourceType => "string";

        public string TargetType => "string";

        private readonly Config _config;

        private readonly List<IJTokenTransformer> _sources = new();

        public StringJoinTransformer(JObject conf)
        {
            if (conf == null) throw new ArgumentNullException($"{nameof(ValueOfTransformer)} requires configuration");

            _config = IJTokenTransformer.GetConfig<Config>(conf);
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

            return string.Join(_config.Seperator, values);
        }

        public class Config
        {
            public string Seperator { get; set; }
        }
    }
}