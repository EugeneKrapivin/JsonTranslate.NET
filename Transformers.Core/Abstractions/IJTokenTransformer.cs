using Newtonsoft.Json.Linq;

namespace TranformerDSLParser.Core
{
    public interface IJTokenTransformer
    {
        public static string Name { get; }

        public static bool RequiresConfig { get; }

        string SourceType { get; }

        string TargetType { get; }

        JToken Transform(JToken root);

        public static T GetConfig<T>(JObject conf)
        {
            return conf.ToObject<T>();
        }

        IJTokenTransformer Bind(params IJTokenTransformer[] source);
    }
}