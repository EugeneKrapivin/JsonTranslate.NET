using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Aggregators
{
    [Transformer(name: "max", requiresConfig: false)]
    public class MaxAggregatingTransformer : NumberAggregatingTransformer
    {
        protected override JToken Aggregate(JArray source)
            => source.All(x => x.Type == JTokenType.Integer)
                ? source.Select(x => x.Value<int>()).Max()
                : source.Select(x => x.Value<decimal>()).Max();
    }
}