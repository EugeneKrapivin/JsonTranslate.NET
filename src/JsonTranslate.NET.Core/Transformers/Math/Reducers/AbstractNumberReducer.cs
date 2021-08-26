using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Math.Reducers
{
    public abstract class AbstractNumberReducer : SinglyBoundTransformer
    {
        protected abstract JToken Aggregate(JArray source);

        public override IEnumerable<JTokenType> SupportedTypes => JTokenTypeConstants.Array;
        
        public override IEnumerable<JTokenType> SupportedResults => JTokenTypeConstants.Numeric;

        protected override JToken TransformSingle(JToken root, TransformationContext ctx = null)
        {
            var arr = _source.Transform(root, ctx);

            return Aggregate(arr as JArray);
        }
    }
}