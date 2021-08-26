
using JsonTranslate.NET.Core.Abstractions;

using Newtonsoft.Json.Linq;

using NSubstitute;

namespace JsonTranslate.NET.Transformer.UnitTests.TypeConverters
{
    public static class TestHelpers
    {
        public static IJTokenTransformer AsTransformationResult<T>(this T source)
        {
            var sub = Substitute.For<IJTokenTransformer>();
            sub.Transform(Arg.Any<JToken>(), Arg.Any<TransformationContext>())
                .Returns(source switch
                {
                    JToken token => token,
                    { } x => JToken.FromObject(source),
                    null => null
                });

            return sub;
        }
    }
}