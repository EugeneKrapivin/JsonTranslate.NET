using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Transformers.Core.Abstractions;
using Transformers.Core.JustDSL;
using Transformers.Core.Transformers;

namespace TranformerDSLParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var just = new JustDslSerializer();
            var instructions = just.Parse(
@"#str_join({""seperator"":"" ""}, 
    #s_lookup_s({""lookup"":{""testush_missing"":""test!!!""},""onMissing"":""default"",""default"":""test???""}, 
        #valueof({""path"":""$.test""})), 
    #unit({""value"":""fuck you""}))");

			var transformerFactory = CreateTransformerFactory();

            var transformerChain = instructions.BuildTransformationTree(transformerFactory);

            var template = new JObject
            {
                ["me"] = @"#str_join({""seperator"":"" ""}, #s_lookup_s({""lookup"":{""testush_missing"":""test!!!""},""onMissing"":""default"",""default"":""test???""}, #valueof({""path"":""$.test""})), #unit({""value"":""fuck you""}))"
			};
			/*
			 * {
			 *	"type":"strcat",
			 *	"bindings": [{"type":valueof"}]
			 * }
			 */
            var p = template.Descendants().OfType<JProperty>().Where(x => x.Value.Value<string>().StartsWith("#"));

			var source = new JObject
            {
                ["test"] = "testush_missing"
            };

			foreach (var s in p)
            {
                var t = just.Parse(s.Value.Value<string>());

                s.Value = t.BuildTransformationTree(transformerFactory).Transform(source);
            }

            Console.WriteLine(template.ToString(Formatting.Indented));

        }

        private static TransformerFactory CreateTransformerFactory() =>
            new TransformerFactory()
                .RegisterTransformer<ValueOfTransformer>()
                .RegisterTransformer<StringToStringLookupTransformer>()
                .RegisterTransformer<StringJoinTransformer>()
                .RegisterTransformer<UnitTransformer>();
    }
}
