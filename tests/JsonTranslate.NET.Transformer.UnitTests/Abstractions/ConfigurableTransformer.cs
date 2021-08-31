using System;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using static JsonTranslate.NET.Transformer.UnitTests.Abstractions.TransformerCreationHelpers;

namespace JsonTranslate.NET.Transformer.UnitTests.Abstractions
{
    [TestFixtureSource(typeof(TransformersGenericSources), nameof(TransformersGenericSources.ConfigurableTransformers))]
    public class ConfigurableTransformer<T>
        where T : IJTokenTransformer
    {
        [Test]
        public void Config_Property_Should_Return_Expected_Config_Value()
        {
            var (sut, conf) = GetTransformerWithConfig<T>();

            Assert.That(JToken.DeepEquals(sut.Config, conf), 
@$"expected
{conf}
but got
{sut.Config}");
        }
    }
}