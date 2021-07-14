using System;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers
{
    [Transformer(name: "unit", requiresConfig: true)]
    public class UnitTransformer : IJTokenTransformer
    {
        static UnitTransformer()
        {
            TransformerFactory.RegisterTransformer<UnitTransformer>();
        }

        public string SourceType => "any";

        public string TargetType => "any";

        private readonly Config _config;

        public UnitTransformer(JObject conf)
        {
            if (conf == null) throw new ArgumentNullException($"{nameof(ValueOfTransformer)} requires configuration");

            _config = this.GetConfig<Config>(conf);
            if (_config.Value == null)
                throw new ArgumentException("Configured value for `unit` transformer can not be null");
        }

        public IJTokenTransformer Bind(IJTokenTransformer source)
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