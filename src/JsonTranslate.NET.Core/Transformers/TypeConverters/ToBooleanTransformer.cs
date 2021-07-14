using System;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.TypeConverters
{
    [Transformer(name: "toboolean", requiresConfig: false)]
    public class ToBooleanTransformer : IJTokenTransformer
    {
        private IJTokenTransformer _source;
        public string SourceType => "any";
        
        public string TargetType => JTokenType.Boolean.ToString();

        public JToken Transform(JToken root)
        {
            var token = _source.Transform(root);
            
            return token switch
            {
                { Type: JTokenType.Boolean } x => x,
                { Type: JTokenType.String } x => Convert.ToBoolean(x.Value<string>()),
                { Type: JTokenType.Integer } x => Convert.ToBoolean(x.Value<int>()),
                { Type: JTokenType.Float } x => Convert.ToBoolean(x.Value<decimal>()),
                var x => throw new ArgumentOutOfRangeException(nameof(root), $"Can not handle type transformation from {x.Type} to {JTokenType.Boolean}")
            };

        }

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));

            return this;
        }
    }
}