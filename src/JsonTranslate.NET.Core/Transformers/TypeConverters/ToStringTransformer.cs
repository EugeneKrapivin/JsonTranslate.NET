using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.TypeConverters
{
    [Transformer(name: "tostring", requiresConfig: false)]
    public class ToStringTransformer : SinglyBoundTransformer
    {
        protected override JToken TransformSingle(JToken root, TransformationContext ctx = null) =>
            _source.Transform(root, ctx) switch
            {
                { Type: JTokenType.String } token => token,
                { Type: JTokenType.Boolean } token => token.Value<bool>() ? "true" : "false",
                {
                    Type: JTokenType.Float
                    or JTokenType.Integer
                    or JTokenType.Guid
                    or JTokenType.Uri
                    or JTokenType.TimeSpan
                    or JTokenType.Date
                } token
                => token.ToString(),
                var x => throw new ArgumentOutOfRangeException(nameof(root), $"Can not handle type transformation from {x.Type} to {JTokenType.String}")
            };

        public override IEnumerable<JTokenType> SupportedTypes => JTokenTypeConstants.Primitive;
        public override IEnumerable<JTokenType> SupportedResults => JTokenTypeConstants.String;
    }
}