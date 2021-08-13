using System;
using JsonTranslate.NET.Core.Transformers.TypeConverters;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.TypeConverters
{
    [TestFixture]
    public class ToStringTransformerUnitTests
    {
        [TestCase("true", "true")]
        [TestCase(true, "true")]
        [TestCase(1, "1")]
        [TestCase(1.1, "1.1")]
        public void Transform(object obj, string expected)
        {
            var cont = new UnitTransformer(new UnitTransformer.ConstConfig
            {
                Value = JToken.FromObject(obj)
            }.ToJObject());

            var transformer = new ToStringTransformer(new TransformationConfig().ToJObject());

            transformer.Bind(cont);

            var result = transformer.Transform(new JObject());
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Token.Value<string>(), Is.EqualTo(expected));
        }

        [Test]
        public void Transform_invalidType()
        {
            var cont = new UnitTransformer(new UnitTransformer.ConstConfig
            {
                Value = new JObject { ["a"] = "a" }
            }.ToJObject());

            var transformer = new ToStringTransformer(new TransformationConfig().ToJObject());

            transformer.Bind(cont);

            var result = transformer.Transform(new JObject());
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Error, Is.EqualTo("Can not transform Object to String"));
        }

        [Test]
        public void Transform_ToLog()
        {
            var log = LogHandler.CreateLog("tostring", new TransformationConfig());
            Assert.That(log, Is.EqualTo("Transforms a primitive value to a String"));
        }

        [TestCase(JSchemaType.String, true)]
        [TestCase(JSchemaType.Boolean, true)]
        [TestCase(JSchemaType.Number, true)]
        [TestCase(JSchemaType.Integer, true)]
        [TestCase(JSchemaType.Object, false)]
        [TestCase(JSchemaType.Array, false)]
        public void CanReceiveType(JSchemaType type, bool canReceive)
        {
            var transformer = new ToStringTransformer(new TransformationConfig().ToJObject());
            var result = transformer.CanReceiveType(type);
            Assert.That(result, Is.EqualTo(canReceive));
        }

        [Test]
        public void CannotHaveMultipleBinding()
        {
            var transformer = new ToStringTransformer(new TransformationConfig().ToJObject());

            transformer.Bind(transformer);

            Assert.That(() => transformer.Bind(transformer),
             Throws.InstanceOf<NotSupportedException>().With.Message
                 .EqualTo(ErrorMessages.TransformerOfTypeDoesNotSupportMultipleBindings
             .StringFormat("tostring")));
        }
    }
}