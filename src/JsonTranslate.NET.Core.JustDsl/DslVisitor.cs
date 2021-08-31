using System;
using System.Collections.Generic;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.JustDsl
{
    public class DslVisitor : JustDslBaseVisitor<Instruction>
    {
        public override Instruction VisitFunc(JustDslParser.FuncContext context)
        {
            var name = context.IDENTIFIER().GetText();

            var conf = context.config()?.GetText();
            var arguments = context.argumentList()?.argument() ?? Array.Empty<JustDslParser.ArgumentContext>();

            var instruction = new Instruction
            {
                Name = name,
                Config = conf == null ? null : JObject.Parse(conf),
                Bindings = arguments.Select(x => x.Accept(this)).ToList()
            };

            return instruction;
        }

        public override Instruction VisitArgument(JustDslParser.ArgumentContext context)
        {
            return context.func().Accept(this);
        }
    }
}