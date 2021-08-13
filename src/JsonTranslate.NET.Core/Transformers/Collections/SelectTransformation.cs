using System;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Collections
{
    [Transformer(TransformerName)]
    public class SelectTransformation : IJTokenTransformer
    {
        private const string TransformerName = "select";

        private IJTokenTransformer _source;

        private IJTokenTransformer _projection;

        public SelectTransformation()
            : this(null)
        {

        }
        
        public SelectTransformation(JObject conf)
        {

        }

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (_source == null)
            {
                _source = source;
            }
            else
            {
                _projection = source;
            }

            return this;
        }

        public string SourceType { get; set; }
        
        public string TargetType { get; set; }

        public JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var arrResult = _source.Transform(root); // select the array from the object root

            if (arrResult == null) return arrResult; // TODO

            var arr = arrResult; 
            if (arr.Type != JTokenType.Array)
            {
                throw new Exception(); // TODO
            }

            var obj = arr.Select(item => item.DeepClone()) // clone so that we don't change the current element for the whole tree
                .Select(item => _projection.Transform(root, new TransformationContext { Root = root, CurrentItem = item}))
                .Aggregate(new JArray(), (o, token) =>
                {
                    o.Add(token);
                    return o;
                });

            return obj;
        }

        public TR Accept<TR>(IVisitor<IJTokenTransformer, TR> visitor)
        {
            return visitor.Visit(this);
        }
    }
}