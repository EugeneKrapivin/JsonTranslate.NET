using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Abstractions
{
    public interface IJTokenTransformer : IAccepting<IJTokenTransformer>
    {
        public IEnumerable<JTokenType> SupportedTypes { get; }

        public IEnumerable<JTokenType> SupportedResults { get; }

        public IEnumerable<IJTokenTransformer> Sources { get; }

        JToken Transform(JToken root, TransformationContext ctx = null);

        IJTokenTransformer Bind(IJTokenTransformer source);
    }
}