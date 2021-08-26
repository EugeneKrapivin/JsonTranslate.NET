using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Abstractions
{
    public class TransformationContext
    {
        public JToken Root { get; set; }
        public JToken CurrentItem { get; set; }
    }
}