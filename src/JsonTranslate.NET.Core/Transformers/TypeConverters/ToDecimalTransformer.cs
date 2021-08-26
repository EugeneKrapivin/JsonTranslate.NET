using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;

using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.TypeConverters
{
    [Transformer(name: "todecimal", requiresConfig: false)]
    public class ToDecimalTransformer : SinglyBoundTransformer
    {
        protected override JToken TransformSingle(JToken root, TransformationContext ctx = null) => 
            _source.Transform(root, ctx) switch
            {
                { Type: JTokenType.Boolean } x => x.Value<bool>() == true ? 1 : 0,
                { Type: JTokenType.String } x => Convert.ToDecimal(x.Value<string>()),
                { Type: JTokenType.Integer } x => Convert.ToDecimal(x.Value<int>()),
                { Type: JTokenType.Float } x => x.Value<decimal>(),
                var x => throw new ArgumentOutOfRangeException(nameof(root), $"Can not handle type transformation from {x.Type} to {JTokenType.Float}")
            };

        public override IEnumerable<JTokenType> SupportedTypes =>
            new[] { JTokenType.Boolean, JTokenType.String, JTokenType.Integer, JTokenType.Float };

        public override IEnumerable<JTokenType> SupportedResults => new[] { JTokenType.Float };
    }
}