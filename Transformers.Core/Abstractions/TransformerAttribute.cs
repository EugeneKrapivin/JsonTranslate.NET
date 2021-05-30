using System;

namespace TranformerDSLParser.Core
{
    public class TransformerAttribute : Attribute
    {
        public bool RequiresConfig { get; private set; }

        public string Name { get; }

        public TransformerAttribute(string name, bool requiresConfig = false)
        {
            Name = name;
            RequiresConfig = requiresConfig;
        }
    }
}