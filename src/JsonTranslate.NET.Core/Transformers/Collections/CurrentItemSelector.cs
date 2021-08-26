using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Collections
{
    [Transformer(name: "current", requiresConfig: true)]
    public class CurrentItemSelector : SinglyBoundTransformer
    {
        public override IEnumerable<JTokenType> SupportedTypes => JTokenTypeConstants.Any;
        
        public override IEnumerable<JTokenType> SupportedResults => JTokenTypeConstants.Any;

        protected override JToken TransformSingle(JToken root, TransformationContext ctx = null)
        {
            if (ctx?.CurrentItem == null)
                throw new BadTransformerBindingException("transformer `#current` should only be bound inside a looping transformer (e.g. select)");

            var item = ctx.CurrentItem;

            return _source.Transform(item);
        }
    }
}