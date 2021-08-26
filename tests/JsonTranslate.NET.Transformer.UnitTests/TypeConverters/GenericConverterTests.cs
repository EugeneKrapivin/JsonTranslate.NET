using System;
using System.Collections.Generic;

using JsonTranslate.NET.Core.Abstractions;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.TypeConverters
{
    public abstract class GenericConverterTests<T>
        where T : IJTokenTransformer, new()
    {
        public virtual void Should_Successfully_Transform_Supported_Types<TValue, TExpected>(TValue obj, TExpected expected)
        {
            var sut = new T();

            sut.Bind(obj.AsTransformationResult());

            var result = sut.Transform(new JObject());
            
            Assert.That(result.Value<TExpected>(), Is.EqualTo(expected));
        }

        [TestCaseSource(nameof(NegativeSources))]
        public virtual void Transformation_With_Invalid_Type_Should_Throw(JToken invalid)
        {
            var sut = new T();

            sut.Bind(invalid.AsTransformationResult());

            Assert.That(() => sut.Transform(new JObject()), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        public static IEnumerable<JToken> NegativeSources => new JToken[]
        {
           // null, // todo: lets leave it out till I refactor to a Maybe<JToken> response
            new JObject(),
            new JArray(),
        };

        [Test]
        public void Casting_Operator_Can_Have_Only_One_Binding()
        {
            var sut = new T();

            sut.Bind(new T());

            Assert.That(() => sut.Bind(new T()), Throws.Exception);
        }
    }
}