using System;
using System.Collections.Generic;

using JsonTranslate.NET.Core.Transformers.TypeConverters;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.TypeConverters
{
    [TestFixture]
    public class ToDecimalTransformerUnitTests : GenericConverterTests<ToDecimalTransformer>
    {
        [TestCaseSource(nameof(Positive))]
        public override void Should_Successfully_Transform_Supported_Types<T, TExpected>(T obj, TExpected expected)
        {
            base.Should_Successfully_Transform_Supported_Types(obj, expected);
        }

        public static IEnumerable<TestCaseData> Positive => new[]
        {
            new TestCaseData("1.0", 1M),
            new TestCaseData(true, 1M),
            new TestCaseData(1, 1m),
            new TestCaseData(1.0, 1m)
        };
    }
}