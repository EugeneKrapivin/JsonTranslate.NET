using System;
using System.Collections.Generic;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers
{
    [Transformer(name: "lookup", requiresConfig: true)]
    public class LookupTransformer : SinglyBoundTransformer
    {
        private readonly LookupConfig _lookupConfig;

        public override IEnumerable<JTokenType> InputTypes => JTokenTypeConstants.Any;
        
        public override IEnumerable<JTokenType> OutputTypes
            => JTokenTypeConstants.Any;

        public LookupTransformer(JObject conf)
        {
            if (conf == null) throw new ArgumentNullException($"{nameof(LookupTransformer)} requires configuration");

            _lookupConfig = this.GetConfig<LookupConfig>(conf);
        }

        protected override JToken TransformSingle(JToken root, TransformationContext ctx = null)
        {
            var value = _source.Transform(root, ctx);

            if (!TryFind(_lookupConfig.Lookup, value, out var lookUpResult))
            {
                if (_lookupConfig.OnMissing == LookupConfig.HandleMissing.Value)
                {
                    return value;
                }
                else
                {
                    return _lookupConfig.Default;
                }
            }

            return lookUpResult;

            bool TryFind(List<KeyValuePair<JToken, JToken>> lookupTable, JToken key, out JToken value)
            {
                value = lookupTable.SingleOrDefault(x => JToken.DeepEquals(x.Key, key)).Value;

                return value != null;
            }
        }

        public class LookupConfig
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