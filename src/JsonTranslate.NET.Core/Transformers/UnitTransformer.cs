using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers
{
    [Transformer(name: "unit", requiresConfig: true)]
    public sealed class UnitTransformer : ValueProvidingTransformer
    {
        private readonly UnitTransformerConfig _unitTransformerConfig;

        public UnitTransformer(JObject conf)
        {
            _unitTransformerConfig = conf == null 
                ? throw new TransformerConfigurationMissingException($"{nameof(ValueOfTransformer)} requires configuration")
                : this.GetConfig<UnitTransformerConfig>(conf);
            
            if (_unitTransformerConfig.Value == null)
                throw new TransformerConfigurationInvalidException("Configured value for `unit` transformer can not be null");
        }

        public override JToken Transform(JToken root, TransformationContext ctx = null) 
            => JToken.FromObject(_unitTransformerConfig.Value);

        public class UnitTransformerConfig
        {
            public object Value { get; set; }
        }
    }
}