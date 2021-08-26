using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions.Exceptions;

namespace JsonTranslate.NET.Core.Abstractions.Transformers
{
    public abstract class MultiBoundTransformer : TransformerBase
    {
        protected readonly List<IJTokenTransformer> _sources = new();

        public override IEnumerable<IJTokenTransformer> Sources => _sources;

        public override IJTokenTransformer Bind(IJTokenTransformer source)
        {
            EnsureSource(source);

            _sources.Add(source ?? throw new CannotBindToNullException());

            EnsureNoCycles();

            return this;
        }
    }
}