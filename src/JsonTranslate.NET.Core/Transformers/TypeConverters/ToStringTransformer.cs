using System;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.TypeConverters
{
    [Transformer(name: "tostring", requiresConfig: false)]
    public class ToStringTransformer : IJTokenTransformer
    {
        private IJTokenTransformer _source;
        public string SourceType => "any";

        public string TargetType => JTokenType.Boolean.ToString();

        public JToken Transform(JToken root)
        {
            var token = _source.Transform(root);

            return token.ToString();
        }

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));

            return this;
        }
    }
}