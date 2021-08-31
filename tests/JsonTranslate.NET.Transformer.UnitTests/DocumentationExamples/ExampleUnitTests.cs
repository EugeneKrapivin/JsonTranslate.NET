using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonTranslate.NET.Core;
using JsonTranslate.NET.Core.JsonDsl;
using JsonTranslate.NET.Core.JustDsl;
using JsonTranslate.NET.Core.Transformers;
using JsonTranslate.NET.Core.Transformers.Collections;
using JsonTranslate.NET.Core.Visitors;
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

        [Test]
        public void ValueOf_Extract_Data_Example()
        {
            var input = JObject.Parse(@"{
  ""menu"": {
    ""popup"": {
      ""menuitem"": [{
          ""value"": ""Open"",
          ""onclick"": ""OpenDoc()""
        }, {
          ""value"": ""Close"",
          ""onclick"": ""CloseDoc()""
        }
      ],
	  ""submenuitem"": ""CloseSession()""
    }
  }
}");
            var expected = JObject.Parse(@"{
  ""result"": {
    ""Open"": ""OpenDoc()"",
	""Close"": ""CloseDoc()""
  }
}");
            var recipe = new ObjectTransformer()
                .Bind(new PropertyTransformer()
                    .Bind("result".AsUnit())
                    .Bind(new ObjectTransformer()
                        .Bind(new PropertyTransformer()
                            .Bind("Open".AsUnit())
                            .Bind(new ValueOfTransformer(new() {["path"] = "$.menu.popup.menuitem[?(@.value=='Open')].onclick" })))
                        .Bind(new PropertyTransformer()
                            .Bind("Close".AsUnit())
                            .Bind(new ValueOfTransformer(new() { ["path"] = "$.menu.popup.menuitem[?(@.value=='Close')].onclick" })))));
            var actual = recipe.Transform(input);


            Console.WriteLine(new JsonDslSerializer().Serialize(recipe.Accept(new ToInstructionVisitor())));

            Assert.That(JToken.DeepEquals(actual, expected));

        }
    }
}
