using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Collections
{
    [Transformer("where")]
    public class WhereTransformer : CollectionTransformer
    {
        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var result = _source.Transform(root, ctx)
                .ValidateNonNull()
                .ValidateType(JTokenType.Array)
                .As<JArray>()
                .Where(item =>
                {
                    var innerCtx = new TransformationContext
                    {
                        CurrentItem = item,
                        Root = root
                    };

                    var predicate = _predicate.Transform(root, innerCtx);

                    return predicate.Value<bool>();
                })
                .Aggregate(new JArray(), (array, token) =>
                {
                    array.Add(token);
                    return array;
                });

            if (result.Any())
            {
                var type = result.First().Type;

                if (result.Any(r => r.Type != type))
                {
                    throw new IncompatibleTypeException(type, result.First(r => r.Type != type).Type);
                }
            }

            return result;
        }
    }
}