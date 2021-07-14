using System;
using System.Collections.Generic;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.StringReducers
{
    [Transformer(name: "toarray", requiresConfig: false)]
    public class ArrayAggregator : IJTokenTransformer
    {
        public string SourceType => "any";
        public string TargetType => "array";

        private List<IJTokenTransformer> _sources = new();

        public JToken Transform(JToken root)
        {
            var arr = new JArray();
            foreach (var element in _sources.Select(source => source.Transform(root)).ToArray())
            {
                arr.Add(element);
            }
            return arr;
        }

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source), "can not bind null transformers");

            _sources.Add(source);

            return this;
        }
    }
}