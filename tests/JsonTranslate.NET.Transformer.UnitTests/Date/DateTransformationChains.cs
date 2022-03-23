using System;

using System.Globalization;

using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Transformers.Date;

using Newtonsoft.Json.Linq;

using NSubstitute;

using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.Date
{
    [TestFixture]
    public class DateTransformationChains
    {
        [TestCase("1970-01-01T00:00:00.000Z")]
        [TestCase("2021-08-19T15:29:40.123Z")]
        public void Transforming_ISO_To_Unix_Timestamp_And_Back_Should_Return_Initial_Value(string data)
        {
            var toTimestamp = new ToUnixTimestampTransformer();
            var toIso = new ToIsoDateTransformer();

            var provider = Substitute.For<IJTokenTransformer>();
            provider.Transform(Arg.Any<JToken>(), Arg.Any<TransformationContext>())
                .Returns(data);

            var chain = toIso.Bind(toTimestamp
                .Bind(provider));

            var result = chain.Transform(new JObject());

            var actual = DateTime.Parse(result.Value<string>());
            var expected = DateTime.Parse(data);

            Assert.That(DateTime.Compare(actual, expected), Is.EqualTo(0));
        }

        [TestCase(0)]
        [TestCase(1629376120)]
        public void Transforming_Unix_Timestamp_To_ISO_And_Back_Should_Return_Initial_Value(long data)
        {
            var toTimestamp = new ToUnixTimestampTransformer();
            var toIso = new ToIsoDateTransformer();

            var provider = Substitute.For<IJTokenTransformer>();
            provider.Transform(Arg.Any<JToken>(), Arg.Any<TransformationContext>())
                .Returns(data);

            var chain = toTimestamp.Bind(toIso
                .Bind(provider));

            var result = chain.Transform(new JObject());

            Assert.That(result.Value<long>(), Is.EqualTo(data));
        }

        [TestCase("dddd, dd MMMM yyyy", "Friday, 29 May 2015")]
        [TestCase("dddd, dd MMMM yyyy HH:mm", "Friday, 29 May 2015 05:50")]
        [TestCase("dddd, dd MMMM yyyy HH:mm tt", "Friday, 29 May 2015 05:50 AM")]
        [TestCase("dddd, dd MMMM yyyy H:mm", "Friday, 29 May 2015 5:50")]
        [TestCase("dddd, dd MMMM yyyy H:mm tt", "Friday, 29 May 2015 5:50 AM")]
        [TestCase("dddd, dd MMMM yyyy HH:mm:ss", "Friday, 29 May 2015 05:50:06")]
        [TestCase("MM/dd/yyyy HH:mm", "05/29/2015 05:50")]
        [TestCase("MM/dd/yyyy hh:mm tt", "05/29/2015 05:50 AM")]
        [TestCase("MM/dd/yyyy H:mm", "05/29/2015 5:50")]
        [TestCase("MM/dd/yyyy h:mm tt", "05/29/2015 5:50 AM")]
        [TestCase("MM/dd/yyyy HH:mm:ss", "05/29/2015 05:50:06")]
        [TestCase("yyyy-MM-ddTHH:mm:ss.fffffffK", "2015-05-16T05:50:06.7200000-04:00")]
        [TestCase("ddd, dd MMM yyyy HH':'mm':'ss 'GMT'", "Sat, 16 May 2015 05:50:06 GMT")]
        [TestCase("yyyy-MM-ddTHH:mm:ss", "2015-05-16T05:50:06")]
        public void Reformatting_A_Date_To_Iso_And_Then_To_Timestamp_And_Back_Again_By_Bilbo_Baggins(string source, string date)
        {
            var toTimestamp = new ToUnixTimestampTransformer();
            var toIso = new ToIsoDateTransformer();
            var datefmtIso = new DateToDateTransformer(new DateToDateTransformer.DateFormatTransformerConfig
            {
                SourceFormat = source,
                TargetFormat = "yyyy-MM-ddTHH:mm:ss.fffffffK",
            });

            var datefmtOrg = new DateToDateTransformer(new DateToDateTransformer.DateFormatTransformerConfig
            {
                SourceFormat = "yyyy-MM-ddTHH:mm:ss.fffffffK",
                TargetFormat = source
            });

            var provider = Substitute.For<IJTokenTransformer>();
            provider.Transform(Arg.Any<JToken>(), Arg.Any<TransformationContext>())
                .Returns(date);

            var chain =
                datefmtOrg.Bind(
                    toIso.Bind(
                        toTimestamp.Bind(
                            datefmtIso.Bind(
                                provider))));

            var result = chain.Transform(new JObject());

            var expected = DateTimeOffset.ParseExact(date, source, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.RoundtripKind);
            var actual = DateTimeOffset.ParseExact(result.Value<string>(), source, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.RoundtripKind);
            Assert.That(DateTimeOffset.Compare(actual, expected), Is.EqualTo(0), $"expected {expected:O} but was {actual:O}");
        }
    }
}