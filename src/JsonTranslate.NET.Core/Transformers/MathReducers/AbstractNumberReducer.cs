using System;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.MathReducers
{
    public abstract class AbstractNumberReducer : IJTokenTransformer
    {

        private IJTokenTransformer _source;

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));

            return this;
        }

        protected abstract JToken Aggregate(JArray source);

        public virtual JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var arr = _source.Transform(root, ctx);

            return Aggregate(arr as JArray);
        }

        public TR Accept<TR>(IVisitor<IJTokenTransformer, TR> visitor)
            => visitor.Visit(this);
    }
}