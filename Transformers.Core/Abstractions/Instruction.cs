using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TranformerDSLParser.Transformers;
using TransformerDSL.Parser;

namespace TranformerDSLParser.Core
{
    public class Instruction
    {
        public string Name { get; set; }

        public JObject Config { get; set; }

        public List<Instruction> Bindings { get; set; }

        public IJTokenTransformer BuildTransformationTree(TransformerFactory factory)
        {
            var root = factory.GetTransformer(Name, Config);

            foreach (var binding in Bindings ?? new List<Instruction>())
            {
                root.Bind(binding.BuildTransformationTree(factory));
            }

            return root;
        }

        public override string ToString() => this.ToString(Formatting.Json);

        public string ToString(Formatting formatting = Formatting.Json)
        {
            if (formatting == Formatting.Json)
            {
                return JsonConvert.SerializeObject(this);
            }

            var sb = new StringBuilder();

            sb.Append($"#{Name}(");

            if (Config != null)
            {
                sb.Append(Config.ToString(Newtonsoft.Json.Formatting.None));
            }

            if (Bindings?.Any() == true)
            {
                sb.Append(", ");
                sb.Append(string.Join(", ", Bindings.Select(x => x.ToString())));
            }

            sb.Append($")");

            return sb.ToString();
        }

        public static Instruction Parse(string source)
        {
            if (source.StartsWith("#"))
            {
                return ParseFromDSL(source);
            }

            if (source.StartsWith("{") && source.EndsWith("}")) // poormans test that we are handling a json
            {
                try
                {
                    return JsonConvert.DeserializeObject<Instruction>(source);
                }
                catch (Exception ex) // todo capture actual deser fail exception
                {
                    throw new Exception("fuck you thats ain't json");
                }
            }

            throw new Exception("so mate... didn't read the documentation huh?");

        }

        private static Instruction ParseFromDSL(string source)
        {
            var antlrStream = new AntlrInputStream(source);
            var lexer = new TransformerDSLLexer(antlrStream);
            var tokenizer = new CommonTokenStream(lexer);
            var parser = new TransformerDSLParser(tokenizer);

            var ctx = parser.func();

            var visitor = new DslVisitor();

            return visitor.Visit(ctx);
        }

        public enum Formatting
        {
            DSL,
            Json
        }
    }
}