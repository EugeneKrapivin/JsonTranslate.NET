using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.StringReducers
{
    [Transformer(name: "strcat", requiresConfig: false)]
    public class ConcatReducer : AbstractStringReducingTransformer
    {
        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            return string.Concat(
                _sources.Select(x => x.Transform(root, ctx).Value<string>()));
        }
    }
}