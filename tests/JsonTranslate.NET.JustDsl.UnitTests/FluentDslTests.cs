using JsonTranslate.NET.Core;
using JsonTranslate.NET.Core.JustDsl;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.JustDsl.UnitTests
{
    public class FluentDslTests
    {
        [Test]
        [Category("examples")]
        public void Complex_StrJoin_With_LookUp()
        {
            var str =
                @"#str_join({""separator"":"" ""}, 
                    #lookup({
                        ""map"":[{
                            ""key"":""look me up"",
                            ""value"":""test!!!""}],
                            ""onMissing"":""default"",
                            ""default"":""test???""
                        },
                        #valueof({""path"":""$.test""})), 
                    #unit({""value"":""this is my unit value""}))";

            var sourceJson = new JObject
            {
                ["test"] = "look me up"
            };
            var serilizer = new JustDslSerializer();
            var instruction = serilizer.Parse(str);
            
            var transformerFactory = new TransformerFactory();
            var transformer = transformerFactory.BuildTransformationTree(instruction);

            var result = transformer.Transform(sourceJson);

            Assert.That(result.Value<string>(), Is.EqualTo("test!!! this is my unit value"));
        }
    }
}