using System;
using System.Linq;
using JsonTranslate.NET.Core;
using JsonTranslate.NET.Core.JustDSL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Runner
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
                ["me"] = @"#str_join({""separator"":"" ""}, #s_lookup_s({""lookup"":{""testush_missing"":""test!!!""},""onMissing"":""default"",""default"":""test???""}, #valueof({""path"":""$.test""})), #unit({""value"":""this is my unit value""}))",
                ["sssss"] = @"#sum({}, #valueof({""path"":""$.arr""}))"
            };

            var p = template.Descendants().OfType<JProperty>().Where(x => x.Value.Value<string>().StartsWith("#"));

			var source = new JObject
            {
                ["test"] = "testush_missing!",
                ["arr"] = new JArray(1,2,3,4,5,6,7,8,9.0)
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
