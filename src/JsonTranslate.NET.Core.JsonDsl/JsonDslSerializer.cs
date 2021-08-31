using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonTranslate.NET.Core.JsonDsl
{
    public class JsonDslSerializer : ISerializeDsl
    {
        private static readonly JsonSerializerSettings settings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver
            {
                
            },
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            

        };
        public string Serialize(Instruction instruction) 
            => JsonConvert.SerializeObject(instruction, settings);

        public Instruction Deserialize(string source) 
            => JsonConvert.DeserializeObject<Instruction>(source);
    }
}
