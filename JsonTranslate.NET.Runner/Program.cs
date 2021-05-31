using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Transformers.Core;
using Transformers.Core.Abstractions;
using Transformers.Core.JustDSL;
using Transformers.Core.Transformers;

namespace TranformerDSLParser
{
    public class Program
    {
        public static void Main()
        {
            var just = new JustDslSerializer();
            var instructions = just.Parse(
@"#str_join({""separator"":"" ""}, 
    #s_lookup_s({""lookup"":{""testush_missing"":""test!!!""},""onMissing"":""default"",""default"":""test???""}, 
        #valueof({""path"":""$.test""})), 
    #unit({""value"":""fuck you""}))");

			var transformerFactory = new TransformerFactory();

            var transformerChain = instructions.BuildTransformationTree(transformerFactory);

            var template = new JObject
            {
                ["me"] = @"#str_join({""separator"":"" ""}, #s_lookup_s({""lookup"":{""testush_missing"":""test!!!""},""onMissing"":""default"",""default"":""test???""}, #valueof({""path"":""$.test""})), #unit({""value"":""fuck you""}))"
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
    }
}
