using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Transformers.Core.Abstractions;
using Transformers.Core.JustDSL.Parser;

namespace Transformers.Core.JustDSL
{
    public class JustDslSerializer : ISerializeDSL
    {
        public string ToString(Instruction instruction)
        {
            var sb = new StringBuilder();

            sb.Append($"#{instruction.Name}(");

            if (instruction.Config != null)
            {
                sb.Append(instruction.Config.ToString(Newtonsoft.Json.Formatting.None));
            }

            if (instruction.Bindings?.Any() == true)
            {
                sb.Append(", ");
                sb.Append(string.Join(", ", instruction.Bindings.Select(x => x.ToString())));
            }

            sb.Append($")");

            return sb.ToString();
        }

        public Instruction Parse(string dsl)
        {
            var antlrStream = new AntlrInputStream(dsl);
            var lexer = new JustDslLexer(antlrStream);
            var tokenizer = new CommonTokenStream(lexer);
            var parser = new JustDslParser(tokenizer);

            var ctx = parser.func();

            var visitor = new DslVisitor();

            return visitor.Visit(ctx);
        }
    }
}
