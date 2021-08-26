using JsonTranslate.NET.Core.Transformers.TypeConverters;

using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.TypeConverters
{

    [TestFixture]
    public class ToIntegerTransformerUnitTests : GenericConverterTests<ToIntegerTransformer>
    {
        [TestCase("1", 1)]
        [TestCase(true, 1)]
        [TestCase(1, 1)]
        [TestCase(1.0, 1)]
        public override void Should_Successfully_Transform_Supported_Types<T, TExpected>(T obj, TExpected expected)
        {
            base.Should_Successfully_Transform_Supported_Types(obj, expected);
        }
    }
}