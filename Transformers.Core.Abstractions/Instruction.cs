using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Transformers.Core.Abstractions
{
    public class Instruction
    {
        public string Name { get; set; }

        public JObject Config { get; set; }

        public List<Instruction> Bindings { get; set; }

        public ISerializeDSL SerializeDsl { get; set; }

        public IJTokenTransformer BuildTransformationTree(ITransformerFactory factory)
        {
            var root = factory.GetTransformer(Name, Config);

            foreach (var binding in Bindings ?? new List<Instruction>())
            {
                root.Bind(binding.BuildTransformationTree(factory));
            }

            return root;
        }

        public override string ToString() 
            => SerializeDsl != null 
                ? SerializeDsl.ToString(this) 
                : base.ToString();
    }
}