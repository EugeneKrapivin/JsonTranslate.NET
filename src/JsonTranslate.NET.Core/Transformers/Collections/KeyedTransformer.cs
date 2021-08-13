using System;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Collections
{
    [Transformer(TransformerName)]
    public class KeyedTransformer : IJTokenTransformer
    {
        private IJTokenTransformer _keySelector;

        private IJTokenTransformer _valueSelector;

        private readonly IJTokenTransformer[] _sources = new IJTokenTransformer[2];
        public const string TransformerName = "keyed";
        public string Name => TransformerName;

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (_keySelector == null)
            {
                _keySelector = source;
                _sources[0] = source;
            }
            else if (_valueSelector == null)
            {
                _valueSelector = source;
                _sources[1] = source;
            }
            else
            {
                // TODO I could probably do better xD
                throw new NotSupportedException();
            }

            return this;
        }

        public string SourceType { get; set; }
        public string TargetType { get; set; }

        public JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var key = _keySelector.Transform(root, ctx).Value<string>(); // tODO validate string
            var value = _valueSelector.Transform(root, ctx);

            return new JProperty(key, value);
        }

        public TR Accept<TR>(IVisitor<IJTokenTransformer, TR> visitor)
        {
            return visitor.Visit(this);
        }
    }
}