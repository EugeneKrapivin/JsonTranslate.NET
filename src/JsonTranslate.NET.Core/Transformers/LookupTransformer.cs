using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers
{
    [Transformer(name: "lookup", requiresConfig: true)]
    public class LookupTransformer : IJTokenTransformer
    {
        static LookupTransformer()
        {
            TransformerFactory.RegisterTransformer<LookupTransformer>();
        }
        
        public string SourceType => "any";

        public string TargetType => "any";

        private readonly Config _config;

        public LookupTransformer(JObject conf)
        {
            if (conf == null) throw new ArgumentNullException($"{nameof(LookupTransformer)} requires configuration");

            _config = this.GetConfig<Config>(conf);
        }

        private IJTokenTransformer _source;

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            _source = source ?? throw new ArgumentNullException($"{nameof(LookupTransformer)} transformer expects exactly 1 input");

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
            public Dictionary<JToken, JToken> Lookup { get; set; }

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