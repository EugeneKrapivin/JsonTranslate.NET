using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Abstractions
{
    public class Instruction : IAccepting<Instruction>
    {
        public string Name { get; set; }

        public JObject Config { get; set; }

        public List<Instruction> Bindings { get; set; }

        public void Deconstruct(out string name, out JObject config, out IReadOnlyCollection<Instruction> bindings)
        {
            name = Name;
            config = Config;
            bindings = Bindings;
        }

        public TR Accept<TR>(IVisitor<Instruction, TR> visitor)
        {
            return visitor.Visit(this);
        }
    }
}