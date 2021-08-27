using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;

using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.TypeConverters
{
    [Transformer(name: "toboolean", requiresConfig: false)]
    public sealed class ToBooleanTransformer : SinglyBoundTransformer
    {
        protected override JToken TransformSingle(JToken root, TransformationContext ctx = null) => 
            _source.Transform(root, ctx) switch
            {
                { Type: JTokenType.Boolean } x => x,
                { Type: JTokenType.String } x => Convert.ToBoolean(x.Value<string>()),
                { Type: JTokenType.Integer } x => Convert.ToBoolean(x.Value<int>()),
                { Type: JTokenType.Float } x => Convert.ToBoolean(x.Value<decimal>()),
                var x => throw new ArgumentOutOfRangeException(nameof(root), $"Can not handle type transformation from {x.Type} to {JTokenType.Boolean}")
            };

        public override IEnumerable<JTokenType> InputTypes =>
            new[] { JTokenType.Boolean, JTokenType.String, JTokenType.Integer, JTokenType.Float };

        public override IEnumerable<JTokenType> OutputTypes => new[] { JTokenType.Boolean };
    }
}