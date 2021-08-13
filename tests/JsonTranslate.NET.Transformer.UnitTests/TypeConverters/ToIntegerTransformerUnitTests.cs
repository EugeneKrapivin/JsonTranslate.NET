using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.TypeConverters
{
    [TestFixture]
    public class ToIntegerTransformerUnitTests
    {
        [TestCase("1")]
        [TestCase(true)]
        [TestCase(1)]
        [TestCase(1.0)]
        public void Transform(object obj)
        {
            var cont = new UnitTransformer(new UnitTransformer.ConstConfig
            {
                Value = JToken.FromObject(obj)
            }.ToJObject());

            var transformer = new ToIntegerTransformer(new TransformationConfig().ToJObject());

            transformer.Bind(cont);

            var result = transformer.Transform(new JObject());
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Token.Value<int>(), Is.EqualTo(1));
        }

        [Test]
        public void Transform_invalidType()
        {
            var cont = new UnitTransformer(new UnitTransformer.ConstConfig
            {
                Value = new JObject { ["a"] = "a" }
            }.ToJObject());

            var transformer = new ToIntegerTransformer(new TransformationConfig().ToJObject());

            transformer.Bind(cont);

            var result = transformer.Transform(new JObject());
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Error, Is.EqualTo("Can not transform Object to integer"));
        }

        [Test]
        public void Transform_ToLog()
        {
            var log = LogHandler.CreateLog("tointeger", new TransformationConfig());

            Assert.That(log, Is.EqualTo("Transforms a primitive value to a Integer"));
        }

        [TestCase(JSchemaType.String, true)]
        [TestCase(JSchemaType.Boolean, true)]
        [TestCase(JSchemaType.Number, true)]
        [TestCase(JSchemaType.Integer, true)]
        [TestCase(JSchemaType.Object, false)]
        [TestCase(JSchemaType.Array, false)]
        public void CanReceiveType(JSchemaType type, bool canReceive)
        {
            var transformer = new ToIntegerTransformer(new TransformationConfig().ToJObject());
            var result = transformer.CanReceiveType(type);
            Assert.That(result, Is.EqualTo(canReceive));
        }

        [Test]
        public void CannotHaveMultipleBinding()
        {
            var transformer = new ToIntegerTransformer(new TransformationConfig().ToJObject());

            transformer.Bind(transformer);

            Assert.That(() => transformer.Bind(transformer),
             Throws.InstanceOf<NotSupportedException>().With.Message
                 .EqualTo(ErrorMessages.TransformerOfTypeDoesNotSupportMultipleBindings
             .StringFormat("tointeger")));
        }
    }
}