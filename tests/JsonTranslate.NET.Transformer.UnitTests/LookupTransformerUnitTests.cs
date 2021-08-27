using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using JsonTranslate.NET.Core.Transformers;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests
{
    [TestFixture]
    public class LookupTransformerUnitTests
    {
        [Test, Combinatorial]
        public void Lookup_Contains_Key_Should_Return_Expected_Value(
            [Values(1, 'a', "abc", true, false, 1.2)] object source, 
            [Values(1, 'a', "abc", true, false, 1.2)] object target)
        {
            var config = new LookupTransformer.LookupConfig
            {
                Map = new() {new(JToken.FromObject(source), JToken.FromObject(target))}
            };

            var substitute = Substitute.For<IJTokenTransformer>();
            substitute.Transform(Arg.Any<JToken>(), Arg.Any<TransformationContext>()).Returns(JToken.FromObject(source));

            var sut = new LookupTransformer(JObject.FromObject(config));
            sut.Bind(substitute);
            
            var result = sut.Transform(new JObject());

            Assert.That(result, Is.EqualTo(JToken.FromObject(target)));
        }

        [Test]
        public void Lookup_Does_Not_Contain_Key_Should_Return_Default(
            [Values(1, 'a', "abc", true, false, 1.2)] object defaultValue)
        {
            var config = new LookupTransformer.LookupConfig()
            {
                Map = new() { new("this", "not it") },
                Default = JToken.FromObject(defaultValue),
                OnMissing = LookupTransformer.LookupConfig.HandleMissing.Default
            };

            var substitute = Substitute.For<IJTokenTransformer>();
            substitute.Transform(Arg.Any<JToken>(), Arg.Any<TransformationContext>()).Returns("not that");

            var sut = new LookupTransformer(JObject.FromObject(config));
            sut.Bind(substitute);

            var result = sut.Transform(new JObject());

            Assert.That(result, Is.EqualTo(config.Default));
        }

        [Test]
        public void Lookup_Does_Not_Contain_Key_Should_Return_Value(
            [Values(1, 'a', "abc", true, false, 1.2)] object value)
        {
            var config = new LookupTransformer.LookupConfig
            {
                Map = new() { new("this", "not it") },
                Default = "THIS IS DEFAULT VALUE",
                OnMissing = LookupTransformer.LookupConfig.HandleMissing.Value
            };
            var returnedValue = JToken.FromObject(value);
            var substitute = Substitute.For<IJTokenTransformer>();
            substitute.Transform(Arg.Any<JToken>(), Arg.Any<TransformationContext>()).Returns(returnedValue);

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
            var config = new LookupTransformer.LookupConfig
            {
                Map = new() { new("this", "not it") },
                Default = "THIS IS DEFAULT VALUE",
                OnMissing = LookupTransformer.LookupConfig.HandleMissing.Value
            };

            var sut = new LookupTransformer(JObject.FromObject(config));
            
            Assert.That(() => sut.Bind(Substitute.For<IJTokenTransformer>()), Throws.Nothing);
            Assert.That(() => sut.Bind(Substitute.For<IJTokenTransformer>()), Throws.InstanceOf<CanHaveOnlyOneBindingException>());
        }

        [Test]
        public void Lookup_Does_Not_Allow_Null_Binding()
        {
            var config = new LookupTransformer.LookupConfig
            {
                Map = new() { new("this", "not it") },
                Default = "THIS IS DEFAULT VALUE",
                OnMissing = LookupTransformer.LookupConfig.HandleMissing.Value
            };

            var sut = new LookupTransformer(JObject.FromObject(config));

            Assert.That(() => sut.Bind(null), Throws.TypeOf<CannotBindToNullException>());
        }
    }
}