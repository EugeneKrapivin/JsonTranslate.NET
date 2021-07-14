using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.MathReducers
{
    [Transformer(name: "average", requiresConfig: false)]
    public class AverageReducer : AbstractNumberReducer
    {
        protected override JToken Aggregate(JArray source) 
            => source.All(x => x.Type == JTokenType.Integer)
                ? source.Select(x => x.Value<int>()).Average()
                : source.Select(x => x.Value<decimal>()).Average();
    }
}