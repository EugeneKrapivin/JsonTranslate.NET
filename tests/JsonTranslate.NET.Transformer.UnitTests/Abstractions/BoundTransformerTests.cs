using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using NUnit.Framework;
using static JsonTranslate.NET.Transformer.UnitTests.Abstractions.TransformerCreationHelpers;

namespace JsonTranslate.NET.Transformer.UnitTests.Abstractions
{
    [TestFixture(Description = "Test base properties of all transformers that support binding")]
    [TestFixtureSource(typeof(TransformersGenericSources), nameof(TransformersGenericSources.BoundTransformers))]
    public class BoundTransformerTests<T> 
        where T : IJTokenTransformer
    {
        [Test]
        public void Should_Not_Allow_To_Bind_To_Self()
        {
            var sut = GetTransformer<T>();

            Assert.That(() => sut.Bind(sut),
                Throws.TypeOf<CannotBindToSelfException>());
        }

        [Test]
        public void Should_Not_Allow_Cyclic_Bindings()
        {
            var first = GetBindingFor<T>();
            var second = GetBindingFor<T>();
            second.Bind(first);

            Assert.That(() => first.Bind(second), Throws.TypeOf<BindingCreateGraphCycleException>());
        }

        [Test]
        public void Should_Not_Allow_Complex_Cyclic_Bindings()
        {
            var first = GetBindingFor<T>();
            var second = GetBindingFor<T>();
            var third = GetBindingFor<T>();
            var fourth = GetBindingFor<T>();
            var fifth = GetBindingFor<T>();

            second.Bind(third);
            third.Bind(fourth);
            fourth.Bind(fifth);
            fifth.Bind(first);

            Assert.That(() => first.Bind(second), Throws.TypeOf<BindingCreateGraphCycleException>());
        }

        [Test]
        public void Should_Not_Allow_Binding_Nulls()
        {
            var sut = GetTransformer<T>();

            Assert.That(() => sut.Bind(null),
                Throws.TypeOf<CannotBindToNullException>());
        }
    }

}