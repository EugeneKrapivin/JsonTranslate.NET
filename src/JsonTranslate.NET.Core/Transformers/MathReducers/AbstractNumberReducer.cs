using System;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.MathReducers
{
    public abstract class AbstractNumberReducer : IJTokenTransformer
    {
        public string SourceType => "number";

        public string TargetType => "number";

        private IJTokenTransformer _source;

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));

            return this;
        }

        protected abstract JToken Aggregate(JArray source);

        public virtual JToken Transform(JToken root)
        {
            var arr = _source.Transform(root);

            return Aggregate(arr as JArray);
        }
    }
}