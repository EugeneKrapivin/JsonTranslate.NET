using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Transformers.Operators;

using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Math.Operators
{
    [Transformer("add")]
    public class AdditionOperator : AbstractBinOperator
    {
        protected override JToken Operate(JToken loper, JToken roper)
        {
            var r = roper.Value<decimal>();
            var l = loper.Value<decimal>();

            return l + r;
        }
    }
}
