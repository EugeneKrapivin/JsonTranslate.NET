using System;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.Abstractions
{
    [TestFixtureSource(typeof(TransformersGenericSources), nameof(TransformersGenericSources.ValueProvidingTransformers))]
    public class ValueProvidingTransformerTests<T> where T : IJTokenTransformer
    {
        private JObject megaConfig = new()
        {
            ["path"] = "$.path",
            ["value"] = "some value"
        };

        [Test]
        public void Should_Not_Allow_Bindings()
        {
            var sut = (T)Activator.CreateInstance(typeof(T), megaConfig);

            Assert.That(() => sut.Bind(new SinglyBoundTestTransformer()), Throws.TypeOf<ValueProvidersCannotBeBoundException>());
        }
    }
}