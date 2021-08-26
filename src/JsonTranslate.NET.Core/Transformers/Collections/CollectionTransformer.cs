using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Collections
{
    public abstract class CollectionTransformer : TransformerBase
    {
        protected IJTokenTransformer _predicate;
        protected IJTokenTransformer _source;
        
        protected readonly List<IJTokenTransformer> _sources = new();

        public override IEnumerable<IJTokenTransformer> Sources => _sources;

        public override IEnumerable<JTokenType> SupportedTypes => JTokenTypeConstants.Array;
        
        public override IEnumerable<JTokenType> SupportedResults => JTokenTypeConstants.Array;

        public override IJTokenTransformer Bind(IJTokenTransformer source)
        {
            EnsureSource(source);

            if (_source is null)
            {
                _source = source;
            }
            else if (_predicate is null)
            {
                _predicate = source;
            }
            else
            {
                throw new BadTransformerBindingException("Transformer expects exactly 2 bindings source and predicate");
            }

            _sources.Add(source);

            EnsureNoCycles();

            return this;
        }
    }
}