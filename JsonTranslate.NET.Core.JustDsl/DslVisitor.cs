using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;
using Transformers.Core.JustDSL.Parser;

namespace JsonTranslate.NET.Core.JustDSL
{
    public class DslVisitor : JustDslBaseVisitor<Instruction>
    {
        public override Instruction VisitFunc(JustDslParser.FuncContext context)
        {
            var name = context.IDENTIFIER().GetText();
            var conf = context.json().GetText();
            var bindings = context.func();

            var instruction = new Instruction
            {
                Name = name,
                Config = JObject.Parse(conf),
                Bindings = bindings.Select(x => x.Accept(this)).ToList()
            };

            return instruction;
        }
    }
}