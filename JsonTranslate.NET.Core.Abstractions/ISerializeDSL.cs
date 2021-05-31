using System;
using System.Collections.Generic;
using System.Text;

namespace Transformers.Core.Abstractions
{
    public interface ISerializeDSL
    {
        string ToString(Instruction instructions);
        
        Instruction Parse(string dsl);
    }
}
