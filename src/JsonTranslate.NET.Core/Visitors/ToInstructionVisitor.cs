using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JsonTranslate.NET.Core.Abstractions;

namespace JsonTranslate.NET.Core.Visitors
{
    public class ToInstructionVisitor : IVisitor<IJTokenTransformer, Instruction>
    {
        public Instruction Visit(IJTokenTransformer target) =>
            new()
            {
                Name = target.GetType().GetCustomAttribute<TransformerAttribute>()?.Name,
                Config = target.Config,
                Bindings = target.Sources.Select(source => source.Accept(this)).ToList()
            };
    }
}
