using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Collections
{
    [Transformer("first")]
    public class FirstTransformer : CollectionTransformer
    {
        public override IEnumerable<JTokenType> SupportedResults => new[] {JTokenType.Integer};

        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var tok = _source.Transform(root, ctx);

            if (tok is not JArray array)
            {
                throw new IncompatibleTypeException(JTokenType.Array, tok.Type);
            }

            foreach (var item in array)
            {
                var innerCtx = new TransformationContext
                {
                    CurrentItem = item,
                    Root = root
                };

                var itemPredicate = _predicate.Transform(root, innerCtx);

                if (itemPredicate.Value<bool>())
                {
                    return item;
                }
            }

            throw new Exception("No items matched the predicate");
        }
    }
}