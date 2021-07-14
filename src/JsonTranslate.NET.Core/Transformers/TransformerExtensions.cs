using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers
{
    public static class TransformerExtensions
    {
        public static T GetConfig<T>(this IJTokenTransformer _, JObject conf)
        {
            return conf.ToObject<T>();
        }
    }
}