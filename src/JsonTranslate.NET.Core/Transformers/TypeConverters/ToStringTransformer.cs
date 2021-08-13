using System;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.TypeConverters
{
    [Transformer(name: "tostring", requiresConfig: false)]
    public class ToStringTransformer : IJTokenTransformer
    {
        private IJTokenTransformer _source;

        public JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var token = _source.Transform(root, ctx);

            return token.ToString();
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