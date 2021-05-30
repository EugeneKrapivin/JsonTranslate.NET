using System;
using Newtonsoft.Json.Linq;
using TranformerDSLParser.Core;

namespace TranformerDSLParser.Transformers
{
    [Transformer(name: "valueof", requiresConfig: true)]
    public class ValueOfTransformer : IJTokenTransformer
    {
        public string SourceType => "any";

        public string TargetType => "any";

        private readonly Config _config;

        public ValueOfTransformer(JObject conf)
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
            return root.SelectToken(_config.Path);
        }

        private class Config
        {
            public string Path { get; set; }
        }
    }
}