using System;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.TypeConverters
{
    [Transformer(name: "todecimal", requiresConfig: false)]
    public class ToDecimalTransformer : IJTokenTransformer
    {
        private IJTokenTransformer _source;

        public JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var token = _source.Transform(root, ctx);

            return token switch
            {
                { Type: JTokenType.Boolean } x => x.Value<bool>() == true ? 1 : 0,
                { Type: JTokenType.String } x => Convert.ToInt32(x.Value<string>()),
                { Type: JTokenType.Integer } x => Convert.ToDecimal(x.Value<int>()),
                { Type: JTokenType.Float } x => x.Value<decimal>(),
                var x => throw new ArgumentOutOfRangeException(nameof(root), $"Can not handle type transformation from {x.Type} to {JTokenType.Float}")
            };
        }

        public TR Accept<TR>(IVisitor<IJTokenTransformer, TR> visitor)
            => visitor.Visit(this);

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));

            return this;
        }
    }
}