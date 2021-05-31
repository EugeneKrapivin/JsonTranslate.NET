using System;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers
{
    [Transformer(name: "valueof", requiresConfig: true)]
    public class ValueOfTransformer : IJTokenTransformer
    {
        static ValueOfTransformer()
        {
            TransformerFactory.RegisterTransformer<ValueOfTransformer>();
        }

        public string SourceType => "any";

        public string TargetType => "any";

        private readonly Config _config;

        public ValueOfTransformer(JObject conf)
        {
            if (conf == null) throw new ArgumentNullException($"{nameof(ValueOfTransformer)} requires configuration");

            _config = this.GetConfig<Config>(conf);
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