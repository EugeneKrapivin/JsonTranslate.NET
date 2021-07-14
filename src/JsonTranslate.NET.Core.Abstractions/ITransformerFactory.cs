using System.Reflection;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Abstractions
{
    public interface ITransformerFactory
    {
        IJTokenTransformer GetTransformer(string name, JObject conf = null);

        TypeInfo RemoveTransformer(string name);

        ITransformerFactory AddTransformer<T>() where T : IJTokenTransformer;
    }
}