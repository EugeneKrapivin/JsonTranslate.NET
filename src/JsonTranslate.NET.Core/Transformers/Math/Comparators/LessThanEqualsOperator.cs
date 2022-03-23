
using JsonTranslate.NET.Core.Abstractions;

using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Operators
{
    [Transformer("lte")]
    public class LessThanEqualsOperator : NumericBinOperator
    {
        protected override JToken OperateInternal(JToken loper, JToken roper) => loper.Value<decimal>() <= roper.Value<decimal>();
    }
}