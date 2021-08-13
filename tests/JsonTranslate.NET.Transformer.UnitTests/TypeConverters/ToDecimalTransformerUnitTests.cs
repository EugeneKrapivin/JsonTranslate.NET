using System;
using JsonTranslate.NET.Core.Transformers.TypeConverters;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.TypeConverters
{
    [TestFixture]
    public class ToDecimalTransformerUnitTests
    {
        [TestCase("1.0")]
        [TestCase(true)]
        [TestCase(1)]
        [TestCase(1.0)]
        public void Transform(object obj)
        {
            var cont = new UnitTransformer(new UnitTransformer.ConstConfig
            {
                Value = JToken.FromObject(obj)
            }.ToJObject());

            var transformer = new ToDecimalTransformer(new TransformationConfig().ToJObject());

            transformer.Bind(cont);

            var result = transformer.Transform(new JObject());
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Token.Value<double>(), Is.EqualTo(1.0));
        }

        [Test]
        public void Transform_invalidType()
        {
            var cont = new UnitTransformer(new UnitTransformer.ConstConfig
            {
                Value = new JObject { ["a"] = "a" }
            }.ToJObject());

            var transformer = new ToDecimalTransformer(new TransformationConfig().ToJObject());

            transformer.Bind(cont);

            var result = transformer.Transform(new JObject());
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Error, Is.EqualTo("Can not transform Object to Decimal"));
        }

        [Test]
        public void CannotHaveMultipleBinding()
        {
            var transformer = new ToDecimalTransformer(new TransformationConfig().ToJObject());

            transformer.Bind(transformer);

            Assert.That(() => transformer.Bind(transformer),
             Throws.InstanceOf<NotSupportedException>().With.Message
                 .EqualTo(ErrorMessages.TransformerOfTypeDoesNotSupportMultipleBindings
             .StringFormat("todecimal")));
        }
    }
}