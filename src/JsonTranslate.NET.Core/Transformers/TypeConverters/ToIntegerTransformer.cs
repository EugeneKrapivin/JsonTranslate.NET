using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;

using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.TypeConverters
{
    [Transformer(name: "tointeger", requiresConfig: false)]
    public class ToIntegerTransformer : SinglyBoundTransformer
    {
        protected override JToken TransformSingle(JToken root, TransformationContext ctx = null) => 
            _source.Transform(root, ctx) switch
            {
                { Type: JTokenType.Boolean } x => x.Value<bool>() ? 1 : 0,
                { Type: JTokenType.String } x => Convert.ToInt32(x.Value<string>()),
                { Type: JTokenType.Integer } x => x,
                { Type: JTokenType.Float } x => Convert.ToInt32(x.Value<decimal>()),
                var x => throw new ArgumentOutOfRangeException(nameof(root), $"Can not handle type transformation from {x.Type} to {JTokenType.Integer}")
            };

        public override IEnumerable<JTokenType> InputTypes =>
            new[] {JTokenType.Boolean, JTokenType.String, JTokenType.Integer, JTokenType.Float};

        public override IEnumerable<JTokenType> OutputTypes => new[] {JTokenType.Integer};
    }
}