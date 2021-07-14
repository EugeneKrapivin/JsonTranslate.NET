using System;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.StringReducers
{
    [Transformer(name: "firstindexof", requiresConfig: false)]
    public class FirstIndexOfReducer : AbstractStringReducingTransformer
    {
        public override string TargetType => "number";

        public override JToken Transform(JToken root)
        {
            var sources = _sources.Select(x => Newtonsoft.Json.Linq.Extensions.Value<string>(x.Transform(root))).ToArray();

            if (sources.Length < 2)
                throw new ArgumentException("Not enough inputs passed into `firstindexof` transformer, expecting 2");

            // todo: typecheck?
            var source = sources[0];
            var target = sources[1];

            return source.IndexOf(target, StringComparison.InvariantCulture);
        }
    }
}