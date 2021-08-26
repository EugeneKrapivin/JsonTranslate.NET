using System;
using JsonTranslate.NET.Core.Transformers;
using JsonTranslate.NET.Core.Transformers.Collections;
using JsonTranslate.NET.Core.Transformers.TypeConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.Collections
{
    //todo fix fix fix
    [TestFixture]
    public class SelectTransformerUnitTests
    {
        [Test]
        public void Transformer_Successfully_Create_Projected_Collection_Of_Objects()
        {
            var type_prop = new KeyedTransformer()
                .Bind(new UnitTransformer(new() { ["value"] = "t" }))
                .Bind(new CurrentItemSelector()
                    .Bind(new ValueOfTransformer(new() { ["path"] = "$.type" })));

            var city_prop = new KeyedTransformer()
                .Bind(new UnitTransformer(new() { ["value"] = "c" }))
                .Bind(new CurrentItemSelector()
                    .Bind(new ValueOfTransformer(new() { ["path"] = "$.city" })));

            var obj = new ObjectTransformer()
                .Bind(type_prop)
                .Bind(city_prop);

            var sut = new SelectTransformation();

            var source = JArray.Parse(@"[{
                ""type"": ""home"",
                ""street1"":"""",
                ""street2"":"""",
                ""zip"":"""",
                ""city"":""hadera"",
                ""country"":"""",
            }, {
                ""type"": ""office"",
                ""street1"":"""",
                ""street2"":"""",
                ""zip"":"""",
                ""city"":""tel aviv"",
                ""country"":"""",
            }]");

            sut
                .Bind(new ValueOfTransformer(new() { ["path"] = "$" }))
                .Bind(obj);

            var actual = sut.Transform(source);

            var expected = new JArray
            {
                JObject.FromObject(new {t = "home", c = "hadera"}),
                JObject.FromObject(new {t = "office", c = "tel aviv"})
            };

            Console.WriteLine("From:");
            Console.WriteLine(source.ToString(Formatting.Indented));
            Console.WriteLine("To:");
            Console.WriteLine(actual.ToString(Formatting.Indented));

            Assert.That(JToken.DeepEquals(expected, actual));
        }

        [Test]
        public void Transformer_Successfully_Create_Projected_Collection_Of_ComplexObjects()
        {
            var source = JObject.Parse(
                @"{
	""date"": ""04-08-2021 19:26:00+03:00"",
	""currency"": ""eur"",
	""transactions"": [
		{
			""type"": ""hotel"",
			""amount"": 100
		},
		{
			""type"": ""car"",
			""amount"": 25
		},
		{
			""type"": ""flight"",
			""amount"": 150
		},
		{
			""type"": ""flight"",
			""amount"": 150
		}
	]
}");

            var expected = JObject.Parse(
                @"{
	""date"": ""04-08-2021 19:26:00+03:00"",
	""purchases"": [
		{
			""type"": ""hotel"",
			""amount"": 100,
            ""currency"": ""eur""
		},
		{
			""type"": ""car"",
			""amount"": 25,
            ""currency"": ""eur""
		},
		{
			""type"": ""flight"",
			""amount"": 150,
            ""currency"": ""eur""
		},
		{
			""type"": ""flight"",
			""amount"": 150,
            ""currency"": ""eur""
		}
	]
}");

            var type_prop = new KeyedTransformer()
                .Bind(new UnitTransformer(new() { ["value"] = "type" }))
                .Bind(new CurrentItemSelector()
                    .Bind(new ValueOfTransformer(new() { ["path"] = "$.type" })));

            var amount_prop = new KeyedTransformer()
                .Bind(new UnitTransformer(new() { ["value"] = "amount" }))
                .Bind(new CurrentItemSelector()
                    .Bind(new ValueOfTransformer(new() { ["path"] = "$.amount" })));

            var currency_prop = new KeyedTransformer()
                .Bind(new UnitTransformer(new() { ["value"] = "currency" }))
                .Bind(new ValueOfTransformer(new() { ["path"] = "$.currency" }));

            var obj = new ObjectTransformer()
                .Bind(type_prop)
                .Bind(amount_prop)
                .Bind(currency_prop);

            var selector = new SelectTransformation();

            selector
                .Bind(new ValueOfTransformer(new() { ["path"] = "$.transactions" }))
                .Bind(obj);

            var root = new ObjectTransformer()
                .Bind(new KeyedTransformer()
                    .Bind(new UnitTransformer(new() { ["value"] = "date" }))
                    .Bind(new ValueOfTransformer(new() { ["path"] = "$.date" })))
                .Bind(new KeyedTransformer()
                    .Bind(new UnitTransformer(new() { ["value"] = "purchases" }))
                    .Bind(selector));

            var r = root.Transform(source);

            Console.WriteLine("From:");
            Console.WriteLine(source.ToString(Formatting.Indented));
            Console.WriteLine("To:");
            Console.WriteLine(r.ToString(Formatting.Indented));

            Assert.That(JToken.DeepEquals(expected, r));
        }

        [Test]
        public void Transformer_Successfully_Create_Projected_Collection_Of_Primitives()
        {
            var toInt = new ToIntegerTransformer()
                .Bind(new CurrentItemSelector()
                    .Bind(new ValueOfTransformer(new() { ["path"] = "$" })));

            var sut = new SelectTransformation();

            var source = JArray.Parse(@"[""0"",""1"",""2"",""3""]");

            sut.Bind(new ValueOfTransformer(new() { ["path"] = "$" }))
                .Bind(toInt);

            var r = sut.Transform(source);

            var expected = new JArray { 0, 1, 2, 3 };

            Assert.That(JToken.DeepEquals(expected, r));

            Console.WriteLine("From:");
            Console.WriteLine(source.ToString(Formatting.Indented));
            Console.WriteLine("To:");
            Console.WriteLine(r.ToString(Formatting.Indented));
        }
    }
}