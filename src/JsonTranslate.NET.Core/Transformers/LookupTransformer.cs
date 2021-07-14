using System;
using System.Collections.Generic;
using System.Linq;
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
            if (_source != null) throw new NotSupportedException($"{nameof(LookupTransformer)} transformer doesn't allow multiple bindings");

            _source = source ?? throw new ArgumentNullException($"{nameof(LookupTransformer)} transformer expects exactly 1 input");

            return this;
        }

        public JToken Transform(JToken root)
        {
            var value = _source.Transform(root);

            if (!TryFind(_config.Lookup, value, out var lookUpResult))
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

            bool TryFind(List<KeyValuePair<JToken, JToken>> lookupTable, JToken key, out JToken value)
            {
                value = lookupTable.SingleOrDefault(x => JToken.DeepEquals(x.Key, key)).Value;

                return value != null;
            }
        }

        internal class Config
        {
            public List<KeyValuePair<JToken, JToken>> Lookup { get; set; } = new();

            public HandleMissing OnMissing { get; set; } = HandleMissing.Value;

            public JToken Default { get; set; }

            public enum HandleMissing
            {
                Value,
                Default,
            }
        }
    }
}