using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Transformer.UnitTests.Abstractions
{

    internal class MultiBoundTestTransformer : MultiBoundTransformer
    {
        public override IReadOnlyCollection<JTokenType> SupportedTypes => new[] { JTokenType.None };
        
        public override IReadOnlyCollection<JTokenType> SupportedResults => JTokenTypeConstants.Any;

        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            throw new NotImplementedException();
        }
    }
}
