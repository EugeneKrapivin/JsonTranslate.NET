using System;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers
{
    [Transformer(name: "todecimal", requiresConfig: false)]
    public class ToDecimalTransformer : IJTokenTransformer
    {
        private IJTokenTransformer _source;
        public string SourceType => "any";

        public string TargetType => JTokenType.Boolean.ToString();

        public JToken Transform(JToken root)
        {
            var token = _source.Transform(root);

            return token switch
            {
                { Type: JTokenType.Boolean } x => x.Value<bool>() == true ? 1 : 0,
                { Type: JTokenType.String } x => Convert.ToInt32(x.Value<string>()),
                { Type: JTokenType.Integer } x => Convert.ToDecimal(x.Value<int>()),
                { Type: JTokenType.Float } x => x.Value<decimal>(),
                var x => throw new ArgumentOutOfRangeException(nameof(root), $"Can not handle type transformation from {x.Type} to {JTokenType.Float}")
            };
        }

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));

            return this;
        }
    }
}