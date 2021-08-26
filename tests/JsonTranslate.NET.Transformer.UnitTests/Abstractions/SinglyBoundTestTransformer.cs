using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Transformer.UnitTests.Abstractions
{
    internal class SinglyBoundTestTransformer : SinglyBoundTransformer
    {
        public override IEnumerable<JTokenType> SupportedTypes => new[] { JTokenType.None };
        public override IEnumerable<JTokenType> SupportedResults => SupportedTypes;

        protected override JToken TransformSingle(JToken root, TransformationContext ctx = null)
        {
            throw new NotImplementedException();
        }
    }
}