using Newtonsoft.Json.Linq;

namespace Transformers.Core.Abstractions
{
    public interface IJTokenTransformer
    {
        string SourceType { get; }

        string TargetType { get; }

        JToken Transform(JToken root);

        IJTokenTransformer Bind(params IJTokenTransformer[] source);
    }
}