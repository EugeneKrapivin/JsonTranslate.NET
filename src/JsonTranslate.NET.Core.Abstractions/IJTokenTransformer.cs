using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Abstractions
{
    public interface IAccepting<out T>
    {
        TR Accept<TR>(IVisitor<T, TR> visitor);
    }

    public interface IVisitor<in T, out TR>
    {
        TR Visit(T target);
    }

    public interface IJTokenTransformer : IAccepting<IJTokenTransformer>
    {
        public IEnumerable<JTokenType> SupportedTypes { get; }

        public IEnumerable<JTokenType> SupportedResults { get; }

        public IEnumerable<IJTokenTransformer> Sources { get; }

        JToken Transform(JToken root, TransformationContext ctx = null);

        IJTokenTransformer Bind(IJTokenTransformer source);
    }

    public class TransformationContext
    {
        public JToken Root { get; set; }
        public JToken CurrentItem { get; set; }
    }
}