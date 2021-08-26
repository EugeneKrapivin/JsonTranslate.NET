using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Operators
{
    [Transformer("if")]
    public class IfTransformer : TransformerBase
    {
        private IJTokenTransformer _conditional;
        
        private IJTokenTransformer _trueResult;
        
        private IJTokenTransformer _falseResult;

        private readonly List<IJTokenTransformer> _sources = new(3);

        public override IEnumerable<IJTokenTransformer> Sources => _sources;

        public override IEnumerable<JTokenType> SupportedTypes => JTokenTypeConstants.Any;

        public override IEnumerable<JTokenType> SupportedResults => JTokenTypeConstants.Boolean;



        public override IJTokenTransformer Bind(IJTokenTransformer source)
        {
            EnsureSource(source);

            if (_conditional is null)
            {
                _conditional = source;
            } 
            else if (_trueResult is null)
            {
                _trueResult = source;
            } 
            else if (_falseResult is null)
            {
                _falseResult = source;
            }
            else
            {
                throw new NotSupportedException(
                    $"Transformer `{GetType().Name}` requires exactly 3 bindings for source, true value, false value");
            }

            _sources.Add(source);

            EnsureNoCycles();

            return this;
        }

        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var predicateResult = _conditional
                .Transform(root, ctx)
                .ValidateNonNull()
                .ValidateType(JTokenType.Boolean);

            var predicate = predicateResult.Value<bool>();

            var trueValue = _trueResult.Transform(root, ctx);
            var falseValue = _falseResult.Transform(root, ctx);

            if (trueValue.Type != falseValue.Type || !(trueValue.IsNumeric() && falseValue.IsNumeric()))
            {
                throw new IncompatibleTypeException(trueValue.Type, falseValue.Type);
            }

            return predicate
                ? trueValue
                : falseValue;
        }
    }
}
