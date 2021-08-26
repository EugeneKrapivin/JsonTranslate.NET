using System;
using System.Collections.Generic;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Aggregators
{
    [Transformer(name: "toarray", requiresConfig: false)]
    public class ArrayAggregator : MultiBoundTransformer
    {
        public override IEnumerable<JTokenType> SupportedTypes => JTokenTypeConstants.Any;
       
        public override IEnumerable<JTokenType> SupportedResults => JTokenTypeConstants.Any;

        public override JToken Transform(JToken root, TransformationContext ctx = null) =>
            _sources
                .Select(source => source.Transform(root, ctx))
                .Aggregate(new JArray(), (array, token) =>
                {
                    array.Add(token);
                    return array;
                });
    }
}