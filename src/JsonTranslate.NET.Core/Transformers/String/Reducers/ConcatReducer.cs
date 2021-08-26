using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.String.Reducers
{
    [Transformer(name: "strcat", requiresConfig: false)]
    public class ConcatReducer : AbstractStringReducingTransformer
    {
        protected override JToken Reduce(IEnumerable<string> sources, TransformationContext ctx = null)
        {
            return string.Concat(sources);
        }
    }
}