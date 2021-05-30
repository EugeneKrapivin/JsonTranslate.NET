using System.Linq;
using Newtonsoft.Json.Linq;
using TransformerDSL.Parser;

namespace TranformerDSLParser.Core
{
    public class DslVisitor : TransformerDSLBaseVisitor<Instruction>
    {
        public override Instruction VisitFunc(TransformerDSLParser.FuncContext context)
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