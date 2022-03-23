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
    public class EpochToIsoTransformerUnitTests
    {
        private static readonly DateTime _epoch = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static IEnumerable<TestCaseData> EpochTimes
        {
            get
            {
                yield return new TestCaseData("s", 0, _epoch);
                yield return new TestCaseData("s", 1, _epoch.AddSeconds(1));
                yield return new TestCaseData("s", 10, _epoch.AddSeconds(10));
                yield return new TestCaseData("s", int.MaxValue, _epoch.AddSeconds(int.MaxValue));

                yield return new TestCaseData("ms", 0, _epoch);
                yield return new TestCaseData("ms", 1, _epoch.AddMilliseconds(1));
                yield return new TestCaseData("ms", 10, _epoch.AddMilliseconds(10));
                yield return new TestCaseData("ms", int.MaxValue, _epoch.AddMilliseconds(int.MaxValue));
            }
        }

        public static IEnumerable<TestCaseData> NegativeEpochTimes
        {
            get
            {
                yield return new TestCaseData("s", -1, new DateTime(1969, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));
                yield return new TestCaseData("s", -10, new DateTime(1969, 12, 31, 23, 59, 50, 0, DateTimeKind.Utc));

                yield return new TestCaseData("ms", -1, new DateTime(1969, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc));
                yield return new TestCaseData("ms", -10, new DateTime(1969, 12, 31, 23, 59, 59, 990, DateTimeKind.Utc));
            }
        }

        [TestCaseSource(nameof(EpochTimes))]
        [TestCaseSource(nameof(NegativeEpochTimes))]
        public void Should_Return_Current_DateTime_When_Translating_From_Epoch(string unit, long add, DateTime expected)
        {
            var sut = new ToIsoDateTransformer(new ToIsoDateTransformer.ToIsoDateTransformerConfig
            {
                Units = unit
            });

            var data = add;

            var valueProvider = Substitute.For<IJTokenTransformer>();
            valueProvider.Transform(Arg.Any<JToken>(), Arg.Any<TransformationContext>())
                .Returns(data);

            sut.Bind(valueProvider);

            var actual = sut.Transform(new JObject());

            Assert.That(actual.Type, Is.EqualTo(JTokenType.String));
            Assert.That(actual.Value<string>(), Is.EqualTo(expected.ToString("O")));
        }

        [TestCase("s", 25, "1970-01-01T00:00:25.0000000Z")]
        [TestCase("ms", 25000, "1970-01-01T00:00:25.0000000Z")]
        public void Should_Return_ISO_CompatibleFormat(string unit, long add, string expected)
        {
            var sut = new ToIsoDateTransformer(new ToIsoDateTransformer.ToIsoDateTransformerConfig
            {
                Units = unit
            });

            var data = add;

            var valueProvider = Substitute.For<IJTokenTransformer>();
            valueProvider.Transform(Arg.Any<JToken>(), Arg.Any<TransformationContext>())
                .Returns(data);

            sut.Bind(valueProvider);

            var actual = sut.Transform(new JObject());

            Assert.That(actual.Type, Is.EqualTo(JTokenType.String));
            Assert.That(actual.Value<string>(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_Throw_Argument_Exception_On_Bad_Format()
        {
            var sut = new ToIsoDateTransformer();
            var failedTransformer = Substitute.For<IJTokenTransformer>();
            failedTransformer.Transform(Arg.Any<JToken>(), Arg.Any<TransformationContext>())
                .Returns("test");

            sut.Bind(failedTransformer);

            Assert.That(()=> sut.Transform(new JObject()), Throws.TypeOf<FormatException>());
        }

        [Test]
        public void Should_Add_Seconds_If_Unit_Config_Is_Invalid()
        {
            var sut = new ToIsoDateTransformer(new ToIsoDateTransformer.ToIsoDateTransformerConfig
            {
                Units = "not a valid unit"
            });
            var addValue = 10;
            var valueProvider = Substitute.For<IJTokenTransformer>();
            valueProvider.Transform(Arg.Any<JToken>(), Arg.Any<TransformationContext>())
                .Returns(addValue);

            sut.Bind(valueProvider);

            var response = sut.Transform(new JObject());

            var actual = response.Value<string>();
            var expected = _epoch.AddSeconds(addValue);

            Assert.That(actual, Is.EqualTo(expected.ToString("O")));
        }
    }
}
