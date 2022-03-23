using System;
using System.Linq;
using System.Reflection;
using JsonTranslate.NET.Core;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.JustDsl;
using JsonTranslate.NET.Core.Transformers;
using JsonTranslate.NET.Core.Transformers.Collections;
using JsonTranslate.NET.Core.Transformers.String.Reducers;
using JsonTranslate.NET.Core.Visitors;
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
                .Bind(new PropertyTransformer()
                    .Bind("phoneNumbers".AsUnit())
                    .Bind(new AggregateObjectTransformer()
                        .Bind(new ValueOfTransformer(new() {["path"] = "$.phone_numbers"}))
                        .Bind(new CurrentItemSelector()
                            .Bind(new ValueOfTransformer(new() {["path"] = "$.type"})))
                        .Bind(new CurrentItemSelector()
                            .Bind(new ValueOfTransformer(new() {["path"] = "$.number"})))))
                .Bind(new PropertyTransformer()
                    .Bind("addrs".AsUnit())
                    .Bind(new AggregateObjectTransformer()
                        .Bind(new ValueOfTransformer(new() {["path"] = "$.addresses"}))
                        .Bind(new CurrentItemSelector()
                            .Bind(new ValueOfTransformer(new() {["path"] = "$.type"})))
                        .Bind(new CurrentItemSelector()
                            .Bind(new ObjectTransformer()
                                .Bind(new PropertyTransformer()
                                    .Bind("city".AsUnit())
                                    .Bind(new ValueOfTransformer(new() {["path"] = "$.city"})))
                                .Bind(new PropertyTransformer()
                                    .Bind("country".AsUnit())
                                    .Bind(new ValueOfTransformer(new() {["path"] = "$.country"})))
                                .Bind(new PropertyTransformer()
                                    .Bind("street".AsUnit())
                                    .Bind(new StringJoinTransformer(new() {["separator"] = ", "})
                                        .Bind(new ValueOfTransformer(new() {["path"] = "$.street1"}))
                                        .Bind(new ValueOfTransformer(new() {["path"] = "$.street2"}))
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

            Console.WriteLine(new JustDslSerializer().Serialize(root.Accept(new ToInstructionVisitor())));
        }
    }
}
