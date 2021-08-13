using System;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.StringReducers
{
    [Transformer(name: "substring", requiresConfig: false)]
    public class SubstringReducer : AbstractStringReducingTransformer
    {
        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            
            if (_sources.Count() < 2)
                throw new ArgumentException("Not enough inputs passed into `substring` transformer, expecting at least 2 at most 3");

            // todo: typecheck?
            var source = _sources[0].Transform(root, ctx).Value<string>();

            if (source == null)
                throw new ArgumentNullException(nameof(source), "Failed to resolve a string from first binding in `substring`");

            var start = _sources[1].Transform(root, ctx).Value<int>();
            var count = default(int?);

            if (_sources.Count == 3)
            {
                count = _sources[2].Transform(root, ctx).Value<int>();
            }

            return count.HasValue 
                ? source.Substring(start, count.Value) 
                : source.Substring(start);
        }
    }
}