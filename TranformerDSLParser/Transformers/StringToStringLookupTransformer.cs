using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using TranformerDSLParser.Core;

namespace TranformerDSLParser.Transformers
{
    [Transformer(name: "s_lookup_s", requiresConfig: true)]
    public class StringToStringLookupTransformer : IJTokenTransformer
    {
        public string SourceType => "string";

        public string TargetType => "string";

        private readonly Config _config;

        public StringToStringLookupTransformer(JObject conf)
        {
            if (conf == null) throw new ArgumentNullException($"{nameof(ValueOfTransformer)} requires configuration");

            _config = IJTokenTransformer.GetConfig<Config>(conf);
        }

        private IJTokenTransformer _source;

        public IJTokenTransformer Bind(params IJTokenTransformer[] source)
        {
            if (source?.Length != 1) throw new ArgumentException($"{nameof(StringToStringLookupTransformer)} transformer expects exactly 1 input");

            _source = source.Single();

            return this;
        }

        public JToken Transform(JToken root)
        {
            var value = _source.Transform(root).Value<string>();

            if (!_config.Lookup.TryGetValue(value, out var lookUpResult))
            {
                if (_config.OnMissing == Config.HandleMissing.Value)
                {
                    return value;
                }
                else
                {
                    return _config.Default;
                }
            }

            return lookUpResult;
        }

        private class Config
        {
            public Dictionary<string, string> Lookup { get; set; }

            public HandleMissing OnMissing { get; set; }

            public string Default { get; set; }

            public enum HandleMissing
            {
                Value,
                Default,
            }
        }
    }
}