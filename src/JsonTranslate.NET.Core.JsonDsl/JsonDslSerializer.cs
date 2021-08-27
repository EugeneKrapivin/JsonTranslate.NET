using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json;

namespace JsonTranslate.NET.Core.JsonDsl
{
    public class JsonDslSerializer : ISerializeDsl
    {
        public string Serialize(Instruction instruction) 
            => JsonConvert.SerializeObject(instruction);

        public Instruction Deserialize(string source) 
            => JsonConvert.DeserializeObject<Instruction>(source);
    }
}
