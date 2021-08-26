using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.Abstractions
{
    [TestFixture]
    public class MultiBoundTransformerUnitTests
    {
        [Test]
        public void Should_Allow_Binding_More_Than_Once()
        {
            var sut = new MultiBoundTestTransformer();
            
            sut.Bind(new MultiBoundTestTransformer());

            Assert.That(() => sut.Bind(new MultiBoundTestTransformer()), Throws.Nothing);
        }
    }
}