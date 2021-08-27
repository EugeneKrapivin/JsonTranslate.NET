using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonTranslate.NET.Core;
using JsonTranslate.NET.Core.JsonDsl;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.DocumentationExamples
{
    [TestFixture]
    public class ExampleUnitTest
    {
        [Test]
        public void String_Join_With_LookUp()
        {
            var recipe = @"{
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

            var input = JObject.FromObject(new
            {
                test = "look me up"
            });

            var instruction = new JsonDslSerializer().Deserialize(recipe);

            var factory = new TransformerFactory();
            var tree = factory.BuildTransformationTree(instruction);

        }
    }
}
