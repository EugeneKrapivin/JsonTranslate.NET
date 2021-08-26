using System.Collections.Generic;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.String.Reducers
{
    public abstract class AbstractStringReducingTransformer : MultiBoundTransformer
    {
        public override IEnumerable<JTokenType> SupportedTypes => JTokenTypeConstants.String;
        public override IEnumerable<JTokenType> SupportedResults => JTokenTypeConstants.String;

        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var results = _sources.Select(source => source.Transform(root, ctx)).ToList();

            var nulls = results.Where(x => x is null);
            if (nulls.Any())
            {
                throw new TransformationResultCannotBeNullException(); // TODO pass in the null generating transformers
            }

            var nonStr = results.Where(x => x.Type != JTokenType.String);
            if (nonStr.Any())
            {
                // todo return all types
                throw new IncompatibleTypeException(JTokenType.String, nonStr.First().Type);
            }

            var sources = results.Select(x => x.Value<string>()).ToList();

            return Reduce(sources, ctx);
        }

        protected abstract JToken Reduce(IEnumerable<string> sources, TransformationContext ctx = null);
    }
}
