using System;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.TypeConverters
{
    [Transformer(name: "tointeger", requiresConfig: false)]
    public class ToIntegerTransformer : IJTokenTransformer
    {
        private IJTokenTransformer _source;

        public JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var token = _source.Transform(root, ctx);

            return token switch
            {
                { Type: JTokenType.Boolean } x => x.Value<bool>() ? 1 : 0,
                { Type: JTokenType.String } x => Convert.ToInt32(x.Value<string>()),
                { Type: JTokenType.Integer } x => x,
                { Type: JTokenType.Float } x => Convert.ToInt32(x.Value<decimal>()),
                var x => throw new ArgumentOutOfRangeException(nameof(root), $"Can not handle type transformation from {x.Type} to {JTokenType.Integer}")
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