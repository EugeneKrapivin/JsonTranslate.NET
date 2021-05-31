using System;

namespace JsonTranslate.NET.Core.Abstractions
{
    public sealed class TransformerAttribute : Attribute
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