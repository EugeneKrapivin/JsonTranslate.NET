using JsonTranslate.NET.Core;
using JsonTranslate.NET.Core.JsonDsl;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonTranslate.NET.JsonDsl.UnitTests
{
    [TestFixture]
    public class JsonDslTests
    {
        [Test]
        public void Example()
        {
            var recipe = @"{
  ""name"": ""str_join"",
  ""config"": { ""separator"": "" "" },
  ""bindings"": [
    {
      ""name"": ""lookup"",
      ""config"": {
        ""map"": [
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

            var instruction = new JsonDslSerializer().Deserialize(recipe);

            var transformerFactory = new TransformerFactory();
            var transformer = transformerFactory.BuildTransformationTree(instruction);

            var result = transformer.Transform(sourceJson);

            Assert.That(result.Value<string>(), Is.EqualTo("test!!! this is my unit value"));
        }
    }
}
