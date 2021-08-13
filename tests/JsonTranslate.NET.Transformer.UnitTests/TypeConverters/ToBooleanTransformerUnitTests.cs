using System;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Exceptions;
using JsonTranslate.NET.Core.Transformers;
using JsonTranslate.NET.Core.Transformers.TypeConverters;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.TypeConverters
{
    [TestFixture]
    public class ToBooleanTransformerUnitTests
    {
        private static IJTokenTransformer GetUnitOf<T>(T obj)
        {
            var unit = Substitute.For<IJTokenTransformer>();
            unit.Transform(Arg.Any<JToken>()).Returns(JToken.FromObject(obj));
            
            return unit;
        }

        [TestCase("true")]
        [TestCase(true)]
        [TestCase(1)]
        [TestCase(1.0)]
        public void Should_Successfully_Transform_Supported_Types(object obj)
        {
            var transformer = new ToBooleanTransformer();

            transformer.Bind(GetUnitOf(obj));

            var result = transformer.Transform(new JObject());
            Assert.That(result.Value<bool>(), Is.EqualTo(true));
        }

        [Test]
        public void Should_Fail_On_Unsupported_Types()
        {
            var unit = GetUnitOf(new JObject { ["a"] = "a" }); ;

            var transformer = new ToBooleanTransformer();

            transformer.Bind(unit);

            var result = transformer.Transform(new JObject());
            Assert.That(result, Is.EqualTo("Can not transform Object to Boolean"));
        }

        [Test]
        public void Cannot_Have_Multiple_Bindings()
        {
            var transformer = new ToBooleanTransformer();

            transformer.Bind(GetUnitOf(1));

            Assert.That(() => transformer.Bind(GetUnitOf(1)),
             Throws.InstanceOf<TransformerBindingException>().With.Message
                 .EqualTo("Cannot bind to self"
             ));
        }
    }
}