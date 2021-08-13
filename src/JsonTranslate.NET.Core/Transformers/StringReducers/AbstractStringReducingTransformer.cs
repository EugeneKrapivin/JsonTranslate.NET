using System;
using System.Collections.Generic;
using System.Text;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.StringReducers
{
    public abstract class AbstractStringReducingTransformer : IJTokenTransformer
    {

        protected readonly List<IJTokenTransformer> _sources = new ();

        public IJTokenTransformer Bind(IJTokenTransformer source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            _sources.Add(source);

            return this;
        }

        public abstract JToken Transform(JToken root, TransformationContext ctx = null);

        public TR Accept<TR>(IVisitor<IJTokenTransformer, TR> visitor)
            => visitor.Visit(this);
    }
}
