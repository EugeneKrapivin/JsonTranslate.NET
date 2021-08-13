using System;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Exceptions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.TypeConverters
{
    [Transformer(name: "toboolean", requiresConfig: false)]
    public class ToBooleanTransformer : IJTokenTransformer
    {
        private IJTokenTransformer _source;

        public JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var token = _source.Transform(root, ctx);
            
            return token switch
            {
                { Type: JTokenType.Boolean } x => x,
                { Type: JTokenType.String } x => Convert.ToBoolean(x.Value<string>()),
                { Type: JTokenType.Integer } x => Convert.ToBoolean(x.Value<int>()),
                { Type: JTokenType.Float } x => Convert.ToBoolean(x.Value<decimal>()),
                var x => throw new ArgumentOutOfRangeException(nameof(root), $"Can not handle type transformation from {x.Type} to {JTokenType.Boolean}")
            };

        }

        public TR Accept<TR>(IVisitor<IJTokenTransformer, TR> visitor)
            => visitor.Visit(this);

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            if (ReferenceEquals(this, source)) throw new TransformerBindingException("Cannot bind to self");
            
            _source = source ?? throw new TransformerBindingException("Cannot bind `null` as transformer"); // TODO

            return this;
        }
    }
}