using System;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Collections
{
    [Transformer("select")]
    public class SelectTransformation : CollectionTransformer
    {
        private IJTokenTransformer _projection;

        public override IJTokenTransformer Bind(IJTokenTransformer source)
        {
            EnsureSource(source);

            if (_source is null)
            {
                _source = source;
            }
            else if (_predicate is null)
            {
                _projection = source;
            }
            else
            {
                throw new BadTransformerBindingException("Transformer expects exactly 2 bindings source and projection");
            }
            
            _sources.Add(source);

            EnsureNoCycles();

            return this;
        }

        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var arrResult = _source.Transform(root); // select the array from the object root

            if (arrResult == null) return arrResult; // TODO

            var arr = arrResult; 
            if (arr.Type != JTokenType.Array)
            {
                throw new Exception(); // TODO
            }

            var obj = arr.Select(item => item.DeepClone())
                .Select(item => _projection.Transform(root, new TransformationContext { Root = root, CurrentItem = item}))
                .Aggregate(new JArray(), (o, token) =>
                {
                    o.Add(token);
                    return o;
                });

            return obj;
        }
    }
}