using System;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using JsonTranslate.NET.Core.Transformers;
using JsonTranslate.NET.Core.Transformers.TypeConverters;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.TypeConverters
{
    [TestFixture]
    public class ToBooleanTransformerUnitTests : GenericConverterTests<ToBooleanTransformer>
    {
        [TestCase("true", true)]
        [TestCase(true, true)]
        [TestCase(1, true)]
        [TestCase(1.0, true)]
        public override void Should_Successfully_Transform_Supported_Types<T, TExpected>(T obj, TExpected expected)
        {
            base.Should_Successfully_Transform_Supported_Types(obj, expected);
        }
    }
}