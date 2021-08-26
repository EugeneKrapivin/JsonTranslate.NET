using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.String.Operators
{
    [Transformer(name: "substring", requiresConfig: false)]
    public class SubstringReducer : TransformerBase
    {
        private IJTokenTransformer _countValue;
        private IJTokenTransformer _fromValue;
        private IJTokenTransformer _source;
        private List<IJTokenTransformer> _sources = new(3);

        public override IEnumerable<JTokenType> SupportedTypes => JTokenTypeConstants.String;
        
        public override IEnumerable<JTokenType> SupportedResults => JTokenTypeConstants.String;
        
        public override IEnumerable<IJTokenTransformer> Sources => _sources;

        public override IJTokenTransformer Bind(IJTokenTransformer source)
        {
            if (source == null) throw new CannotBindToNullException();

            if (_source == null)
            {
                _source = source;
            } else if (_fromValue == null)
            {
                _fromValue = source;
            }
            else if (_countValue == null)
            {
                _countValue = source;
            }
            else
            {
                throw new TransformerBindingException($"Transformer of type `{GetType().Name}` expects either 2-3 bindings (source, from [, count])");
            }
            
            _sources.Add(source);
            
            EnsureNoCycles();

            return this;
        }

        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            if (_source is null || _fromValue is null)
            {
                throw new TransformerBindingException($"Transformer of type `{GetType().Name}` expects either 2-3 bindings (source, from [, count])");
            }
            
            // todo: typecheck?
            var source = _source.Transform(root, ctx).Value<string>();

            if (source == null)
                throw new ArgumentNullException(nameof(source), "Failed to resolve a string from first binding in `substring`");

            var start = _fromValue.Transform(root, ctx).Value<int>();
            var count = default(int?);

            if (_countValue != null)
            {
                count = _countValue.Transform(root, ctx).Value<int>();
            }

            return count.HasValue 
                ? source.Substring(start, count.Value) 
                : source.Substring(start);
        }
    }
}