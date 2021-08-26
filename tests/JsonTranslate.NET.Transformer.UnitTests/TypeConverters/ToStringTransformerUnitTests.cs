using System;

using JsonTranslate.NET.Core.Transformers.TypeConverters;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.TypeConverters
{
    [TestFixture]
    public class ToStringTransformerUnitTests : GenericConverterTests<ToStringTransformer>
    {
        [TestCase("true", "true")]
        [TestCase(true, "true")]
        [TestCase(1, "1")]
        [TestCase(1.1, "1.1")]
        public override void Should_Successfully_Transform_Supported_Types<T, TExpected>(T obj, TExpected expected)
        {
            base.Should_Successfully_Transform_Supported_Types(obj, expected);
        }
    }
}