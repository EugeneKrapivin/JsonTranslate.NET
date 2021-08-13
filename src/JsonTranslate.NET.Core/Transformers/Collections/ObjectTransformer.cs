using System;
using System.Collections.Generic;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Collections
{
    [Transformer("obj")]
    public class ObjectTransformer : IJTokenTransformer
    {
        public TR Accept<TR>(IVisitor<IJTokenTransformer, TR> visitor) => visitor.Visit(this);

        public string Name => "obj";

        private readonly List<IJTokenTransformer> _sources = new();

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            if (source is KeyedTransformer)
            {
                _sources.Add(source);
            }
            else
            {
                throw new NotSupportedException(
                    $"Transformer of type `{Name}` only supports bindings of type `{KeyedTransformer.TransformerName}`");
            }

            return this;
        }

        public string SourceType { get; set; }
        public string TargetType { get; set; }

        public JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var properties = _sources.Select(x => x.Transform(root, ctx)).ToArray();

            // TODO: validate all properties are OK

            return new JObject(properties); // TODO WARN
        }
    }
}