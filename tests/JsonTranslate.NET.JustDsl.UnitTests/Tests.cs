using JsonTranslate.NET.Core;
using JsonTranslate.NET.Core.JsonDsl;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.JustDsl.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Category("examples")]
        public void Complex_StrJoin_With_LookUp()
        {
            var str = 
@"{
  ""name"": ""str_join"",
  ""config"": { ""separator"": "" "" },
  ""bindings"": [
    {
      ""name"": ""lookup"",
      ""config"": {
        ""lookup"": [
          {
            ""key"": ""look me up"",
            ""value"": ""test!!!""
          }
        ],
        ""onMissing"": ""default"",
        ""default"": ""test???""
      },
      ""bindings"": [
        {
          ""name"": ""valueof"",
          ""config"": { ""path"": ""$.test"" }
        }
      ]
    },
    {
      ""name"": ""unit"",
      ""config"": { ""value"": ""this is my unit value"" }
    }
  ]
}";

            var sourceJson = new JObject
            {
                ["test"] = "look me up"
            };
            var serilizer = new JsonDslSerializer();
            var instruction = serilizer.Parse(str);
            
            var transformerFactory = new TransformerFactory();
            var transformer = instruction.BuildTransformationTree(transformerFactory);

            var result = transformer.Transform(sourceJson, null);
        }
    }
}