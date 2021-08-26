using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Math.Reducers
{
    [Transformer(name: "sum", requiresConfig: false)]
    public class SumReducer : AbstractNumberReducer
    {
        protected override JToken Aggregate(JArray source) 
            => source.All(x => x.Type == JTokenType.Integer)
                ? source.Select(x => x.Value<int>()).Sum()
                : source.Select(x => x.Value<decimal>()).Sum();
    }
}