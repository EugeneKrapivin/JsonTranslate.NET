using Newtonsoft.Json.Linq;
using Transformers.Core.Abstractions;

namespace Transformers.Core.Transformers
{
    public static class TransformerExtensions
    {
        public static T GetConfig<T>(this IJTokenTransformer _, JObject conf)
        {
            return conf.ToObject<T>();
        }
    }
}