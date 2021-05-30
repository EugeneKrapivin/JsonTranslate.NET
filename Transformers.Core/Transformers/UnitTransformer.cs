using System;
using Newtonsoft.Json.Linq;
using TranformerDSLParser.Core;

namespace TranformerDSLParser.Transformers
{
    [Transformer(name: "unit", requiresConfig: true)]
    public class UnitTransformer : IJTokenTransformer
    {
        public string SourceType => "any";

        public string TargetType => "any";

        private readonly Config _config;

        public UnitTransformer(JObject conf)
        {
            if (conf == null) throw new ArgumentNullException($"{nameof(ValueOfTransformer)} requires configuration");

            _config = IJTokenTransformer.GetConfig<Config>(conf);
        }

        public IJTokenTransformer Bind(params IJTokenTransformer[] source)
        {
            throw new NotSupportedException();
        }

        public JToken Transform(JToken root)
        {
            return JToken.FromObject(_config.Value);
        }

        private class Config
        {
            public object Value { get; set; }
        }
    }
}