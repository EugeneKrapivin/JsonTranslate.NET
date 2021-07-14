using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.StringReducers
{
    [Transformer(name: "strcat", requiresConfig: false)]
    public class ConcatReducer : AbstractStringReducingTransformer
    {
        public override JToken Transform(JToken root)
        {
            return string.Concat(_sources.Select(x => Newtonsoft.Json.Linq.Extensions.Value<string>(x.Transform(root))));
        }
    }
}