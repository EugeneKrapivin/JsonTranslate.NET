using System;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Exceptions;
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

        private readonly UnitTransformerConfig _unitTransformerConfig;

        public UnitTransformer(JObject conf)
        {
            if (conf == null) throw new TransformerConfigurationMissingException($"{nameof(ValueOfTransformer)} requires configuration");

            _unitTransformerConfig = this.GetConfig<UnitTransformerConfig>(conf);
            if (_unitTransformerConfig.Value == null)
                throw new TransformerConfigurationInvalidException("Configured value for `unit` transformer can not be null");
        }

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            throw new NotSupportedException();
        }

        public JToken Transform(JToken root, TransformationContext ctx = null)
        {
            return JToken.FromObject(_unitTransformerConfig.Value);
        }

        public TR Accept<TR>(IVisitor<IJTokenTransformer, TR> visitor)
            => visitor.Visit(this);

        public class UnitTransformerConfig
        {
            public object Value { get; set; }
        }
    }
}