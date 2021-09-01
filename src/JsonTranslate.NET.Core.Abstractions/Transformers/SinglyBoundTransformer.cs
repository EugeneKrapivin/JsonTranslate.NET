using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Abstractions.Transformers
{
    public abstract class SinglyBoundTransformer : TransformerBase
    {
        protected IJTokenTransformer _source;
        
        private readonly List<IJTokenTransformer> _sources = new(1); // ugly hack :(

        public override IEnumerable<IJTokenTransformer> Sources => _sources;

        public override IJTokenTransformer Bind(IJTokenTransformer source)
        {
            if (_source != null)
            {
                throw new CanHaveOnlyOneBindingException(GetType().Name);
            }

            EnsureSource(source);

            _source = source;

            _sources.Add(_source);

            EnsureNoCycles();

            return this;
        }

        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            EnsureSource(_source);

            return TransformSingle(root, ctx);
        }

        protected abstract JToken TransformSingle(JToken root, TransformationContext ctx = null);
    }
}