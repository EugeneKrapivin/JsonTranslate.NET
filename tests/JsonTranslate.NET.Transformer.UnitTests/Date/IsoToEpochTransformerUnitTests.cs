using Newtonsoft.Json.Linq;

using NSubstitute;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Schema;
using JsonTranslate.NET.Core.Transformers.Date;
using JsonTranslate.NET.Core.Abstractions;

namespace JsonTranslate.NET.Transformer.UnitTests.Date
{
    [TestFixture]
    public class IsoToEpochTransformerUnitTests
    {
        private static readonly DateTime _epoch = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(_epoch, 0, "s");
                yield return new TestCaseData(new DateTime(2021, 8, 19, 13, 52, 30, 0, DateTimeKind.Utc), 1629381150, "s");

                yield return new TestCaseData(_epoch, 0, "ms");
                yield return new TestCaseData(new DateTime(2021, 8, 19, 13, 52, 30, 0, DateTimeKind.Utc), 1629381150000, "ms");
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void Should_Return_Current_UnixTimestamp_When_Translating_From_IsoDate(DateTime data, double expected, string unit)
        {
            var sut = new ToUnixTimestampTransformer(new ToUnixTimestampTransformer.ToUnixTimestampTransformerConfig
            {
                Units = unit
            });

            var valueProvider = Substitute.For<IJTokenTransformer>();
            valueProvider.Transform(Arg.Any<JToken>(), Arg.Any<TransformationContext>())
                .Returns(data.ToString("O"));

            sut.Bind(valueProvider);

            var actual = sut.Transform(new JObject());

            Assert.That(actual.Type, Is.EqualTo(JTokenType.Float));
            Assert.That(actual.Value<double>(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_Throw_Argument_Exception_On_Bad_Format()
        {
            var sut = new ToUnixTimestampTransformer();
            var failedTransformer = Substitute.For<IJTokenTransformer>();
            failedTransformer.Transform(Arg.Any<JToken>(), Arg.Any<TransformationContext>())
                .Returns("test");

            sut.Bind(failedTransformer);

            Assert.That(() => sut.Transform(new JObject()), Throws.ArgumentException);
        }

        [Test]
        public void Should_Fail_When_String_Is_Not_Valid_DateTime()
        {
            var sut = new ToUnixTimestampTransformer(new ToUnixTimestampTransformer.ToUnixTimestampTransformerConfig
            {
                Units = "ms"
            });

            var valueProvider = Substitute.For<IJTokenTransformer>();
            valueProvider.Transform(Arg.Any<JToken>(), Arg.Any<TransformationContext>())
                .Returns("not really a date");

            sut.Bind(valueProvider);

            Assert.That(() => sut.Transform(new JObject()), Throws.ArgumentException
                .With.Message.Match("Failed to parse string \"(?<str>.*)\" to date"));
        }
    }
}
