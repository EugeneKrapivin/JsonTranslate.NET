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

        private readonly ValueOfTransformerConfig _valueOfTransformerConfig;

        public ValueOfTransformer(JObject conf)
        {
            if (conf == null) throw new ArgumentNullException($"{nameof(ValueOfTransformer)} requires configuration");

            _valueOfTransformerConfig = this.GetConfig<ValueOfTransformerConfig>(conf);
        }

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            throw new NotSupportedException();
        }

        public JToken Transform(JToken root, TransformationContext ctx = null)
        {
            return root.SelectToken(_valueOfTransformerConfig.Path);
        }

        public TR Accept<TR>(IVisitor<IJTokenTransformer, TR> visitor)
            => visitor.Visit(this);

        private class ValueOfTransformerConfig
        {
            public string Path { get; set; }
        }
    }
}