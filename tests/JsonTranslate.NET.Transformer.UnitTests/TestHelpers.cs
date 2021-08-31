using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Transformers;
using Newtonsoft.Json.Linq;
using NSubstitute;

namespace JsonTranslate.NET.Transformer.UnitTests
{
    public static class TestHelpers
    {
        public static IJTokenTransformer AsUnit<T>(this T source)
        {
            return new UnitTransformer(new JObject {["value"] = JToken.FromObject(source)});
        }

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