using System;
using JsonTranslate.NET.Core.Transformers;
using JsonTranslate.NET.Core.Transformers.Collections;
using JsonTranslate.NET.Core.Transformers.String.Reducers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.Collections
{
    [TestFixture]
    public class ToObjectTransformerUnitTests
    {
        [Test]
        public void ToObject_Should_Return_An_Object()
        {
            var source = new JObject
            {
                ["phone_numbers"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "home",
                        ["number"] = "+1-555-5551"
                    },
                    new JObject
                    {
                        ["type"] = "work",
                        ["number"] = "+1-555-5552"
                    }
                },
                ["addresses"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "home",
                        ["city"] = "New-York",
                        ["street1"] = "1st Ave 1",
                        ["street2"] = "Apt 11",
                        ["country"] = "USA",
                    },
                    new JObject
                    {
                        ["type"] = "work",
                        ["city"] = "New-York",
                        ["street1"] = "1st Ave 2",
                        ["street2"] = "floor 100",
                        ["country"] = "USA",
                    },
                }
            };

            var expected = new JObject
            {
                ["phoneNumbers"] = new JObject 
                { 
                    ["home"] = "+1-555-5551",
                    ["work"] = "+1-555-5552"
                },
                ["addrs"] = new JObject
                {
                    ["home"] = new JObject
                    {
                        ["city"] = "New-York",
                        ["street"] = "1st Ave 1, Apt 11",
                        ["country"] = "USA",
                    },
                    ["work"] = new JObject
                    {
                        ["city"] = "New-York",
                        ["street"] = "1st Ave 2, floor 100",
                        ["country"] = "USA",
                    },
                }
            };

            var root = new ObjectTransformer()
                    .Bind(new KeyedTransformer()
                        .Bind("phoneNumbers".AsTransformationResult())
                        .Bind(new ToObjectTransformer()
                            .Bind(new ValueOfTransformer(new() { ["path"] = "$.phone_numbers" }))
                            .Bind(new CurrentItemSelector()
                                .Bind(new ValueOfTransformer(new() { ["path"] = "$.type" })))
                            .Bind(new CurrentItemSelector()
                                .Bind(new ValueOfTransformer(new() { ["path"] = "$.number" })))))
                    .Bind(new KeyedTransformer()
                        .Bind("addrs".AsTransformationResult())
                        .Bind(new ToObjectTransformer()
                            .Bind(new ValueOfTransformer(new() { ["path"] = "$.addresses" }))
                            .Bind(new CurrentItemSelector()
                                 .Bind(new ValueOfTransformer(new() { ["path"] = "$.type" })))
                            .Bind(new CurrentItemSelector()
                                 .Bind(new ObjectTransformer()
                                    .Bind(new KeyedTransformer()
                                        .Bind("city".AsTransformationResult())
                                        .Bind(new ValueOfTransformer(new() { ["path"] = "$.city" })))
                                    .Bind(new KeyedTransformer()
                                        .Bind("country".AsTransformationResult())
                                        .Bind(new ValueOfTransformer(new() { ["path"] = "$.country" })))
                                    .Bind(new KeyedTransformer()
                                        .Bind("street".AsTransformationResult())
                                        .Bind(new StringJoinAggregator(new() { ["separator"] = ", " })
                                            .Bind(new ValueOfTransformer(new() { ["path"] = "$.street1" }))
                                            .Bind(new ValueOfTransformer(new() { ["path"] = "$.street2" }))
                                        )
                                    )
                                 )
                            )
                        )
                    );

            var actual = root.Transform(source);

            Console.WriteLine("From:");
            Console.WriteLine(source.ToString(Formatting.Indented));
            Console.WriteLine("To:");
            Console.WriteLine(actual.ToString(Formatting.Indented));

            Assert.That(JToken.DeepEquals(expected, actual));

        }
    }
}
