using System;
using System.Linq;
using JsonTranslate.NET.Core;
using JsonTranslate.NET.Core.JustDSL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace JsonTranslate.NET.Runner
{
    public class Program
    {
        public static void Main()
        {
            var just = new JustDslSerializer();

            var transformerFactory = new TransformerFactory();


            var template = new JObject
            {
                ["me"] = @"#str_join({""separator"":"" ""}, #lookup({""lookup"":[{""key"":""look me up"",""value"":""test!!!""}],""onMissing"":""default"",""default"":""test???""}, #valueof({""path"":""$.test""})), #unit({""value"":""this is my unit value""}))",
            };

            var p = template.Descendants().OfType<JProperty>().Where(x => x.Value.Value<string>().StartsWith("#"));

			var source = new JObject
            {
                ["test"] = "look me up",
            };

			foreach (var s in p)
            {
                var t = just.Parse(s.Value.Value<string>());
                System.Console.WriteLine(JsonConvert.SerializeObject(t, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.Indented
                }));
                
                s.Value = t.BuildTransformationTree(transformerFactory).Transform(source);
            }

            Console.WriteLine(template.ToString(Formatting.Indented));

        }
    }
}
