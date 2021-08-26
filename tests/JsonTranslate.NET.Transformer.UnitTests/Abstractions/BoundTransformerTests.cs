using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JsonTranslate.NET.Core;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.Abstractions
{
    public static class AllBoundTransformers
    {
        public static IEnumerable<Type> BoundTransformers
            => from type in Assembly.GetAssembly(typeof(TransformerFactory)).DefinedTypes
                    let attr = type.GetCustomAttribute<TransformerAttribute>()
                    where type.ImplementedInterfaces.Any(x => x == typeof(IJTokenTransformer))
                          && attr is not null
                          && type.BaseType != typeof(ValueProvidingTransformer)
                    select type;
    }

    [TestFixture(typeof(SinglyBoundTestTransformer))]
    [TestFixture(typeof(MultiBoundTestTransformer))]
    [TestFixtureSource(typeof(AllBoundTransformers), nameof(AllBoundTransformers.BoundTransformers))]
    public class BoundTransformerTests<T> 
        where T : IJTokenTransformer, new()
    {
        [Test]
        public void Should_Not_Allow_Binding_Nulls()
        {
            var sut = new T();

            Assert.That(() => sut.Bind(null),
                Throws.TypeOf<CannotBindToNullException>());
        }

        [Test]
        public void Should_Not_Allow_To_Bind_To_Self()
        {
            var sut = new T();

            Assert.That(() => sut.Bind(sut),
                Throws.TypeOf<CannotBindToSelfException>());
        }

        [Test]
        public void Should_Not_Allow_Cyclic_Bindings()
        {
            var first = new T();
            var second = new T();
            second.Bind(first);

            Assert.That(() => first.Bind(second), Throws.Exception);
        }

        [Test]
        public void Should_Not_Allow_Complex_Cyclic_Bindings()
        {
            var first = new T();
            var second = new T();
            var third = new T();
            var fourth = new T();
            var fifth = new T();

            second.Bind(third);
            third.Bind(fourth);
            fourth.Bind(fifth);
            fifth.Bind(first);

            Assert.That(() => first.Bind(second), Throws.Exception);
        }
    }
}