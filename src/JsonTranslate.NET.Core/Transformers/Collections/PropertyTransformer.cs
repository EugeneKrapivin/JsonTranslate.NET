using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Collections
{
    [Transformer("property")]
    public class PropertyTransformer : TransformerBase
    {
        private IJTokenTransformer _keySelector;

        private IJTokenTransformer _valueSelector;

        private readonly List<IJTokenTransformer> _sources = new(2);

        public override IEnumerable<JTokenType> InputTypes => JTokenTypeConstants.Any;
        public override IEnumerable<JTokenType> OutputTypes => new[] {JTokenType.Property};
        public override IEnumerable<IJTokenTransformer> Sources => _sources;

        public override IJTokenTransformer Bind(IJTokenTransformer source)
        {
            EnsureSource(source);

            if (_keySelector == null)
            {
                _keySelector = source;
            }
            else if (_valueSelector == null)
            {
                _valueSelector = source;
            }
            else
            {
                throw new TransformerBindingException(); // TODO message
            }

            _sources.Add(source);
            
            EnsureNoCycles();

            return this;
        }

        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var key = _keySelector
                .Transform(root, ctx)
                .ValidateNonNull()
                .ValidateType(JTokenType.String)
                .Value<string>();
            
            var value = _valueSelector.Transform(root, ctx);

            return new JProperty(key, value);
        }
    }
}