using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using NUnit.Framework;
using static JsonTranslate.NET.Transformer.UnitTests.Abstractions.TransformerCreationHelpers;

namespace JsonTranslate.NET.Transformer.UnitTests.Abstractions
{
    [TestFixture(typeof(MultiBoundTestTransformer))]
    [TestFixtureSource(typeof(TransformersGenericSources), nameof(TransformersGenericSources.MultiBoundTransformers))]
    public class MultiBoundTransformerUnitTests<T> where T : IJTokenTransformer
    {
        [Test]
        public void Should_Allow_Binding_More_Than_Once()
        {
            var sut = GetTransformer<T>();
            
            sut.Bind(GetBindingFor<T>());

            Assert.That(() => sut.Bind(GetBindingFor<T>()), Throws.Nothing);
        }
    }

    [TestFixture(typeof(SinglyBoundTestTransformer))]
    [TestFixtureSource(typeof(TransformersGenericSources), nameof(TransformersGenericSources.SinglyBoundTransformers))]
    public class SinglyBoundTransformerUnitTests<T> where T : IJTokenTransformer
    {
        [Test]
        public void Should_Allow_Binding_Exactly_One_Binding()
        {
            var sut = GetTransformer<T>();
            
            sut.Bind(GetBindingFor<T>());

            Assert.That(() => sut.Bind(GetBindingFor<T>()), Throws.TypeOf<CanHaveOnlyOneBindingException>());
        }
    }
}