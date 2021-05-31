using System;
using Newtonsoft.Json;
using Transformers.Core.Abstractions;

namespace Transformers.Core.JsonDSL
{
    public class JsonDslSerializer : ISerializeDSL
    {
        public string ToString(Instruction instruction) 
            => JsonConvert.SerializeObject(instruction);

        public Instruction Parse(string source) 
            => JsonConvert.DeserializeObject<Instruction>(source);
    }
}
