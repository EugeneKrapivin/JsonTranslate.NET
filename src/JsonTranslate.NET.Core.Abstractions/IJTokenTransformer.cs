using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Abstractions
{
    public interface IJTokenTransformer
    {
        string SourceType { get; }

        string TargetType { get; }

        JToken Transform(JToken root);

        IJTokenTransformer Bind(IJTokenTransformer source);
    }
}