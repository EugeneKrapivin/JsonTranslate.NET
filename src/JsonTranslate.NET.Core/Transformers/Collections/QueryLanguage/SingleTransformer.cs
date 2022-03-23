using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Collections
{
    [Transformer("single")]
    public class SingleTransformer : AbstractCollectionTransformer
    {
        public override IEnumerable<JTokenType> OutputTypes => JTokenTypeConstants.Any;

        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var tok = _source.Transform(root, ctx);

            if (tok is not JArray array)
            {
                throw new IncompatibleTypeException(JTokenType.Array, tok.Type);
            }

            var result = new JArray();
            foreach (var item in array)
            {
                var innerCtx = new TransformationContext
                {
                    CurrentItem = item,
                    Root = root
                };

                if (_predicate != null)
                {
                    var predicate = _predicate.Transform(root, innerCtx);

                    if (predicate.Value<bool>())
                    {
                        result.Add(item);
                    }
                }
                else
                {
                    result.Add(item);
                }
            }

            return result.Count switch
            {
                > 1 => throw new Exception("Predicate matched more than 1 item in the collection"),
                0 => throw new Exception("Predicate failed to match any item in the collection", null),
                _ => result[0]
            };
        }
    }
}