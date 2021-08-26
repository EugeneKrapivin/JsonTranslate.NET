using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using JsonTranslate.NET.Core.Transformers;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests
{
    [TestFixture]
    public class UnitTransformerUnitTests
    {
        public static IEnumerable<TestCaseData> PositiveTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new JObject { ["value"] = "expected value" },
                    JToken.FromObject("expected value"));
                yield return new TestCaseData(
                    new JObject { ["value"] = 1 },
                    JToken.FromObject(1));
                yield return new TestCaseData(
                    new JObject { ["value"] = 1d },
                    JToken.FromObject(1d));
                yield return new TestCaseData(
                    new JObject { ["value"] = true },
                    JToken.FromObject(true));
                yield return new TestCaseData(
                    new JObject { ["value"] = new JArray(1, 2, 3, 4) },
                    new JArray(1, 2, 3, 4));
                yield return new TestCaseData(
                    new JObject { ["value"] = new JObject { ["test"] = "value" } },
                    new JObject { ["test"] = "value" });
            }
        }

        [TestCaseSource(nameof(PositiveTestCases))]
        public void Unit_Should_Allow_Any_Value_Type(JObject conf, JToken expected)
        {
            var sut = new UnitTransformer(conf);

            var r = sut.Transform(null);

            Assert.That(r, Is.EqualTo(expected));
        }

        [Test]
        public void Unit_Should_Not_Allow_Null_Value()
        {
            Assert.That(() => new UnitTransformer(new JObject { ["value"] = null }), 
                Throws.TypeOf<TransformerConfigurationInvalidException>());
        }

        [Test]
        public void Unit_Requires_Config()
        {
            Assert.That(() => new UnitTransformer(null), 
                Throws.TypeOf<TransformerConfigurationMissingException>());
        }

        [Test]
        [Ignore("enforce config schema")]
        public void Unit_Requires_Valid_Config()
        {
            Assert.That(() => new UnitTransformer(new JObject()), Throws.ArgumentException);
        }

        [Test]
        public void Unit_Does_Not_Allow_Bindings()
        {
            var sut = new UnitTransformer(new JObject { ["value"] = "valid" });
            Assert.That(() => sut.Bind(Substitute.For<IJTokenTransformer>()), Throws.TypeOf<ValueProvidersCannotBeBoundException>());
        }
    }
}