using System;
using System.Collections.Generic;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Collections
{
    [Transformer("obj")]
    public class ToObjectTransformer : TransformerBase
    {
        private IJTokenTransformer _source;
        private IJTokenTransformer _keySelector;
        private IJTokenTransformer _valueSelector;

        public override IEnumerable<JTokenType> SupportedTypes { get; }
        public override IEnumerable<JTokenType> SupportedResults { get; }
        public override IEnumerable<IJTokenTransformer> Sources { get; }

        public override IJTokenTransformer Bind(IJTokenTransformer source)
        {
            EnsureSource(source);

            if (_source == null)
            {
                _source = source;
            }
            else if (_keySelector == null)
            {
                _keySelector = source;
            } else if (_valueSelector == null)
            {
                _valueSelector = source;
            }
            else
            {
                throw new Exception("Transformer expects exactly 3 bindings (sourceSelector, keySelector, valueSelector)");
            }

            EnsureNoCycles();

            return this;
        }

        public override JToken Transform(JToken root, TransformationContext ctx = null) =>
            _source.Transform(root, ctx)
                .ValidateNonNull()
                .ValidateType(JTokenType.Array)
                .As<JArray>()
                .Aggregate(new JObject(), (obj, item) =>
                {
                    var innerCtx = new TransformationContext
                    {
                        Root = root,
                        CurrentItem = item,
                    };

                    var key = _keySelector.Transform(root, innerCtx)
                        .ValidateNonNull()
                        .ValidateType(JTokenType.String);

                    var value = _valueSelector.Transform(root, innerCtx).ValidateNonNull();

                    obj.Add(key.Value<string>()!, value);

                    return obj;
                });
    }
}