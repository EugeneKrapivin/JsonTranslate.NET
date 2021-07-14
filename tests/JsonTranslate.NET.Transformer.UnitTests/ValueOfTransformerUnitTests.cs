using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Transformers;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NSubstitute.Core.Arguments;
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
                    new JObject {["path"] = "$.pathDoesn'tExist"},
                    null);
                yield return new TestCaseData(
                    new JObject {["inner"] = new JObject {["path"] = "expected value"}},
                    new JObject {["path"] = "$.inner.pathDoesn'tExist"},
                    null);
                yield return new TestCaseData(
                    new JObject {["inner"] = new JObject {["path"] = "expected value"}},
                    new JObject {["path"] = "$..pathDoesn'tExist"},
                    null);
                yield return new TestCaseData(
                    new JObject {["inner"] = new JObject {["inner"] = new JObject {["path"] = "expected value"}}},
                    new JObject {["path"] = "$..[?(@.path)].pathDoesn'tExist"},
                    null);
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
        public void Valueof_Invalid_Path_Should_Return_Null(JToken json, JObject conf, JToken expected)
        {
            // Act
            var sut = new ValueOfTransformer(conf);
            var result = sut.Transform(json);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
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
        [Ignore("enforce config schema")]
        public void Valueof_Requires_Valid_Config()
        {
            Assert.That(() => new ValueOfTransformer(new JObject()), Throws.ArgumentException);
        }

        [Test]
        public void Valueof_Does_Not_Allow_Bindings()
        {
            var sut = new ValueOfTransformer(new JObject {["path"] = "valid"});
            Assert.That(() => sut.Bind(Substitute.For<IJTokenTransformer>()), Throws.TypeOf<NotSupportedException>());
        }
    }

    
    [TestFixture]
    public class LookupTransformerUnitTests
    {
        [Test, Combinatorial]
        public void Lookup_Contains_Key_Should_Return_Expected_Value(
            [Values(1, 'a', "abc", true, false, 1.2)] object source, 
            [Values(1, 'a', "abc", true, false, 1.2)] object target)
        {
            var config = new LookupTransformer.Config
            {
                Lookup = new() {new(JToken.FromObject(source), JToken.FromObject(target))}
            };

            var substitute = Substitute.For<IJTokenTransformer>();
            substitute.Transform(Arg.Any<JToken>()).Returns(JToken.FromObject(source));

            var sut = new LookupTransformer(JObject.FromObject(config));
            sut.Bind(substitute);
            
            var result = sut.Transform(new JObject());

            Assert.That(result, Is.EqualTo(JToken.FromObject(target)));
        }

        [Test]
        public void Lookup_Does_Not_Contain_Key_Should_Return_Default(
            [Values(1, 'a', "abc", true, false, 1.2)] object defaultValue)
        {
            var config = new LookupTransformer.Config
            {
                Lookup = new() { new("this", "not it") },
                Default = JToken.FromObject(defaultValue),
                OnMissing = LookupTransformer.Config.HandleMissing.Default
            };

            var substitute = Substitute.For<IJTokenTransformer>();
            substitute.Transform(Arg.Any<JToken>()).Returns("not that");

            var sut = new LookupTransformer(JObject.FromObject(config));
            sut.Bind(substitute);

            var result = sut.Transform(new JObject());

            Assert.That(result, Is.EqualTo(config.Default));
        }

        [Test]
        public void Lookup_Does_Not_Contain_Key_Should_Return_Value(
            [Values(1, 'a', "abc", true, false, 1.2)] object value)
        {
            var config = new LookupTransformer.Config
            {
                Lookup = new() { new("this", "not it") },
                Default = "THIS IS DEFAULT VALUE",
                OnMissing = LookupTransformer.Config.HandleMissing.Value
            };
            var returnedValue = JToken.FromObject(value);
            var substitute = Substitute.For<IJTokenTransformer>();
            substitute.Transform(Arg.Any<JToken>()).Returns(returnedValue);

            var sut = new LookupTransformer(JObject.FromObject(config));
            sut.Bind(substitute);

            var result = sut.Transform(new JObject());

            Assert.That(result, Is.EqualTo(returnedValue));
        }

        [Test]
        public void Lookup_Requires_Config()
        {
            Assert.That(() => new LookupTransformer(null), Throws.ArgumentNullException);
        }

        [Test]
        [Ignore("enforce config schema")]
        public void Lookup_Requires_Valid_Config()
        {
            Assert.That(() => new LookupTransformer(new JObject()), Throws.ArgumentException);
        }

        [Test]
        public void Lookup_Allows_Single_Binding()
        {
            var config = new LookupTransformer.Config
            {
                Lookup = new() { new("this", "not it") },
                Default = "THIS IS DEFAULT VALUE",
                OnMissing = LookupTransformer.Config.HandleMissing.Value
            };

            var sut = new LookupTransformer(JObject.FromObject(config));
            
            Assert.That(() => sut.Bind(Substitute.For<IJTokenTransformer>()), Throws.Nothing);
            Assert.That(() => sut.Bind(Substitute.For<IJTokenTransformer>()), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void Lookup_Does_Not_Allow_Null_Binding()
        {
            var config = new LookupTransformer.Config
            {
                Lookup = new() { new("this", "not it") },
                Default = "THIS IS DEFAULT VALUE",
                OnMissing = LookupTransformer.Config.HandleMissing.Value
            };

            var sut = new LookupTransformer(JObject.FromObject(config));

            Assert.That(() => sut.Bind(null), Throws.ArgumentNullException);
        }
    }
}