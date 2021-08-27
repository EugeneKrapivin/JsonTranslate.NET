using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.String.Operators
{
    public abstract class StringOperator : TransformerBase
    {
        private IJTokenTransformer _loper;
        private IJTokenTransformer _roper;
        
        private readonly List<IJTokenTransformer> _sources = new (2);

        public override IEnumerable<JTokenType> InputTypes => JTokenTypeConstants.String;

        public override IEnumerable<IJTokenTransformer> Sources => _sources;

        public override IJTokenTransformer Bind(IJTokenTransformer source)
        {
            EnsureSource(source);

            if (_loper == null)
            {
                _loper = source;
            } 
            else if (_roper == null)
            {
                _roper = source;
            }
            else
            {
                throw new TransformerBindingException($"Transformer `{GetType().Name}` supports exactly 2 bindings");
            }
            
            _sources.Add(source);

            EnsureNoCycles();

            return this;
        }

        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var loper = _loper.Transform(root, ctx)
                .ValidateNonNull()
                .ValidateType(JTokenType.String)
                .Value<string>();
            
            var roper = _loper
                .Transform(root, ctx)
                .ValidateNonNull()
                .ValidateType(JTokenType.String)
                .Value<string>();

            return Operate(loper, roper, ctx);
        }
        protected abstract JToken Operate(string loper, string roper, TransformationContext ctx = null);
    }
}