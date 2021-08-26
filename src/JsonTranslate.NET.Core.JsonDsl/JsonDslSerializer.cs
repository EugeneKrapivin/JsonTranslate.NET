using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json;

namespace JsonTranslate.NET.Core.JsonDsl
{
    public class JsonDslSerializer : ISerializeDSL
    {
        public string ToString(Instruction instruction) 
            => JsonConvert.SerializeObject(instruction);

        public Instruction Parse(string source) 
            => JsonConvert.DeserializeObject<Instruction>(source);
    }
}
