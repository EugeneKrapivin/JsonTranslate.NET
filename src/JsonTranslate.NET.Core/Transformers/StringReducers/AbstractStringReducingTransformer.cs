using System;
using System.Collections.Generic;
using System.Text;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.StringReducers
{
    public abstract class AbstractStringReducingTransformer : IJTokenTransformer
    {
        public string SourceType => "string";

        public virtual string TargetType => "string";

        protected readonly List<IJTokenTransformer> _sources = new ();

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            _sources.Add(source);

            return this;
        }

        public abstract JToken Transform(JToken root);
    }
}
