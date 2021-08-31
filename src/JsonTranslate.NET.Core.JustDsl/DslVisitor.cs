using System;
using System.Collections.Generic;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.JustDsl
{
    public class DslVisitor : JustDslBaseVisitor<Instruction>
    {
        public override Instruction VisitStart(JustDslParser.StartContext context)
        {
            return context.func().Accept(this);
        }

        public override Instruction VisitFunc(JustDslParser.FuncContext context)
        {
            var name = context.IDENTIFIER().GetText();
            var @params = context.parameter_list();

            JustDslParser.ConfigContext configCtx = null;
            JustDslParser.ArgumentContext[] argumentContexts;

            if (@params.config() == null)
            {
                argumentContexts = @params.no_config_parameter_list()?.argument() ??
                               Array.Empty<JustDslParser.ArgumentContext>();
            }
            else
            {
                configCtx = @params.config();
                argumentContexts = @params.argument_list()?.argument() ??
                               Array.Empty<JustDslParser.ArgumentContext>();
            }
            
            var instruction = new Instruction
            {
                Name = name,
                Config = configCtx == null ? null : JObject.Parse(configCtx.GetText()),
                Bindings = argumentContexts.Select(x => x.Accept(this)).ToList()
            };

            return instruction;
        }

        public override Instruction VisitArgument(JustDslParser.ArgumentContext context)
        {
            return context.func().Accept(this);
        }
    }
}