using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Operators
{
    public abstract class AbstractBinOperator : TransformerBase
    {
        private IJTokenTransformer _loper;
        private IJTokenTransformer _roper;
        
        private readonly List<IJTokenTransformer> _sources = new(2);

        public override IEnumerable<IJTokenTransformer> Sources => _sources;

        public override IJTokenTransformer Bind(IJTokenTransformer source)
        {
            EnsureSource(source);

            if (_loper is null)
            {
                _loper = source;
            }
            else if (_roper is null)
            {
                _roper = source;
            }
            else
            {
                throw new NotSupportedException(
                    $"Binary operator `{GetType().Name}` requires exactly 2 bindings for right and left operands");
            }
            
            _sources.Add(source);
            
            EnsureNoCycles();

            return this;
        }

        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var loper = _loper.Transform(root, ctx).ValidateNonNull();

            var roper = _roper.Transform(root, ctx).ValidateNonNull();

            return Operate(loper, roper);
        }

        protected abstract JToken Operate(JToken loper, JToken roper);
    }
}