using System;
using JsonTranslate.NET.Core;
using JsonTranslate.NET.Core.JustDsl;
using Newtonsoft.Json;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.JustDsl.UnitTests
{
    public class JustDslTests
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
            var instruction = serilizer.Deserialize(str);
            
            var transformerFactory = new TransformerFactory();
            var transformer = transformerFactory.BuildTransformationTree(instruction);

            var result = transformer.Transform(sourceJson);

            Assert.That(result.Value<string>(), Is.EqualTo("test!!! this is my unit value"));
        }

        [Test]
        [Category("examples")]
        [Ignore("bug in antlr parser, issue #4")]
        public void Parse_From_String_And_Successfully_Execute()
        {
            var source = JObject.Parse("{\"phone_numbers\":[{\"type\":\"home\",\"number\":\"+1-555-5551\"},{\"type\":\"work\",\"number\":\"+1-555-5552\"}],\"addresses\":[{\"type\":\"home\",\"city\":\"New-York\",\"street1\":\"1st Ave 1\",\"street2\":\"Apt 11\",\"country\":\"USA\"},{\"type\":\"work\",\"city\":\"New-York\",\"street1\":\"1st Ave 2\",\"street2\":\"floor 100\",\"country\":\"USA\"}]}");

            var expected = JObject.Parse("{\"phoneNumbers\":{\"home\":\"+1-555-5551\",\"work\":\"+1-555-5552\"},\"addrs\":{\"home\":{\"city\":\"New-York\",\"country\":\"USA\",\"street\":\"1st Ave 1, Apt 11\"},\"work\":{\"city\":\"New-York\",\"country\":\"USA\",\"street\":\"1st Ave 2, floor 100\"}}}");

            var root = @"#obj(#property(#unit({""value"":""phoneNumbers""}), #agr_obj(#valueof({""path"":""$.phone_numbers""}), #current(#valueof({""path"":""$.type""})), #current(#valueof({""path"":""$.number""})))), #property(#unit({""value"":""addrs""}), #agr_obj(#valueof({""path"":""$.addresses""}), #current(#valueof({""path"":""$.type""})), #current(#obj(#property(#unit({""value"":""city""}), #valueof({""path"":""$.city""})), #property(#unit({""value"":""country""}), #valueof({""path"":""$.country""})), #property(#unit({""value"":""street""}), #str_join({""separator"":"", ""}, #valueof({""path"":""$.street1""}), #valueof({""path"":""$.street2""}))))))))";

            var instructions = new JustDslSerializer().Deserialize(root);
            var factory = new TransformerFactory();
            var transformer = factory.BuildTransformationTree(instructions);
            var actual = transformer.Transform(source);

            Console.WriteLine("From:");
            Console.WriteLine(source.ToString(Formatting.Indented));
            Console.WriteLine("To:");
            Console.WriteLine(actual.ToString(Formatting.Indented));

            Assert.That(JToken.DeepEquals(expected, actual));
        }
    }
}