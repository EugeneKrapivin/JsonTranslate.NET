using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.JustDSL
{
    public class DslVisitor : JustDslBaseVisitor<Instruction>
    {
        public override Instruction VisitFunc(JustDslParser.FuncContext context)
        {
            var name = context.IDENTIFIER().GetText();
            var conf = context.config()?.GetText();
            var bindings = context.func();

            var instruction = new Instruction
            {
                Name = name,
                Config = conf == null ? null : JObject.Parse(conf),
                Bindings = bindings.Select(x => x.Accept(this)).ToList()
            };

            return instruction;
        }

        public override Instruction VisitArgumentList()
        {
            
        }
    }
}