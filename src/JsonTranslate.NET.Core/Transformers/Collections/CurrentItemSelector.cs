using System;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Exceptions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Collections
{
    [Transformer(name: "current", requiresConfig: true)]
    public class CurrentItemSelector : IJTokenTransformer
    {
        private IJTokenTransformer _source;

        public JToken Transform(JToken root, TransformationContext ctx = null)
        {
            if (ctx?.CurrentItem == null)
                throw new BadTransformerBindingException("transformer `#current` should only be bound inside a looping transformer (e.g. select)");

            var item = ctx.CurrentItem;

            return _source.Transform(item);
        }

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            if (_source != null) throw new NotSupportedException("can bind more than one");
            
            _source = source ?? throw new ArgumentNullException(nameof(source));

            return this;
        }

        public TR Accept<TR>(IVisitor<IJTokenTransformer, TR> visitor)
        {
            throw new NotImplementedException();
        }
    }
}