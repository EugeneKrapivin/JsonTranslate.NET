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
    public class ValueOfTransformerUnitTests
    {
        public static IEnumerable<TestCaseData> PositiveTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new JObject {["path"] = "expected value"},
                    new JObject {["path"] = "$.path"},
                    JToken.FromObject("expected value"));
                yield return new TestCaseData(
                    new JObject {["inner"] = new JObject {["path"] = "expected value"}},
                    new JObject {["path"] = "$.inner.path"},
                    JToken.FromObject("expected value"));
                yield return new TestCaseData(
                    new JObject {["inner"] = new JObject {["path"] = "expected value"}},
                    new JObject {["path"] = "$..path"},
                    JToken.FromObject("expected value"));
                yield return new TestCaseData(
                    new JObject {["inner"] = new JObject {["inner"] = new JObject {["path"] = "expected value"}}},
                    new JObject {["path"] = "$..[?(@.path)].path"},
                    JToken.FromObject("expected value"));
            }
        }

        public static IEnumerable<TestCaseData> NegativeTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new JObject {["path"] = "expected value"},
                    new JObject {["path"] = "$.pathDoesn'tExist"});
                yield return new TestCaseData(
                    new JObject {["inner"] = new JObject {["path"] = "expected value"}},
                    new JObject {["path"] = "$.inner.pathDoesn'tExist"});
                yield return new TestCaseData(
                    new JObject {["inner"] = new JObject {["path"] = "expected value"}},
                    new JObject {["path"] = "$..pathDoesn'tExist"});
                yield return new TestCaseData(
                    new JObject {["inner"] = new JObject {["inner"] = new JObject {["path"] = "expected value"}}},
                    new JObject {["path"] = "$..[?(@.path)].pathDoesn'tExist"});
            }
        }

        public static IEnumerable<TestCaseData> PositiveTypeValidationTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new JObject {["path"] = "expected value"},
                    new JObject {["path"] = "$.path"},
                    JTokenType.String);
                yield return new TestCaseData(
                    new JObject {["path"] = 1},
                    new JObject {["path"] = "$.path"},
                    JTokenType.Integer);
                yield return new TestCaseData(
                    new JObject {["path"] = 1d},
                    new JObject {["path"] = "$.path"},
                    JTokenType.Float);
                yield return new TestCaseData(
                    new JObject {["path"] = true},
                    new JObject {["path"] = "$.path"},
                    JTokenType.Boolean);
                yield return new TestCaseData(
                    new JObject {["path"] = new JArray(1, 2, 3, 4)},
                    new JObject {["path"] = "$.path"},
                    JTokenType.Array);
                yield return new TestCaseData(
                    new JObject {["path"] = new JObject { }},
                    new JObject {["path"] = "$.path"},
                    JTokenType.Object);
            }
        }

        // multiple tokens match selected path should fail
        // no token match path should fail


        [TestCaseSource(nameof(PositiveTestCases))]
        public void Valueof_Valid_Json_Path_Should_Return_Expected_Value(JToken json, JObject conf, JToken expected)
        {
            // Act
            var sut = new ValueOfTransformer(conf);
            var result = sut.Transform(json);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCaseSource(nameof(NegativeTestCases))]
        public void Valueof_Invalid_Path_Should_Throw(JToken json, JObject conf)
        {
            // Act
            var sut = new ValueOfTransformer(conf);
            // Assert
            Assert.That(() => sut.Transform(json), Throws.Exception);
        }

        [TestCaseSource(nameof(PositiveTypeValidationTestCases))]
        public void Valueof_Should_Not_Change_Type(JToken json, JObject conf, JTokenType expected)
        {
            // Act
            var sut = new ValueOfTransformer(conf);
            var result = sut.Transform(json);

            // Assert
            Assert.That(result.Type, Is.EqualTo(expected));
        }

        [Test]
        public void Valueof_Should_Throw_Exception_If_Multiple_Values_Match_Path()
        {
            var json = new JObject
            {
                ["inner"] = new JObject
                {
                    ["path"] = "expected value",
                    ["inner"] = new JObject
                    {
                        ["path"] = "expected value"
                    }
                }
            };
            var conf = new JObject {["path"] = "$..[?(@.path)].path"};

            var sut = new ValueOfTransformer(conf);

            Assert.That(() => sut.Transform(json),
                Throws.InstanceOf<Newtonsoft.Json.JsonException>().With.Message
                    .EqualTo("Path returned multiple tokens."));
        }

        [Test]
        public void Valueof_Requires_Config()
        {
            Assert.That(() => new ValueOfTransformer(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Valueof_Does_Not_Allow_Bindings()
        {
            var sut = new ValueOfTransformer(new JObject {["path"] = "valid"});
            Assert.That(() => sut.Bind(Substitute.For<IJTokenTransformer>()), Throws.TypeOf<ValueProvidersCannotBeBoundException>());
        }
    }
}